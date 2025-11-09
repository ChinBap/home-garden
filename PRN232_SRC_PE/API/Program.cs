using Microsoft.EntityFrameworkCore;
using Q1;
using Q1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PePrnFall22B1Context>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();