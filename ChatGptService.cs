namespace Namespace
{
    public class ChatGptService
    {
        private readonly HttpClient _httpClient;

        public ChatGptService(HttpClient httpClient) => _httpClient = httpClient;

        public string? GetChatGptResponse(string userInput)
        {
            return null;
        }
    }
}