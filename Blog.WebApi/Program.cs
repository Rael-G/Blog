using Blog.Persistance;
using Blog.Application;
using Blog.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigurePersistance(builder.Configuration);
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureAuth();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.InitializeDb();
app.SeedDb(builder.Configuration);

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
