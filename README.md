# Blog.Blazor

The frontend for the blog — Blazor WebAssembly, MudBlazor. Talks to
`Blog.API` over HTTP, and to `blog-notifications` over a SignalR
connection for live comment notifications — in the Docker Compose setup,
both go through `blog-gateway` (a single address; this app has no idea a
gateway exists — see `../learning-notes/notes/45-api-gateway-in-practice.md`
and `44-notifications-service-hub-move.md`). It doesn't run anything on
its own.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- `Blog.API`, `blog-notifications`, and `blog-gateway` running first —
  see `../blog/README.md` (its Docker Compose setup runs all three, plus
  RabbitMQ). By default this app points at the gateway's Docker Compose
  port, `http://localhost:8082`, for both `ApiBaseUrl` and
  `NotificationsBaseUrl`.

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
