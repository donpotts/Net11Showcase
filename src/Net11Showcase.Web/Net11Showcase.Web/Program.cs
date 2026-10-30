using Net11Showcase.Web.Client.Pages;
using Net11Showcase.Web.Components;
using Net11Showcase.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ShowcaseDataService>();
// Agent Ops demo agent (offline). Register your own IAgentTransport instead to drive a live agent.
builder.Services.AddScoped<Net11Showcase.Components.AgentOps.IAgentTransport, Net11Showcase.Components.AgentOps.SimulatedAgentTransport>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Net11Showcase.Web.Client._Imports).Assembly);

app.Run();
