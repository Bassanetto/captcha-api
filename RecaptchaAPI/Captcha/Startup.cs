using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Este método é chamado durante o tempo de execução. Use este método para adicionar serviços ao contêiner.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
    {
        options.AddPolicy("MyAllowedOrigins",
            policy =>
            {
                policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
        services.AddControllers();
        services.AddHttpClient();


        // Configuração do Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Minha API",
                Version = "v1",
                Description = "Descrição da API"
            });
        });
    }

    // Este método é chamado durante o tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("MyAllowedOrigins");

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // Middleware para o Swagger somente em ambiente de desenvolvimento
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
            });
        }

        // Outras configurações de middleware...

        app.UseRouting();

        // Outras configurações de roteamento...

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}