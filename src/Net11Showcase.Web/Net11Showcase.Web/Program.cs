using Net11Showcase.Web.Client.Pages;
using Net11Showcase.Web.Components;
using Net11Showcase.Shared.Services;

// Ensure content root always resolves to the location containing wwwroot
var baseDir = AppContext.BaseDirectory;
string contentRoot;

if (Directory.Exists(Path.Combine(baseDir, "wwwroot")))
{
    contentRoot = baseDir;
}
else if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")))
{
    contentRoot = Directory.GetCurrentDirectory();
}
else
{
    var candidate = Path.Combine(Directory.GetCurrentDirectory(), "src", "Net11Showcase.Web", "Net11Showcase.Web");
    contentRoot = Directory.Exists(Path.Combine(candidate, "wwwroot")) ? candidate : Directory.GetCurrentDirectory();
}

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = contentRoot,
    WebRootPath = Path.Combine(contentRoot, "wwwroot")
});

// Add services to the container.
builder.Services.AddSingleton<ShowcaseDataService>();
builder.Services.AddScoped<Net11Showcase.Components.AgentOps.IAgentTransport, Net11Showcase.Components.AgentOps.SimulatedAgentTransport>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    })
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
// In standalone local loopback mode, avoid HSTS and HTTPS redirection so HTTP WebSockets (ws://) connect without browser blocking
var isHttpsConfigured = builder.Configuration["urls"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) == true;
if (isHttpsConfigured && !app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Net11Showcase.Web.Client._Imports).Assembly);

app.Run();
