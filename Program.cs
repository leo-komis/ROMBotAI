using FastText.NetWrapper;
using MongoDB.Bson.IO;
using Namespace;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ROMBotAI
{
    public static class Program
    {
        private static readonly ConcurrentDictionary<string, float[]> Embeddings = new ConcurrentDictionary<string, float[]>();
        private static readonly List<string[]> QAPairs = new List<string[]>();
        private static object httpClient;
        private static readonly string APIKey = "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";

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ChatGptService>(client =>
            {
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIKey);
            });
        }

        public static async Task LoadEmbeddingsAsync(IWebHostEnvironment hostingEnvironment)
        {
            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "embeddings.csv");
            using StreamReader reader = new StreamReader(filePath);
            string embeddingsText = await reader.ReadToEndAsync();

            IEnumerable<string> lines = embeddingsText
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1);

            foreach (string? line in lines)
            {
                string[] fields = line.Split(',');
                string key = fields[0];
                float[] values = fields.Skip(1).Select(f => float.Parse(f)).ToArray();
                Embeddings[key] = values;
            }
        }

        private static string GetClosestEmbedding(float[] questionEmbedding)
        {
            float maxSimilarity = float.MinValue;
            string closestEmbedding = string.Empty;
            object syncLock = new();

            Parallel.ForEach(Embeddings, kvp =>
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

        private static async Task<float[]> GenerateEmbeddingAsync(string text, string v)
        {
            return await GenerateEmbeddingAsync(text, "application/json");
        }

        private static float[] ConvertCompletionToEmbeddings(string completion)
        {
            string modelPath = "cc.en.300.bin"; // Path to the downloaded FastText model

            using FastTextWrapper fastText = new FastTextWrapper();
            fastText.LoadModel(modelPath);

            string[] words = completion.Split(' '); // Tokenize the text into words
            float[] embeddings = new float[300]; // Initialize the embeddings array (size depends on the model)

            foreach (string word in words)
            {
                float[] wordEmbedding = fastText.GetWordVector(word);
                for (int i = 0; i < embeddings.Length; i++)
                {
                    embeddings[i] += wordEmbedding[i];
                }
            }
            // Normalize the embeddings
            float norm = 0;
            for (int i = 0; i < embeddings.Length; i++)
            {
                norm += embeddings[i] * embeddings[i];
            }
            norm = (float)Math.Sqrt(norm);
            for (int i = 0; i < embeddings.Length; i++)
            {
                embeddings[i] /= norm;
            }

            return embeddings;
        }
        private static float CosineSimilarity(float[] embedding1, float[] embedding2)
        {
            int simdLength = Vector<float>.Count;
            int length = embedding1.Length;
            int remainder = length % simdLength;

            Vector<float> dotProductVector = Vector<float>.Zero;
            for (int i = 0; i < length - remainder; i += simdLength)
            {
                dotProductVector += new Vector<float>(embedding1, i) * new Vector<float>(embedding2, i);
            }

            float dotProduct = 0;
            for (int i = 0; i < simdLength; i++)
            {
                dotProduct += dotProductVector[i];
            }

            for (int i = length - remainder; i < length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
            }

            float magnitude1 = (float)Math.Sqrt(embedding1.Sum(x => Math.Pow(x, 2)));
            float magnitude2 = (float)Math.Sqrt(embedding2.Sum(x => Math.Pow(x, 2)));

            float similarity = dotProduct / (magnitude1 * magnitude2);

            return similarity;
        }
        public static void Main(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices((_, services) => ConfigureServices(services));

            builder.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

            dotnIHost host = (dotnIHost)builder.Build();
            host.Run();
        }

        internal static object GetChatBot()
        {
            throw new NotImplementedException();
        }

        internal static Task<string> GetAnswerAsync(string question, double temperature, int maxTokens)
        {
            throw new NotImplementedException();
        }
    }

    internal class dotnIHost
    {
    }

    internal class OpenAIClient
    {
        private readonly OpenAIApiClientConfiguration openAIApiClientConfiguration;

        public OpenAIClient(OpenAIApiClientConfiguration openAIApiClientConfiguration) => this.openAIApiClientConfiguration = openAIApiClientConfiguration;
    }

    public class ChatBot
    {
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
}