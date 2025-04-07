using Application.Repositories.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Configuração do MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configuração dos repositórios
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IImagemRepository, ImagemRepository>();

// Configuração do MongoDB Context
builder.Services.AddSingleton<MongoDbContext>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDbContext(settings);
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();