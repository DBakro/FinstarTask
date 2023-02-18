using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NpgsqlData
{
    public interface ISqlResult
    {
        bool Succeeded { get; }

        void AddErrorsToModelState(ModelStateDictionary modelState);
    }
}