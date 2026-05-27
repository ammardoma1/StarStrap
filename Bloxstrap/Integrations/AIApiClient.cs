using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StarStrap.Enums;

namespace StarStrap.Integrations
{
    public class AIApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> GetAdviceAsync(AIAgentProvider provider, string apiKey, string systemPrompt, string base64Image)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "API Key is missing. Please configure it in Settings.";

            try
            {
                return provider switch
                {
                    AIAgentProvider.OpenAI => await GetOpenAIAdvice(apiKey, systemPrompt, base64Image),
                    AIAgentProvider.Gemini => await GetGeminiAdvice(apiKey, systemPrompt, base64Image),
                    AIAgentProvider.Anthropic => await GetAnthropicAdvice(apiKey, systemPrompt, base64Image),
                    AIAgentProvider.Groq => await GetGroqAdvice(apiKey, systemPrompt, base64Image),
                    _ => "Unsupported provider."
                };
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine("AIAgent", $"Error getting advice: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        private static async Task<string> GetOpenAIAdvice(string apiKey, string systemPrompt, string base64Image)
        {
            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = "What should I do next?" },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                        }
                    }
                },
                max_tokens = 300
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "No response";
        }

        private static async Task<string> GetGeminiAdvice(string apiKey, string systemPrompt, string base64Image)
        {
            var payload = new
            {
                systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = "What should I do next?" },
                            new { inlineData = new { mimeType = "image/jpeg", data = base64Image } }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest:generateContent?key={apiKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? "No response";
        }

        private static async Task<string> GetAnthropicAdvice(string apiKey, string systemPrompt, string base64Image)
        {
            var payload = new
            {
                model = "claude-3-5-sonnet-20241022",
                max_tokens = 300,
                system = systemPrompt,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "image", source = new { type = "base64", media_type = "image/jpeg", data = base64Image } },
                            new { type = "text", text = "What should I do next?" }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
            request.Headers.Add("x-api-key", apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString() ?? "No response";
        }

        private static async Task<string> GetGroqAdvice(string apiKey, string systemPrompt, string base64Image)
        {
            var payload = new
            {
                model = "llama-3.2-90b-vision-preview",
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = "What should I do next?" },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                        }
                    }
                },
                max_tokens = 300
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "No response";
        }
    }
}
