namespace ROMBot_AI.Pages
{
    public interface IChatGptService
    {
        Task<string> GetChatGptResponseAsync(string userInput);
    }
}