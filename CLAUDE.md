# CLAUDE.md - Development Guide for KZ BBCode Generator

## Project Overview

KZ BBCode Generator is a Windows Forms application using MVVM pattern for generating BBCode and Markdown formatting across multiple forum and chat platforms.

## Build Commands

```bash
# Development build
cd src/KZBBCode
dotnet build

# Run application
dotnet run --project src/KZBBCode

# Release build (single exe)
dotnet publish src/KZBBCode -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -o publish

# Run tests
dotnet test
```

## Architecture

### MVVM Pattern
- **Models**: `Platform.cs`, `AppSettings.cs` - Data structures
- **ViewModels**: `MainViewModel.cs` - Application logic with CommunityToolkit.Mvvm
- **Views**: `MainForm.cs`, Dialogs - UI components

### Generator System
- `IBBCodeGen` interface defines all tag methods
- `BBCodeBase` provides default BBCode implementations
- Platform-specific generators override only differing methods
- `GeneratorFactory` provides generator instances by platform

### Services
- `ThemeService` - 12 theme definitions with recursive control styling
- `SettingsService` - JSON persistence to %AppData%

## Key Files

| File | Purpose |
|------|---------|
| `MainViewModel.cs` | All application logic, commands |
| `MainForm.cs` | UI layout, event handling |
| `BBCodeBase.cs` | Default BBCode implementations |
| `DiscordGen.cs` / `SlackGen.cs` | Markdown generators |
| `ThemeService.cs` | Theme definitions |
| `GeneratorFactory.cs` | Generator instantiation |

## Adding a New Platform

1. Create generator class in `Generators/`:
```csharp
public class NewPlatformGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.NewPlatform;

    // Override methods that differ from standard BBCode
    public override string Bold(string text) => $"**{text}**";
}
```

2. Add to `PlatformType` enum in `Models/Platform.cs`
3. Add to `PlatformInfo.All` list
4. Register in `GeneratorFactory._generators`

## Adding a New Theme

Add to `ThemeService.cs`:
```csharp
public static Theme NewTheme => new(
    "New Theme",
    Color.FromArgb(r, g, b),  // Background
    Color.FromArgb(r, g, b),  // Foreground
    // ... other colors
);
```

Add to `AllThemes` array.

## Adding a New Tag Type

1. Add method to `IBBCodeGen` interface
2. Implement default in `BBCodeBase`
3. Override in platform generators as needed
4. Add command in `MainViewModel`
5. Add toolbar button in `MainForm`

## Testing

```bash
dotnet test tests/KZBBCode.Tests
```

Key test areas:
- Generator output correctness per platform
- YouTube ID extraction
- Color/URL validation
- Settings persistence

## Code Style

- Use `partial` methods from CommunityToolkit.Mvvm for property change notifications
- Use source generators for regex patterns (`[GeneratedRegex]`)
- Keep generators simple - override only what differs
- Use records for immutable data (Theme, PlatformInfo)

## Common Issues

### Icon not showing
- Ensure `Desktop.BBCode.KZ.icon.ico` is in `Resources/`
- Check `.csproj` includes it as EmbeddedResource and Content

### Clipboard access fails
- Wrap in try-catch, clipboard can be locked by other apps

### Theme not applying
- `ThemeService.ApplyTheme` must be called recursively on all controls
- Check control type switch in `ApplyThemeToControl`

## Dependencies

- `CommunityToolkit.Mvvm` - MVVM helpers, auto-generated commands
- `System.Text.Json` - Settings serialization
- .NET 9 Windows Forms - UI framework
