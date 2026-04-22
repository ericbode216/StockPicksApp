using Microsoft.AspNetCore.Mvc;

public static class ExtensionsStockPicksMapping
{
    public static void mapStockPicksEndpoints(this WebApplication app)
    {
        app.MapGet(
            "/test",
            () =>
            {
                return Results.Ok("Test Successful");
            }
        );
        app.MapGet(
            "/stocks",
            async (IStockPicksRepositiory repository) =>
            {
                return Results.Ok(await repository.GetAll());
            }
        );
        app.MapGet(
            "/stocks/{stockId:int}",
            async (int stockId, IStockPicksRepositiory repository) =>
            {
                var stockPick = await repository.Get(stockId);
                if (stockPick == null)
                {
                    return Results.Problem($"Stock with ID {stockId} not found", statusCode: 404);
                }
                else
                {
                    return Results.Ok(stockPick);
                }
            }
        );
        app.MapPost(
            "/stocks",
            async ([FromBody] StockPickAddDto stockPickDto, IStockPicksRepositiory repository) =>
            {
                var stockAdded = await repository.Add(stockPickDto);
                return stockAdded;
            }
        );
        app.MapPut(
            "/stocks",
            async ([FromBody] StockPickUpdateDto stockPickDto, IStockPicksRepositiory repository) =>
            {
                var stockPick = await repository.Get(stockPickDto.Id);
                if (stockPick == null)
                {
                    return Results.Problem(
                        $"Stock with ID {stockPickDto.Id} not found",
                        statusCode: 404
                    );
                }
                else
                {
                    var updatedStockPick = await repository.Update(stockPickDto);
                    return Results.Ok(updatedStockPick);
                }
            }
        );
        app.MapDelete(
            "/stocks/{stockId:int}",
            async (int stockId, IStockPicksRepositiory repository) => 
        {
            var entityDeleted = await repository.Delete(stockId);
            return Results.Ok(entityDeleted);
        });
    }
}
