# Corley Engine
A custom C# game engine built on top of the [Raylib-cs](https://github.com/ChrisDill/Raylib-cs) wrapper for [Raylib](https://www.raylib.com/). The engine is intended to be a specialist "point 'n' click adventure" engine, aiming to provide a niche, modern alternative to classic toolsets like [Adventure Game Studio](https://www.adventuregamestudio.co.uk/).

Named after the motorcycle company in the classic LucasArts adventure [Full Throttle](https://en.wikipedia.org/wiki/Full_Throttle_(1995_video_game)), the engine is currently in active development. It serves as both a passion project for the genre and a practical sandbox for exploring advanced engine architecture, tool development, and data-driven design.

## Technical Architecture
Corley Engine is built top-to-bottom in C# and is structured to strictly separate runtime execution from the editor environment.

- **Object-Oriented Rendering**: The engine utilises an object-oriented approach for renderable objects (`RenderableObject`, `SpriteObject`, `TextObject`), ensuring modular and extensible rendering logic.

- **Data-Driven Scene Loading**: The engine supports structured scene management and data-driven entity definition.

- **Asset Management**: Corley Engine uses a centralised asset management system that handles texture and font loading, keeping the rendering loop fast and safe.

## The Roadmap
The ultimate goal for Corley Engine is to develop a complete, standalone editor toolkit alongside the runtime environment.

- **The Runtime**: A precompiled, lightweight executable that contains the core engine logic for loading scenes and entities.

- **The Toolkit**: A planned dedicated UI application used to create the project files that the runtime executable will load.

Currently, the core rendering systems, input handling, and scene management are in active development.

## Third-Party Assets
- [**Pixelzone**](https://ggbot.itch.io/pixelzone-font): Licensed under [Creative Commons 0](https://creativecommons.org/publicdomain/zero/1.0/).

## License
This project is licensed under the [MIT License](LICENSE).