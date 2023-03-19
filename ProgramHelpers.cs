using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

internal static class ProgramHelpers
{
    private static ChatBot ChatBot = new ChatBot();
    private static List<string[]> QAPairs = new List<string[]>();
    private static Dictionary<string, float[]> embedding = new Dictionary<string, float[]>();

    public static async Task<string> GetAnswerAsync(string question, double temperature, int maxTokens, HttpClient httpClient)
    {
        float[] questionEmbedding = await GenerateEmbeddingAsync(question);
        string closestEmbedding = GetClosestEmbedding(questionEmbedding);

        if (!string.IsNullOrEmpty(closestEmbedding))
        {
            return closestEmbedding;
        }
        else
        {
            string answer = await GetAnswerFromAPIAsync(question, temperature, maxTokens, httpClient);
            QAPairs.Add(new string[] { question, answer });
            float[] answerEmbedding = await GenerateEmbeddingAsync(answer);
            embedding[answer] = answerEmbedding;
            return answer;
        }
    }

    private static string GetClosestEmbedding(float[] questionEmbedding)
    {
        float maxSimilarity = float.MinValue;
        string closestEmbedding = string.Empty;
        object syncLock = new();

        Parallel.ForEach(embedding, kvp =>
        {
            float similarity = CosineSimilarity(questionEmbedding, kvp.Value);

            lock (syncLock)
            {
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    closestEmbedding = kvp.Key;
                }
            }
        });

        return closestEmbedding;
    }

    private static float CosineSimilarity(float[] questionEmbedding, float[] value)
    {
        throw new NotImplementedException();
    }

    private static Task<float[]> GenerateEmbeddingAsync(string text)
    {
        throw new NotImplementedException();
    }

    public static async Task<string> GetAnswerFromAPIAsync(string question, double temperature, int maxTokens, HttpClient httpClient)
    {
        // Prepare the API request
        var request = new
        {
            engine = "text-davinci-003", // Replace with the appropriate engine
            prompt = question,
            max_tokens = maxTokens,
            n = 1,
            stop = (string[])null,
            temperature = temperature
        };

        // Send the request
        StringContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        var response = await httpClient.PostAsync("https://api.openai.com/v1/engines/text-davinci-003/completions", content);

        // Process the response
        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObject = JObject.Parse(responseJson);
        string answer = responseObject["choices"][0]["text"].ToString().Trim();

        return answer;
    }
}