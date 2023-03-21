
using System.Threading.Tasks;

namespace SmartSchoolsV2.Views
{
    public interface INumberPickerDialog
    {
        Task<(bool, int)> ShowPicker(string title, string okButtonText, string cancelButtonText, NumberPickerOptions options);
    }
}
