using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ROMBotAI.Pages
{
    public class ChatModel : PageModel
    {
        [BindProperty]
        public string Question { get; set; } = null!;

        public string Answer { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Answer = await ProgramHelpers.GetAnswerAsync(Question, 0.7, 1024, new System.Net.Http.HttpClient());

            return Page();
        }
    }
}