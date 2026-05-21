using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

# region middleware
// Log request/response info
app.Use(async (context, next) =>
{
  await next();

  Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
});

// Reroute old page to new
app.UseRewriter(new RewriteOptions().AddRedirect("history", "about"));
# endregion

# region endpoints
app.MapGet("/", () => "Welcome to Contoso");
app.MapGet("/about", () => "Contoso was founded in 2020");
# endregion

app.Run();
