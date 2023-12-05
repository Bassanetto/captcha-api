using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/captcha")]
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
    public async Task<IActionResult> VerificarRecaptcha([FromBody] CaptchaRequest request)
    {
        var chaveSecreta = _configuration["Recaptcha:SecretKey"];
        var resposta = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={chaveSecreta}&response={request.token}", null);

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

public class CaptchaRequest
{
    public string token { get; }

    public CaptchaRequest(string token)
    {
        this.token = token;
    }
}