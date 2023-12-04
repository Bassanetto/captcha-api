using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RecaptchaController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public RecaptchaController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("verificar-recaptcha")]
    public async Task<IActionResult> VerificarRecaptcha([FromBody] string token)
    {
        var chaveSecreta = _configuration["Recaptcha:SecretKey"];
        var resposta = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={chaveSecreta}&response={token}", null);

        if (resposta.IsSuccessStatusCode)
        {
            // A resposta do reCAPTCHA é válida
            return Ok(new { Success = true });
        }
        else
        {
            // A resposta do reCAPTCHA é inválida
            return BadRequest(new { Success = false, Message = "Falha na verificação do reCAPTCHA." });
        }
    }
}