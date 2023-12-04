var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Configurar a aplicação.
var app = builder.Build();

// Configurar o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Adicionar os controllers ao pipeline.
app.MapControllers();

app.Run();
