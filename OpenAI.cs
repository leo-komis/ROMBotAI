namespace ROMBotAI
{
    public class OpenAI
    {
        public string apiKey;

        public OpenAI(string apiKey) => this.apiKey = "ROMBOT_API_KEY";

        public object Engines { get; internal set; }
    }
}