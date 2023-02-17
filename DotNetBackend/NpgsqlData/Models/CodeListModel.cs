namespace NpgsqlData.Models
{
    public class CodeListModel
    {
        public CodeListModel(uint total, IList<ICode> codes)
        {
            Total = total;
            Rows = codes;
        }

        public uint Total { get; private set; }
        public IList<ICode> Rows { get; private set; }
    }
}