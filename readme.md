# Corley Engine
A custom C# game engine built on top of the [MonoGame](https://monogame.net/) framework. The engine is intended to be a speciialist "point 'n' click adventure" engine, along the lines of [Adventure Game Studio](https://www.adventuregamestudio.co.uk/).

The purpose of this project is part educational (I am studying for a BSc in Game Programming), and part for the experience.

Not to mention I just like point 'n' click adventures.

## Architecture
The engine uses an Entity-Component-System (ECS) similar to engines like [Unity](https://unity.com/) and [Godot](https://godotengine.org/), and will endeavour to stick to clean code and good practice as much as I am capable of (I am a student, after all!).

The engine is built in C#, with the intention to be C# top-to-bottom.

## The Plan
I hope to make Corley Engine (named for the motorcycle company in the amazing point 'n' click game, [Full Throttle](https://en.wikipedia.org/wiki/Full_Throttle_(1995_video_game))) into a complete engine and editor, with a full user interace and tools for building PnC games.

The current plan is to utilise a wrapper system, where the game engine is precompiled and "empty", and loads the information dynamically from a contents folder. The toolkit would create that contents folder and package it up with the precompiled engine.

It is very early days, however.

## License
This project is licensed under the [MIT License](LICENSE).