using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace testeTicketTech.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string OLLAMA_URL = "http://localhost:11434/api/generate";
        private const string MODEL_NAME = "llama3.2";

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { error = "Mensagem não pode estar vazia" });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromMinutes(2); // Aumenta timeout para 2 minutos

                var ollamaRequest = new
                {
                    model = MODEL_NAME,
                    prompt = request.Message,
                    stream = false
                };

                var json = JsonSerializer.Serialize(ollamaRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(OLLAMA_URL, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    // Log para debug - REMOVA depois de testar
                    Console.WriteLine($"Resposta Ollama: {responseBody}");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody, options);

                    if (ollamaResponse != null && !string.IsNullOrWhiteSpace(ollamaResponse.Response))
                    {
                        return Ok(new { response = ollamaResponse.Response });
                    }
                    else
                    {
                        return Ok(new { response = "Resposta vazia do modelo" });
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { error = "Erro ao comunicar com Ollama" });
                }
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new { error = "Ollama não está rodando. Inicie com 'ollama serve'" });
            }
            catch (TaskCanceledException)
            {
                return StatusCode(500, new { error = "Timeout: A IA demorou muito para responder" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erro: {ex.Message}" });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class OllamaResponse
    {
        public string Model { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public bool Done { get; set; }
    }
}