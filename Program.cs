using Microsoft.AspNetCore.Rewrite;
using MyWebApp.Interfaces;
using MyWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

# region services
// Scoped (new service for each request)
builder.Services.AddScoped<IWelcomeService, WelcomeService>();
# endregion

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
app.MapGet("/", (IWelcomeService welcomeService) => welcomeService.GetWelcomeMessage());
app.MapGet("/welcomes", (IWelcomeService welcomeService1, IWelcomeService welcomeService2) =>
{
  string message1 = welcomeService1.GetWelcomeMessage();
  string message2 = welcomeService2.GetWelcomeMessage();

  return $"{message1}\n{message2}";
});
app.MapGet("/about", () => "Contoso was founded in 2020");
# endregion

app.Run();
