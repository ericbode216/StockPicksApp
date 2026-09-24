using Microsoft.AspNetCore.Mvc;

public static class ExtensionsPickReasonsMapping
{
    public static void mapPickReasonsEndpoints(this WebApplication app)
    {
        
        app.MapGet(
            "/api/reasons",
            async (IPickReasonsService pickReasonsService) =>
            {
                return Results.Ok(await pickReasonsService.GetAll());
            }
        );
        app.MapGet(
            "/api/stock-picks/{stockPickId:int}/reasons",
            async (int stockPickId, IPickReasonsService pickReasonsService) =>
            {
                return Results.Ok(await pickReasonsService.GetByStockId(stockPickId));
            }
        );
        app.MapPost(
            "/api/stock-picks/{stockPickId:int}/reasons",
            async([FromBody] PickReasonDto pickReason, int stockPickId, IPickReasonsService pickReasonsService) =>
            {
                return Results.Ok(await pickReasonsService.Add(pickReason));
            }
        );
    }
}