using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NpgsqlData
{
    public class SqlResult: ISqlResult
    {
        public SqlResult()
        {
            Succeeded = true;
        }

        public SqlResult(params SqlError[] errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        public static SqlResult Success
        {
            get
            {
                return new();
            }
        }

        public bool Succeeded { get; protected set; }

        public IEnumerable<SqlError>? Errors { get; }

        public string GetErrorsString()
        {
            if (Errors == null)
            {
                return string.Empty;
            }

            return string.Join(';', Errors);
        }

        public void AddErrorsToModelState(ModelStateDictionary modelState)
        {
            if (Succeeded || Errors == null)
            {
                return;
            }
            foreach (SqlError sqlError in Errors)
            {
                modelState.AddModelError(sqlError.Column, sqlError.Description);

            }
        }

        public static SqlResult Failed(params SqlError[] errors)
        {
            return new SqlResult(errors);
        }
    }
}