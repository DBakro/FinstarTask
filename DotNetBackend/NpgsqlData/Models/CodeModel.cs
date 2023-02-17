namespace NpgsqlData.Models
{
    public class CodeModel : ICode
    {
        public CodeModel(int code, string value)
        {
            Code = code;
            Value = value;
        }

        public CodeModel(int id, int code, string value)
        {
            Id = id;
            Code = code;
            Value = value;
        }

        public int Id { get; private set; }
        public int Code { get; private set; }
        public string Value { get; private set; }
    }
}