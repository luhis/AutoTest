# AutoTest

## Running Tests

This project uses **xUnit v3** with `OutputType=Exe`. Do NOT use `dotnet test` — it fails with a testhost error.

Run tests directly as executables:

```bash
# Unit tests
dotnet run --project Test/AutoTest.Unit.Test/AutoTest.Unit.Test.csproj

# Integration tests
dotnet run --project Test/AutoTest.Integration.Test/AutoTest.Integration.Test.csproj
```

## Build

After making changes, always run a release build to ensure analyzers execute:

```bash
dotnet build AutoTest.sln -c Release
```
