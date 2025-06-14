var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "UP" }));

app.Run();

// Created for testing purposes
public partial class Program { }