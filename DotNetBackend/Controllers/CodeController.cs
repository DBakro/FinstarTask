using DotNetBackend.ApiModels;
using DotNetBackend.Extensions;
using Microsoft.AspNetCore.Mvc;
using NpgsqlData;
using NpgsqlData.Models;
using NpgsqlData.Stores;

namespace DotNetBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly ICodeStore _codeStore;

        public CodeController(ICodeStore codeStore)
        {
            _codeStore = codeStore;
        }

        [HttpGet]
        public JsonResult GetList(string? search = null, int? limit = null, int? offset = null)
        {
            CodeListModel codeList = _codeStore.GetCodeList(search, limit, offset);
            return new JsonResult(codeList);
        }

        [HttpPost]
        public IActionResult Post(IList<CodeInputModel> codeInputs)
        {
            if (!ModelState.IsValid)
            {
                string errorMsg = ModelState.GetModelStateErrorsJsonString();
                return BadRequest(errorMsg);
            }

            IEnumerable<ICode> codes = codeInputs.Select((c, index) => new CodeModel(c.Code, c.Value));

            SqlResult sqlResult = _codeStore.CreateList(codeInputs);
            if (!sqlResult.Succeeded)
            {
                sqlResult.AddErrorsToModelState(ModelState);
                string errorMsg = ModelState.GetModelStateErrorsJsonString();
                return BadRequest(errorMsg);
            }

            return Ok();
        }
    }
}