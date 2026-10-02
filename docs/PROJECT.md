# Project map

[← README](../README.md) · [Development](DEVELOPMENT.md)

## Where things live

| Location | Contents |
| --- | --- |
| [`Assets/Sailboat/Scripts/`](../Assets/Sailboat/Scripts/) | Game logic: sailing, wind, story levels, UI, quests, and progression |
| [`Assets/Sailboat/Scenes/Story/`](../Assets/Sailboat/Scenes/Story/) | The five playable story levels |
| [`Assets/Sailboat/Scenes/Transition/`](../Assets/Sailboat/Scenes/Transition/) | Main menu, shared transition, ending, and splash scene |
| [`Assets/Sailboat/Resources/`](../Assets/Sailboat/Resources/) | Prefabs, art, input mappings, localization, and ScriptableObject content |
| [`Assets/CommonScripts/`](../Assets/CommonScripts/) | Shared audio, interaction, UI, utility, and editor code |
| [`Assets/AddressableAssetsData/`](../Assets/AddressableAssetsData/) | Addressables configuration, including localization groups |
| [`Krita/`](../Krita/) | Art source files, design sketches, and notes |
| [`Packages/`](../Packages/) / [`ProjectSettings/`](../ProjectSettings/) | Package versions and Unity configuration |

## Story flow

**Main Menu** starts a five-level sequence, **Level 0** through **Level 4**. **Transition General** runs before each level and once more before **Ending**.

[`MainMenu`](../Assets/Sailboat/Scripts/Levels/MainMenu.cs) starts transition index 0. [`TransitionScene`](../Assets/Sailboat/Scripts/Levels/TransitionScene.cs) selects the corresponding text and loads `Level {index}`, or `Ending` at index 5. [`LevelScene`](../Assets/Sailboat/Scripts/Levels/LevelScene.cs) advances the index when a level finishes.

Levels 0–2 use destination triggers. Level 3 handles fisherman rescue, and Level 4 handles passenger drop-offs. Their [`Level0`–`Level4` scripts](../Assets/Sailboat/Scripts/Levels/) are the first place to look when changing the demo's objectives. Scene names and indices are used in code; changing only the scene assets will break that flow.

## Sailing and wind

[`PlayerInputController`](../Assets/Sailboat/Scripts/PlayerInputController.cs) receives actions from [`Sailboat Inputs.inputactions`](../Assets/Sailboat/Resources/Input%20Mappings/Sailboat%20Inputs.inputactions). The `Sailing`, `Menu`, `Tutorial`, and `Transition` maps separate gameplay input from UI and story screens.

The story's square-rig raft and boat prefabs live in [`Prefabs/Player/`](../Assets/Sailboat/Resources/Prefabs/Player/). Their [`RaftWithSailController`](../Assets/Sailboat/Scripts/Controllers/RaftWithSailController.cs) and [`SquareRigBoatController`](../Assets/Sailboat/Scripts/Controllers/SquareRigBoatController.cs) turn wind and sail orientation into `Rigidbody2D` forces. [`VehicleController`](../Assets/Sailboat/Scripts/Controllers/VehicleController.cs) supplies shared paddling and sail-state behavior.

[`WindManager`](../Assets/Sailboat/Scripts/Wind/WindManager.cs) returns either constant wind or wind sampled from collider-defined areas. When area mode is enabled, it selects overlapping areas with the highest priority and averages their wind vectors. The implementations in [`Wind/WindAreas/`](../Assets/Sailboat/Scripts/Wind/WindAreas/) cover constant, changing, periodic, and spatially varying wind.

## UI and content

[`G`](../Assets/Sailboat/Scripts/G.cs) holds shared scene references. [`GameManager`](../Assets/Sailboat/Scripts/GameManager.cs) coordinates pause/resume, action-map switching, and NPC interactions. These scripts expect the scene's configured managers and UI references.

[`GameBootstrapper`](../Assets/Sailboat/Scripts/GameBootstrapper.cs) initializes progression, item definitions, inventory, and quests. [`GameEvents`](../Assets/Sailboat/Scripts/GameEvents.cs) connects collection and interaction events to systems such as [`QuestManager`](../Assets/Sailboat/Scripts/Quest%20System/QuestManager.cs).

Editable definitions are under [`Resources/Scriptable Objects/`](../Assets/Sailboat/Resources/Scriptable%20Objects/): dialogs, items, quests, trading offers, tutorials, and progression states. Several loaders use literal `Resources.LoadAll` paths, so preserve folder names and IDs when editing content. Quest progress is reset on the loaded ScriptableObjects; inspect asset changes after playtesting.

Text tables and the four locale assets live in [`Resources/Localizations/`](../Assets/Sailboat/Resources/Localizations/). [`LocalizationManager`](../Assets/Sailboat/Scripts/LocalizationManager.cs) connects the main-menu dropdown to Unity Localization. Update the tables when adding player-facing text.

## Experiments and older work

[`Scenes/Legacy/`](../Assets/Sailboat/Scenes/Legacy/) contains earlier prototypes and backups. [`New controls test.unity`](../Assets/Sailboat/Scenes/New%20controls%20test.unity) and [`Controllers/Realistic/`](../Assets/Sailboat/Scripts/Controllers/Realistic/) explore separate boat and sail components, including tightening/loosening input. They are outside the committed story build's scene list.

The repository also includes quest, trading, and vehicle-progression systems beyond the demo's direct level scripts. Check a scene's actual component and prefab references before assuming a system participates in the published story flow.
