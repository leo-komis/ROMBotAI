using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ROMBot_AI.Pages
{
    public class ChatModel : PageModel
    {
        private readonly IChatGptService _chatGptService;

        // Property for user input
        [BindProperty]
        public string UserInput { get; set; }

        // Property for chat history
        public List<string> ChatHistory { get; set; }

        // Constructor
        public ChatModel(IChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
            ChatHistory = new List<string>();
        }
        // Method to handle form submission (e.g., when the user sends a message)
        public async Task<IActionResult> OnPostAsync()
        {
            // Add user input to chat history
            ChatHistory.Add($"User: {UserInput}");

            // Process the user input, e.g., send it to the ChatGPT model and get a response
            string chatGptResponse = await _chatGptService.GetChatGptResponseAsync(UserInput);

            // Add ChatGPT response to chat history
            ChatHistory.Add($"ChatGPT: {chatGptResponse}");

            // Clear user input for the next message
            UserInput = string.Empty;

            return Page();
        }
    }
}