using System.Collections.Concurrent;

public class ChatBot
{
    private readonly System.Net.Http.HttpClient httpClient;

    public ChatBot()
    {
    }

    public ChatBot(System.Net.Http.HttpClient httpClient) => this.httpClient = httpClient;

    public ChatBot(object v) => V = v;

    public object V { get; }

    public async Task<string> GetAnswerAsync(string question, double temperature, int maxTokens, Func<string, Task<float[]>> generateEmbeddingAsync, Func<float[], string> getClosestEmbedding, List<string[]> qaPairs, ConcurrentDictionary<string, float[]> embeddings)
    {
        float[] questionEmbedding = await generateEmbeddingAsync(question);
        string closestEmbedding = getClosestEmbedding(questionEmbedding);
        if (!string.IsNullOrEmpty(closestEmbedding))
        {
            return closestEmbedding;
        }
        else
        {
            string answer = await GetAnswerAsync(question, temperature, maxTokens, generateEmbeddingAsync, getClosestEmbedding, qaPairs, embeddings);
            qaPairs.Add(new string[] { question, answer });
            float[] embedding = await generateEmbeddingAsync(answer);
            embeddings[answer] = embedding;

            return answer;
        }
    }

    internal Task<string> GetAnswerAsync(string question, double temperature, int maxTokens)
    {
        throw new NotImplementedException();
    }
}