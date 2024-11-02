using RmlServer;

var builder = WebApplication.CreateBuilder(args);

// Define allowed origins for CORS
var allowedOrigins = builder.Environment.IsDevelopment()
    ? new[] { "http://localhost:32785" }
    : new[] { "https://rmltools.com" };

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS to allow only the specific base addresses
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add SignalR services
builder.Services.AddSignalR();

// Add support for serving Blazor WebAssembly static files
builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger setup removed
}

// Use CORS with the specific origins policy
app.UseCors("AllowSpecificOrigins");

// Serve static files for the Blazor WebAssembly app
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Map the SignalR hub endpoint
app.MapHub<RenderHub>("/RenderHub");

// Map fallback to the Blazor WebAssembly app
app.MapFallbackToFile("index.html");

app.Run();
