# FileTypeGuard

Check if a file is really what its name says. Backend only, C# console app.

Reads the first bytes, called magic bytes, finds the real type, compares it to the extension, and reports MATCH, MISMATCH, UNKNOWN, or ERROR.

## Features

- Magic bytes for PDF, PNG, JPEG, ZIP, EXE, GIF
- Reads only first 8 bytes with FileStream, no full file load
- Extension check with JPG and JPEG treated as same
- Handles missing file, empty file, and small file without crash
- Risk levels LOW, MEDIUM, HIGH
- No printing inside helpers, only `Main` prints

## Project Structure

- `Program.cs` - Main app, all 4 methods
- `FileTypeGuard.csproj` - Project file, .NET 8.0

## How to Run

```bash
dotnet run -- "real.pdf"
dotnet run -- "fake.pdf"
dotnet run -- "empty.txt"
dotnet run -- "missing.pdf"
dotnet run
