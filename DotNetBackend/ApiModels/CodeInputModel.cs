using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.ApiModels
{
    public class CodeInputModel
    {
        [Display(Name = "Код")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "{0} должен быть в диапазоне от {1} до {2}")]
        [Required(ErrorMessage = "{0} должен быть указано")]
        public int Code { get; set; }

        [Display(Name = "Значение")]
        [Required(ErrorMessage = "{0} должно быть указано")]
        public string? Value { get; set; }
    }
}