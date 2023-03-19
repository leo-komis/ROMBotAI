using ROMBotAI;

internal class OpenAIClient : OpenAIClientBase
{
    public OpenAIClient(OpenAIApiClientConfiguration openAIApiClientConfiguration) => OpenAIClient.openAIApiClientConfiguration = openAIApiClientConfiguration;
}
