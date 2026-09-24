using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("LocalDBConnectionString");
builder.Services.AddDbContext<StockPicksDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IStockPicksRepository, StockPicksRepository>();
builder.Services.AddScoped<IStockPicksService, StockPicksService>();
builder.Services.AddScoped<IPickReasonsRepository, PickReasonsRepository>();
builder.Services.AddScoped<IPickReasonsService, PickReasonsService>();
//builder.Services.AddHttpClient<IMarketDataService, MarketDataService>();
builder.Services.AddScoped<IMarketDataService, MarketDataService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(p =>
    p.WithOrigins("http://localhost:5173", "http://localhost:5174")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.UseHttpsRedirection();

app.mapStockPicksEndpoints();
app.mapPickReasonsEndpoints();

app.Run();
