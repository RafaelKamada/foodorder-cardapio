using Application.Handlers;
using Application.Queries;
using Application.Repositories.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MongoDB
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ??
        builder.Configuration["MongoDbSettings:ConnectionString"];
    options.DatabaseName = builder.Configuration["MongoDbSettings:DatabaseName"] ?? "FoodOrder_Cardapio";
});

// Configuração do MongoDB Context
builder.Services.AddScoped<IMongoDbContext, MongoDbContext>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);
    return new MongoDbContext(database);
});

// Configuração do MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ObterTodosProdutosQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ObterTodosProdutosHandler).Assembly);
});

// Configuração dos repositórios
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
//builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IImagemRepository, ImagemRepository>();


builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.Use(async (context, next) =>
{
    await Task.Delay(2000); // Espera 2 segundos
    await next();
});

app.MapHealthChecks("/health");

// Configuração do Swagger
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();