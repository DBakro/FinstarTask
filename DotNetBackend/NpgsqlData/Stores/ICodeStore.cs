using DotNetBackend.ApiModels;
using Npgsql;
using NpgsqlData.Models;

namespace NpgsqlData.Stores
{
    public interface ICodeStore
    {
        IList<ICode> GetCodes(string? search = null, int? limit = null, int? offset = null, NpgsqlConnection? connection = null);

        uint GetCount(string? search = null, NpgsqlConnection? connection = null);

        ICodeList GetCodeList(string? search = null, int? limit = null, int? offset = null);

        ISqlResult Create(ICode codeModel, NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);

        ISqlResult CreateList(IList<CodeInputModel> codeInputs);

        ISqlResult DeleteAll(NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);
    }
}