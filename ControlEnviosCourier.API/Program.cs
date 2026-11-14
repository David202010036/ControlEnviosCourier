using ControlEnviosCourier.Domain.Entities;
using ControlEnviosCourier.Infrastructure.Context;
using ControlEnviosCourier.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<PaqueteRepository>();
builder.Services.AddScoped<GenericRepository<Envio>>();
builder.Services.AddScoped<GenericRepository<EstadoEnvio>>();

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
