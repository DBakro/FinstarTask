using DotNetBackend.ApiModels;
using Npgsql;
using NpgsqlData.Models;

namespace NpgsqlData.Stores
{
    public interface ICodeStore
    {
        IList<ICode> GetCodes(string? search = null, int? limit = null, int? offset = null, NpgsqlConnection? connection = null);
      
        uint GetCount(string? search = null, NpgsqlConnection? connection = null);

        CodeListModel GetCodeList(string? search = null, int? limit = null, int? offset = null);
      
        SqlResult Create(ICode codeModel, NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);

        SqlResult CreateList(IList<CodeInputModel> codeInputs);

        SqlResult DeleteAll(NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);
    }
}