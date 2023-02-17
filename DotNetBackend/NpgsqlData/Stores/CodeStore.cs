using DotNetBackend;
using DotNetBackend.ApiModels;
using Npgsql;
using NpgsqlData.Models;
using NpgsqlData.Repositories;

namespace NpgsqlData.Stores
{
    public class CodeStore : ICodeStore
    {
        private const string ErrorMessage = "Ошибка добавления записей: {0}";
        private readonly ICodeRepository _codeRepository;
        private readonly ILogger _logger;
        private readonly string? _connectionString;

        public CodeStore(ILoggerFactory loggerFactory)
        {
            _connectionString = Startup.ConnectionString;
            _codeRepository = new CodeRepository(_connectionString);
            _logger = loggerFactory.CreateLogger<CodeStore>();
        }

        public IList<ICode> GetCodes(string? search = null, int? limit = null, int? offset = null, NpgsqlConnection? connection = null)
        {
            return _codeRepository.GetCodes(search, limit, offset, connection);
        }

        public uint GetCount(string? search = null, NpgsqlConnection? connection = null)
        {
            return _codeRepository.GetCount(search, connection);
        }

        public CodeListModel GetCodeList(string? search = null, int? limit = null, int? offset = null)
        {
            using (NpgsqlConnection connection = new (_connectionString))
            {
                connection.Open();

                IList<ICode> codeModels = new List<ICode>();

                uint count = GetCount(search, connection);

                if (count == 0)
                {
                    return new CodeListModel(0, codeModels);
                }

                codeModels = GetCodes(search, limit, offset, connection);
                if (!codeModels.Any())
                {
                    return new CodeListModel(0, codeModels);
                }

                return new CodeListModel(count, codeModels);
            }
        }

        public SqlResult Create(ICode codeModel, NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null)
        {
            if (codeModel == null)
            {
                return SqlResult.Failed(new SqlError("Данные не указаны"));
            }
            if (string.IsNullOrEmpty(codeModel.Value))
            {
                return SqlResult.Failed(new SqlError("Данные не указаны", "value"));
            }

            int rowsCreated = _codeRepository.Insert(codeModel, connection, transaction);
            return rowsCreated > 0 ? SqlResult.Success : SqlResult.Failed(new SqlError("Не удалось добавить запись: code - {0}, value - {1}", codeModel.Code, codeModel.Value));
        }

        public SqlResult CreateList(IList<CodeInputModel> codeInputs)
        {
            if (!codeInputs?.Any() ?? true)
            {
                return SqlResult.Failed(new SqlError("Данные не указаны"));
            }

            NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction();

            try
            {
                SqlResult sqlResult = DeleteAll(connection, transaction);
                if (!sqlResult.Succeeded)
                {
                    return sqlResult;
                }

                IEnumerable<ICode> codesOrdered = codeInputs.OrderBy(c => c.Code).Select((c, index) => new CodeModel(index + 1, c.Code, c.Value));

                for (int i = 0; i < codesOrdered.Count(); i++)
                {
                    sqlResult = Create(codesOrdered.ElementAt(i), connection, transaction);
                    if (!sqlResult.Succeeded)
                    {
                        return sqlResult;
                    }
                }

                transaction.Commit();
                return SqlResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorMessage, ex);
                return SqlResult.Failed(new SqlError("Ошибка добавления записей"));
            }
            finally
            {
                connection.Dispose();
            }
        }

        public SqlResult DeleteAll(NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null)
        {
            _codeRepository.Delete(connection, transaction);
            return SqlResult.Success;
        }
    }
}