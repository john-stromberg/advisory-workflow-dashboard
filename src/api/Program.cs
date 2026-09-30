using ClientMeetingPrep.Api.Application.Contracts;
using ClientMeetingPrep.Api.Infrastructure.Adapters;
using ClientMeetingPrep.Api.Infrastructure.Persistence;
using ClientMeetingPrep.Api.Infrastructure.Registry;
using ClientMeetingPrep.Api.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("client-meeting-prep"));

builder.Services.AddScoped<IMeetingPacketService, MeetingPacketService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IModuleRegistryService, ModuleRegistryService>();
builder.Services.AddScoped<IClientDataAdapter, MockClientDataAdapter>();
builder.Services.AddScoped<IPortfolioDataAdapter, MockPortfolioDataAdapter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
