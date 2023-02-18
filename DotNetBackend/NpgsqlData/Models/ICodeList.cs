namespace NpgsqlData.Models
{
    public interface ICodeList
    {
        uint Total { get; }
        IList<ICode> Rows { get; }
    }
}