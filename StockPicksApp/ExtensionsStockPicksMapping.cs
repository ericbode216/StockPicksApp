using Microsoft.AspNetCore.Mvc;

public static class ExtensionsStockPicksMapping
{
    public static void mapStockPicksEndpoints(this WebApplication app)
    {
        app.MapGet(
            "/api/stock-picks",
            async (IStockPicksService service) =>
            {
                return Results.Ok(await service.GetAll());
            }
        );
        app.MapGet(
            "api/stock-picks{stockId:int}",
            async (int stockId, IStockPicksService service) =>
            {
                var stockPick = await service.GetById(stockId);
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
            "/api/stock-picks",
            async ([FromBody] StockPickAddDto stockPickDto, IStockPicksService service) =>
            {
                try{
                    var stockAdded = await service.Add(stockPickDto);
                    return Results.Ok(stockAdded);
                }catch(ArgumentException e){
                    Console.WriteLine(e.Message);
                    return  Results.BadRequest(e.Message);
                }
            }
        );
        app.MapPut(
            "/api/stock-picks",
            async ([FromBody] StockPickUpdateDto stockPickDto, IStockPicksService service) =>
            {
                var stockPick = await service.GetById(stockPickDto.Id);
                if (stockPick == null)
                {
                    return Results.Problem(
                        $"Stock with ID {stockPickDto.Id} not found",
                        statusCode: 404
                    );
                }
                else
                {
                    var updatedStockPick = await service.Update(stockPickDto);
                    return Results.Ok(updatedStockPick);
                }
            }
        );
        
        app.MapDelete(
            "/api/stock-picks/{stockId:int}",
            async (int stockId, IStockPicksService service) => 
        {
            Console.WriteLine("Inside mapping");
            var entityDeleted = await service.Delete(stockId);
            return Results.Ok(entityDeleted);
        });
    }
}
