# KZ BBCode Generator v3.0

A modern, extensible Windows application for generating BBCode and Markdown formatting for forums and chat platforms.

```
╔═══════════════════════════════════════════════════════════════╗
║  _  __ ______   ____  ____   ____          _                  ║
║ | |/ /|___  /  | __ )| __ ) / ___|___   __| | ___             ║
║ | ' /    / /   |  _ \|  _ \| |   / _ \ / _` |/ _ \            ║
║ | . \   / /_   | |_) | |_) | |__| (_) | (_| |  __/            ║
║ |_|\_\ /____|  |____/|____/ \____\___/ \__,_|\___|            ║
╚═══════════════════════════════════════════════════════════════╝
```

## Features

### 10 Platform Support

| Platform | Type | Format |
|----------|------|--------|
| phpBB | Legacy Forum | BBCode |
| vBulletin | Legacy Forum | BBCode |
| MyBB | Legacy Forum | BBCode |
| Classic BBCode | Legacy Forum | BBCode |
| SMF | Legacy Forum | BBCode |
| IPB/Invision | Legacy Forum | BBCode |
| XenForo | Modern Forum | BBCode |
| Discourse | Modern Forum | Markdown |
| Discord | Chat | Markdown |
| Slack | Chat | mrkdwn |

### 20+ Tag Types

- **Text Formatting**: Bold, Italic, Underline, Strikethrough, Color, Size, Font
- **Links & Media**: URL, Email, Mention, Image, YouTube
- **Structure**: Quote, Code, Spoiler, List, Table, Align
- **Special**: Headers, Horizontal Rule

### 16 Visual Themes

1. Dark Mode (default)
2. Dracula (purple accents)
3. Tokyo Night (rich blues)
4. Catppuccin Mocha (soothing pastels)
5. Gruvbox Dark (retro groove)
6. Matrix (green terminal)
7. Neon (magenta/cyan)
8. Cyberpunk (red/gold)
9. Ocean (blue tones)
10. Synthwave (80s pink/purple)
11. Light Mode
12. Solarized Dark
13. Solarized Light
14. Monokai
15. Nord
16. High Contrast (accessibility)

### Additional Features

- **Auto-copy** to clipboard on generation
- **Session history** of generated codes
- **Keyboard shortcuts** (Ctrl+B, Ctrl+I, Ctrl+K, etc.)
- **Batch operations** for multiple URLs/images
- **Settings persistence** (theme, platform, window position)
- **Desktop shortcut** creation with custom icon
- **Single-file deployment** (~60-80MB self-contained exe)

## Screenshots

```
┌─────────────────────────────────────────────────────────┐
│ [Platform ▼] [Theme ▼]              [?] [Settings] [X] │
├─────────────────────────────────────────────────────────┤
│ Toolbar: [B][I][U][S][Color][Size][Link][Img][Table]... │
├───────────────────────────┬─────────────────────────────┤
│                           │                             │
│   INPUT TEXT AREA         │   GENERATED CODE            │
│                           │   (auto-copied)             │
│                           │                             │
├───────────────────────────┴─────────────────────────────┤
│ Status: Ready | Platform: phpBB | Theme: Dark           │
└─────────────────────────────────────────────────────────┘
```

## Installation

### Download Release

Download the latest `KZBBCode.exe` from the [Releases](../../releases) page.

No installation required - just run the executable.

### Build from Source

Requirements:
- .NET 9 SDK
- Windows 10/11 (64-bit)

```powershell
# Clone repository
git clone https://github.com/yourusername/KZ-BBCode-Generator.git
cd KZ-BBCode-Generator

# Build
dotnet build

# Run
dotnet run --project src/KZBBCode

# Publish single-file exe
dotnet publish src/KZBBCode -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -o publish
```

## Usage

1. **Select Platform**: Choose your target forum/chat from the dropdown
2. **Enter Text**: Type or paste content in the input area
3. **Apply Formatting**: Click toolbar buttons or use keyboard shortcuts
4. **Copy Result**: Code is auto-copied (or click Copy button)
5. **Paste**: Ctrl+V in your forum/chat

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+B | Bold |
| Ctrl+I | Italic |
| Ctrl+U | Underline |
| Ctrl+K | Insert URL |
| F1 | Open Help |

### Batch Operations

Enter multiple URLs (one per line) and use:
- **URLs** button for batch link generation
- **Imgs** button for batch image embedding

## Project Structure

```
KZ-BBCode-Generator/
├── src/
│   └── KZBBCode/
│       ├── Models/          # Platform, AppSettings
│       ├── ViewModels/      # MainViewModel (MVVM)
│       ├── Views/           # MainForm, Dialogs
│       ├── Generators/      # Platform-specific generators
│       ├── Services/        # Theme, Settings
│       ├── Helpers/         # Validation
│       └── Resources/       # Icon
├── tests/
│   └── KZBBCode.Tests/
├── .github/workflows/
└── README.md
```

## Configuration

Settings are stored in `%AppData%/KZBBCode/settings.json`:

```json
{
  "themeName": "Dark Mode",
  "platformName": "phpBB",
  "autoCopyToClipboard": true,
  "showPreview": true,
  "rememberWindowPosition": true
}
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

MIT License - see [LICENSE](LICENSE) file.

## Credits

- **Developer**: KrazyZone LAZYFROG
- **Technology**: C# .NET 9, Windows Forms, CommunityToolkit.Mvvm
- **Icon**: Custom design

---

*Making forum formatting beautiful - one BBCode at a time.*

🐸 **LAZYFROG** - Keeping the retro web alive
