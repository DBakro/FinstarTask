using Npgsql;
using NpgsqlData.Models;

namespace NpgsqlData.Repositories
{
    public interface ICodeRepository
    {
        int Delete(NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);
        IList<ICode> GetCodes(string? search, int? limit, int? offset, NpgsqlConnection? connection = null);
        uint GetCount(string? search, NpgsqlConnection? connection = null);
        int Insert(ICode codeModel, NpgsqlConnection? connection = null, NpgsqlTransaction? transaction = null);
    }
}