using System.Collections.Concurrent;

// dotnet-minimal-tasks-api
// A clean ASP.NET Core 8 minimal API over an in-memory task list.
// Dependency-free: no database, no third-party packages — just idiomatic
// endpoint routing, records as DTOs, and the Results helpers.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var tasks = new ConcurrentDictionary<Guid, TaskItem>();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", utc = DateTime.UtcNow }));

app.MapGet("/tasks", () => tasks.Values.OrderBy(t => t.CreatedUtc));

app.MapGet("/tasks/{id:guid}", (Guid id) =>
    tasks.TryGetValue(id, out var task) ? Results.Ok(task) : Results.NotFound());

app.MapPost("/tasks", (CreateTask request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var item = new TaskItem(Guid.NewGuid(), request.Title.Trim(), Done: false, DateTime.UtcNow);
    tasks[item.Id] = item;
    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapPost("/tasks/{id:guid}/complete", (Guid id) =>
{
    if (!tasks.TryGetValue(id, out var task))
        return Results.NotFound();

    var updated = task with { Done = true };
    tasks[id] = updated;
    return Results.Ok(updated);
});

app.MapDelete("/tasks/{id:guid}", (Guid id) =>
    tasks.TryRemove(id, out _) ? Results.NoContent() : Results.NotFound());

app.Run();

record TaskItem(Guid Id, string Title, bool Done, DateTime CreatedUtc);
record CreateTask(string Title);
