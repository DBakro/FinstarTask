using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace DotNetBackend.Extensions
{
    public static class ModelStateStringBuilder
    {
        public static string GetModelStateErrorsJsonString(this ModelStateDictionary modelState)
        {
            IEnumerable<KeyValuePair<string, string[]>> errors = modelState.IsValid
                ? null
                : modelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(v => v.ErrorMessage).ToArray())
                .Where(m => m.Value.Any());

            StringBuilder output = new("{");

            if(errors != null)
            {
                foreach (KeyValuePair<string, string[]> error in errors)
                {
                    output.Append($"\"{error.Key.Replace(".", "")}\": \"{string.Join(",", error.Value)}\"");
                }
            }

            output.Append('}');

            return output.ToString();
        }
    }
}