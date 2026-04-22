using Microsoft.AspNetCore.Mvc;

public static class ExtensionsPickReasonsMapping
{
    public static void mapPickReasonsEndpoints(this WebApplication app)
    {
        app.MapGet(
            "/test2",
            () =>
            {
                return Results.Ok("Test 2 Successful");
            }
        );
        app.MapGet(
            "/allpickreasons",
            async (IPickReasonsRepository repository) =>
            {
                return Results.Ok(await repository.GetAll());
            }
        );
        app.MapGet(
            "/stockpick/{stockPickId:int}/pickreasons",
            async (int stockPickId, IPickReasonsRepository repository) =>
            {
                return Results.Ok(await repository.Get(stockPickId));
            }
        );
        app.MapPost(
            "/stockpick/{stockPickId:int}/pickreasons",
            async([FromBody] PickReasonEntity pickReason, int stockPickId, IPickReasonsRepository repository) =>
            {
                return Results.Ok(await repository.Add(pickReason));
            }
        );
    }
}