using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JSON serializer 
builder.Services.AddControllers().AddNewtonsoftJson(Options=>
Options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    Options=>Options.SerializerSettings.ContractResolver=new DefaultContractResolver());

var app = builder.Build();

// Enable CORS
app.UseCors(c=>c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
