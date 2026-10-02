# Development

[← README](../README.md) · [Project map](PROJECT.md)

## Open and play

1. Install **Unity 6000.3.25f1** through Unity Hub. The authoritative version is in [`ProjectVersion.txt`](../ProjectSettings/ProjectVersion.txt). Add **Web Build Support** if you want a browser build.
2. Clone the project:

   ```sh
   git clone https://github.com/IvanPavlov7007/FERNWEH.git
   ```

3. In Unity Hub, add the cloned `FERNWEH` folder and open it. Allow the initial asset import and package restore to finish. Git must be available to Unity because some packages use Git URLs.
4. Open **Window → Asset Management → Addressables → Groups**. For a fresh checkout, use **Play Mode Script → Use Asset Database (fastest)**. This local preference lets the Editor load localization assets without prebuilt bundles. **Use Existing Build** requires a local Addressables content build; generated bundles are not tracked.
5. Open [`Assets/Sailboat/Scenes/Transition/Main Menu.unity`](../Assets/Sailboat/Scenes/Transition/Main%20Menu.unity), press **Play**, and start the game from the menu.

The game uses URP, the Input System, Cinemachine, and Unity Localization with Addressables. Exact package versions live in [`manifest.json`](../Packages/manifest.json) and [`packages-lock.json`](../Packages/packages-lock.json). Restore those files as committed before troubleshooting package or compile errors.

## Build

The published demo runs in the browser. To build that target:

1. Open **File → Build Profiles**, choose the **Web** platform, and switch to it. Install its module through Unity Hub if it is missing.
2. Use the committed global scene list from [`EditorBuildSettings.asset`](../ProjectSettings/EditorBuildSettings.asset). Keep **Main Menu** first and include **Transition General**, **Level 0–4**, and **Ending**. A profile-specific scene override must include these too.
3. In **Addressables → Groups**, run **Build → New Build → Default Build Script** for the active target. This generates the localization content needed by the player. Rebuild it when its content or target changes.
4. Choose **Build And Run** and write the output to `Builds/Web/` (already ignored by Git). Use the served browser build for the playtest.

The build hook [`BuildIncrementor.cs`](../Assets/CommonScripts/Editor/BuildIncrementor.cs) increments [`Assets/Resources/Build.asset`](../Assets/Resources/Build.asset) and updates the player version before a build. Review those resulting changes before committing.

Unity references: [scene lists](https://docs.unity3d.com/6000.0/Documentation/Manual/build-profile-scene-list.html) and [Addressables content builds](https://docs.unity.cn/Packages/com.unity.addressables%402.2/manual/builds-full-build.html).

## Quick playtest

After changing gameplay, scenes, or localization:

- Start from the main menu and check language selection and readable text.
- Follow the tutorials; test sail aiming, raising/lowering, paddling, and boat steering.
- Open the journal, resume, restart a level, and return to the menu.
- Check transitions through the story levels, fisherman pickup/drop-off, and the ending.
- Repeat the relevant flow in a Web build and check the Console / browser console for errors.

## Repository habits

- Keep Unity `.meta` files with their assets. Use Unity's Project window when moving or renaming assets.
- Keep scene names and resource paths in sync with their callers; see the [project map](PROJECT.md).
- Leave generated folders such as `Library/`, `Temp/`, and `Builds/` out of Git.
- Raw `Promo/` and `Recordings/` folders are local, ignored sources. Only the small README exports live in [`docs/media/`](media/README.md).
- Update these Markdown files alongside changes to setup, controls, or scene flow. No documentation generator or separate site is required.
