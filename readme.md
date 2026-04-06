# Corley Engine
A custom C# game engine built on top of the [MonoGame](https://monogame.net/) framework. The engine is intended to be a speciialist "point 'n' click adventure" engine, it aims to provide a modern, code-clean alternative to classic toolsets like [Adventure Game Studio](https://www.adventuregamestudio.co.uk/).

Named after the motorcycle company in the classic LucasArts adventure [Full Throttle](https://en.wikipedia.org/wiki/Full_Throttle_(1995_video_game))), the engine is currently in active development alongside my BSc in Games Programming. It serves as both a passion project for the genre and a practical sandbox for exploring advanced engine architecture, tool development, and data-driven design.

## Technical Architecture
Corley Engine is built top-to-bottom in C# and is structured to strictly separate runtime execution from the editor environment.

- **Entity-Component-System (ECS)**: The engine utilises a highly decoupled ECS architecture inspired by modern game engines like [Unity](https://unity.com/) and [Godot](https://godotengine.org/), allowing for modular game logic and clean memory management.

- **Data-Driven Scene Loading**: The engine relies on a custom System.Text.Json serialization pipeline. Scenes and entities are not hardcoded; they are stored as JSON data, parsed at runtime via reflection, and hot-loaded into active memory.

**Asset Management**: Corley Engine uses a centralised asset pipeline that handles texture hot-swapping and memory pooling, keeping the rendering loop fast and safe.

## The Roadmap
The ultimate goal for Corley Engine is to develop a complete, standalone editor toolkit alongside the runtime environment.

The planned architecture utilises a "wrapper" system:

- **The Runtime**: A precompiled, lightweight executable that contains zero hardcoded game logic.

- **The Toolkit**: A dedicated UI application used to create the project files that the runtime executable will load in. The editor will also be created in this manner; as a content package for the runtime executable. This ensures the editor and game do not need separate development, since they use the exact same executable.

- **The Pipeline**: The toolkit packages the user-generated JSON data and assets, dropping them into the runtime's directory to produce a shippable game.

Currently, the core ECS and serialisation pipelines are functioning, and development is moving toward the graphical editor interface.

## License
This project is licensed under the [MIT License](LICENSE).