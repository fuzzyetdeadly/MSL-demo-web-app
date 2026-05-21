using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

# region middleware
// Reroute old page to new
app.UseRewriter(new RewriteOptions().AddRedirect("history", "about"));
# endregion

# region endpoints
app.MapGet("/", () => "Welcome to Contoso");
app.MapGet("/about", () => "Contoso was founded in 2020");
# endregion

app.Run();
