# Blog.Blazor

The frontend for the blog — Blazor WebAssembly, MudBlazor. Talks to
`Blog.API` over HTTP and a SignalR connection; it doesn't run anything on
its own.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- `Blog.API` running locally first — see `../blog/README.md`. By
  default this app points at `https://localhost:7163`, the API's local
  `https` launch profile.

## Getting started

```bash
cd Blog.Blazor
dotnet run
```

Opens at `https://localhost:7279` (the `https` launch profile). The API
base URL comes from `wwwroot/appsettings.Development.json` — if your
local API runs on a different port, update `ApiBaseUrl` there.

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
