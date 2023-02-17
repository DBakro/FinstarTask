using Npgsql;
using NpgsqlData.Models;
using System.Data;
using System.Text;

namespace NpgsqlData.Repositories
{
    public class CodeRepository: ICodeRepository
    {
        private readonly string? _connectionString;

        public CodeRepository(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<ICode> GetCodes(string? search, int? limit, int? offset, NpgsqlConnection? connection = null)
        {
            StringBuilder sqlStringBuilder = new("SELECT id, code, value FROM codes");

            Dictionary<string, object> parameters = new();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sqlStringBuilder.Append(" WHERE code::text like @Search OR value ilike @Search");
                parameters.Add("@Search", $"%{search.Trim()}%");
            }

            if (limit != null && offset != null)
            {
                sqlStringBuilder.Append(" LIMIT @Limit OFFSET @Offset");
                parameters.Add("@Limit", (int)limit);
                parameters.Add("@Offset", (int)offset);
            }

            connection ??= new NpgsqlConnection(_connectionString);

            IList<ICode> codeModels = new List<ICode>();

            connection ??= new NpgsqlConnection(_connectionString);

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(connection, CommandType.Text, sqlStringBuilder.ToString(), parameters))
            {
                while (reader.Read())
                {
                    codeModels.Add(GetModel(reader));
                }
            }

            return codeModels;
        }

        public uint GetCount(string? search, NpgsqlConnection? connection = null)
        {
            StringBuilder sqlStringBuilder = new("SELECT COUNT(id) FROM codes");

            Dictionary<string, object> parameters = new();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sqlStringBuilder.Append(" WHERE code::text like @Search OR value ilike @Search");
                parameters.Add("@Search", $"%{search.Trim()}%");
            }

            uint count = 0;

            connection ??= new NpgsqlConnection(_connectionString);

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(connection, CommandType.Text, sqlStringBuilder.ToString(), parameters))
            {
                if (reader.Read())
                {
                    count = Convert.ToUInt32(reader[0]);
                }
            }

            return count;
        }

        public int Insert(ICode codeModel, NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null)
        {
            string sql = @"INSERT INTO codes (id, code, value)
                           VALUES (@Id, @Code, @Value)";

            Dictionary<string, object> parameters = new()
            {
                { "@Id", codeModel.Id },
                { "@Code", codeModel.Code },
                { "@Value", codeModel.Value }
            };

            connection ??= new NpgsqlConnection(_connectionString);
            int rowsCreated = NpgsqlHelper.ExecuteNonQuery(connection, CommandType.Text, sql, parameters, transaction);
            return rowsCreated;
        }

        public int Delete(NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null)
        {
            string sql = "DELETE FROM codes";

            connection ??= new NpgsqlConnection(_connectionString);
            int rowsDeleted = NpgsqlHelper.ExecuteNonQuery(connection, CommandType.Text, sql, null, transaction);
            return rowsDeleted;
        }

        private static ICode GetModel(IDataReader reader)
        {
            return new CodeModel(Convert.ToInt16(reader["id"]), Convert.ToInt16(reader["code"]), reader["value"].ToString());
        }
    }
}