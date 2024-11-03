using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var misReglasCors = "ReglasCors";
builder.Services.AddCors(option => option.AddPolicy(name: misReglasCors,
    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(misReglasCors);
app.UseAuthorization();

app.MapControllers();

app.Run();





/* pasos para ejecutar la api 

1) entrar al archivo appsettings.json y cambiar su cadena de conexion 
2) ejecutar el proyecto y dejarlo abierto...... una vez abierto abrir el  html que tiene el front para controlar que funcione*/