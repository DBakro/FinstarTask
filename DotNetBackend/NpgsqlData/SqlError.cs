namespace NpgsqlData
{
    public class SqlError
    {
        public SqlError(string description, string column)
        {
            Column = column;
            Description = description;
        }

        public SqlError(string description, params object[] msgParams)
        {
            Column = "All";
            Description = string.Format(description, msgParams);
        }

        public string Column { get; set; }
        public string Description { get; set; }
    }
}