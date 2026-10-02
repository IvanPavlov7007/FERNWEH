# README media

[← Main README](../../README.md)

Small, committed exports keep the landing page self-contained. Raw promo footage stays in the ignored `Promo/` folder and is not required to view the docs or open the game project.

| Export | Local source | Selection |
| --- | --- | --- |
| `fernweh.png` | `Promo/ITCH_Banner.png` | Original banner, copied unchanged |
| `sailing.gif` | `Promo/FERNWEH_Gameplay1080.mp4` | 0.5–4.5 seconds |
| `coast.gif` | `Promo/FERNWEH_Gameplay1080.mp4` | 12–16 seconds |

The GIFs use 480 px width, 10 fps, and a 96-color palette. They loop as short excerpts; the cuts are not seamless. Captions and alt text in the README describe each clip without requiring animation.

To regenerate, restore the two local source files above, install FFmpeg, and run from the repository root:

```powershell
./docs/media/generate.ps1
# Or supply an FFmpeg executable:
./docs/media/generate.ps1 -FFmpeg 'C:/path/to/ffmpeg.exe'
```

Adjust the clip selections in [`generate.ps1`](generate.ps1), then update this table. Keep exports short and review the resulting file sizes and playback before committing. No media generation is needed for normal documentation edits.
