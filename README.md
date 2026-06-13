# dotnet-minimal-tasks-api

A clean **ASP.NET Core 8 minimal API** over an in-memory task list — a small, dependency-free sample of idiomatic .NET endpoint design.

No database, no third-party packages: just minimal-API routing, `record` DTOs, and the `Results` helpers. Easy to read, easy to extend into a real service.

## Endpoints

| Method | Route | Description |
| ------ | ----- | ----------- |
| `GET` | `/health` | Liveness check |
| `GET` | `/tasks` | List tasks (oldest first) |
| `GET` | `/tasks/{id}` | Get one task |
| `POST` | `/tasks` | Create a task — body: `{ "title": "..." }` |
| `POST` | `/tasks/{id}/complete` | Mark a task done |
| `DELETE` | `/tasks/{id}` | Delete a task |

## Run

```bash
dotnet run
```

Then:

```bash
curl http://localhost:5000/health

# create
curl -X POST http://localhost:5000/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Ship the feature"}'

# list
curl http://localhost:5000/tasks
```

(The exact port is printed at startup.)

## Notes

State is in-memory (`ConcurrentDictionary`), so it resets on restart — the point is to show clean, minimal endpoint structure, not persistence. Swapping the dictionary for a repository + EF Core / Dapper is a natural next step.

## License

MIT
