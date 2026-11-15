using ControlEnviosCourier.Application.Models;
using ControlEnviosCourier.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Context;
using ControlEnviosCourier.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errores = actionContext.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Datos inválidos." : e.ErrorMessage);
        return new BadRequestObjectResult(ServiceResult<object>.ErrorResponse(string.Join(" ", errores)));
    };
});
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<PaqueteRepository>();
builder.Services.AddScoped<GenericRepository<Envio>>();
builder.Services.AddScoped<GenericRepository<EstadoEnvio>>();

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PaqueteService>();
builder.Services.AddScoped<EstadoEnvioService>();
builder.Services.AddScoped<EnvioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
