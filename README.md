# Project Portfolio #0

![CI](https://badgen.net/badge/License/MIT/blue)
![CI](https://badgen.net/badge/Unity/6000.0.33f1/blue)
![CI](https://badgen.net/badge/Status/Pre-Production/yellow)
[![CI](https://badgen.net/badge/LinkedIn/Konstanty%20Karczymarzyk/green)](https://linkedin.com/in/konstanty-karczymarzyk-a58625239)

An RTS/Tower Defense game project focused on maximum unit modularity, allowing for easy expansion of unit components and game features during the production phase.
This project served as an initial exploration of game mechanics, with particular emphasis on demonstrating a scalable and modular unit architecture.

It's still in a pre-production state, with potential problems that were not detected during small-scale testing.
Areas for potential future optimization and improvements include pathfinding algorithms, rendering performance and reference handling.


## Usage

The project requires Unity Editor version `6000.0.33f1` to be installed.

In the Scenes/ directory, you will find [`Gameplay_Scene`](`Assets/Project_Portfolio/Scenes/Gameplay_Scene.unity`) and
[`Main_Menu_Scene`](`Assets/Project_Portfolio/Scenes/Main_Menu_Scene.unity`). The project can be run from either of these scenes.


## Project Structure

*Some of those directories are split into two subdirectories: `objects` (for strictly related assets) and `scripts`, to help maintain order within a given feature.*

- [`Global`](`Assets/Project_Portfolio/Global`): Contains core game assets accessible from any part of the game.
  By default, its assets should be implemented through game functions.
- [`Gameplay`](`Assets/Project_Portfolio/Gameplay`)
  - [`Core`](`Assets/Project_Portfolio/Gameplay/Core`): Contains the main managers and behavior for the entire gameplay.
    It contains interfaces definitions which implementations are located in the [`Features/`](`Assets/Project_Portfolio/Features`) directory. 
  - [`UI`](`Assets/Project_Portfolio/Gameplay/UI`):
  - [`Units`](`Assets/Project_Portfolio/Gameplay/Units`): Contains base classes. Uses `Features` interfaces to implement unit's behaviour 
  - [`Features`](`Assets/Project_Portfolio/Gameplay/Features`): Contains game interface implementations.
- `GFX`: Graphcs assets
  - `Materials`: Contains shaders, mater materials and material instances
  - `Models` 
- `Scenes`


## Registry

The Registry system serves as a foundational element for managing and accessing various game systems and data.
It operates on a base `Registry` class, specific registry implementations provide specialized management for different scopes within the project.

- [`GameRegistry`](`Assets/Project_Portfolio/Global/Scripts/Management/GameRegistry.cs`): Singleton class provides a global scope
  for managing systems and data that are accessible throughout the entire game session. Its property ensures a single point
  of access. It is initialized early in the application lifecycle via `GameInstaller`.
- [`SceneRegistry`](`Assets/Project_Portfolio/Gameplay/Manager/SceneRegistry.cs`): Provides scope of a specific Unity scene.
  It is responsible for managing scene-specific objects and data.<br>When `SceneRegistry::Get()` is called and
  there is no instance for the required scene, a new instance of `SceneRegistry` and `SceneRegistryContainer` is created
  and the registry is added to `GameRegistry`.
  - [`SceneRegistryContainer`](`Assets/Project_Portfolio/Gameplay/Manager/SceneRegistryContainer.cs`):
    This class extends `MonoBehaviour` and its purpose is managing `SceneRegistry` lifecycle

