namespace NpgsqlData.Models
{
    public interface ICode
    {
        public int Id { get; }
        public int Code { get; }
        public string Value { get; }
    }
}
