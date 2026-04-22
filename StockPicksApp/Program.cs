using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("LocalDBConnectionString");
builder.Services.AddDbContext<StockPicksDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IStockPicksRepositiory, StockPicksRepository>();
builder.Services.AddScoped<IPickReasonsRepository, PickReasonsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(p => p.WithOrigins("http://localhost:5174").AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.MapGet(
    "/",
    () =>
    {
        return "Hello World!";
    }
);
app.mapStockPicksEndpoints();
app.mapPickReasonsEndpoints();

app.Run();
