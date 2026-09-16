# Blog.Blazor

The frontend for the blog — Blazor WebAssembly, MudBlazor. Talks to
`Blog.API` over HTTP, and to `blog-notifications` over a SignalR
connection for live comment notifications (a separate, independently
deployed service — see
`../learning-notes/notes/44-notifications-service-hub-move.md`); it
doesn't run anything on its own.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- `Blog.API` and `blog-notifications` running first — see
  `../blog/README.md` (its Docker Compose setup runs both, plus
  RabbitMQ). By default this app points at the Docker Compose ports:
  `http://localhost:8080` for `Blog.API`, `http://localhost:8081` for
  `blog-notifications`.

## Getting started

```bash
cd Blog.Blazor
dotnet run
```

Opens at `https://localhost:7279` (the `https` launch profile). The base
URLs come from `wwwroot/appsettings.Development.json` — if your local
services run on different ports, update `ApiBaseUrl`/`NotificationsBaseUrl`
there. Without `NotificationsBaseUrl` configured, live comment
notifications are silently skipped rather than erroring — everything
else still works.

## Running tests

```bash
dotnet test Blog.Blazor.Test
```

## Configuration & environments

`wwwroot/appsettings.{Development,Staging,Production}.json` — which one
loads is decided by `dotnet run`'s dev server locally (always
`Development`), or the `blazor-environment` meta tag in
`wwwroot/index.html` once published as static files. Full reasoning in
`../learning-notes/notes/35-environments-config.md`.

## Learn more

This project is part of a running C#/.NET learning series alongside
`Blog.API` — see `../learning-notes/` (`./build.sh` there generates a PDF
of the whole thing).
