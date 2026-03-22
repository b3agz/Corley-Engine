using Microsoft.Xna.Framework;

namespace CorleyEngine.Core;

/// <summary>
/// Provides an interface to get time information from Corley Engine.
/// </summary>
public static class Time {

    /// <summary>
    /// The time elapsed in seconds since the last frame affected by <see cref="TimeScale"/>.
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    /// The raw time in seconds that has elapsed since the last frame.
    /// </summary>
    public static float UnscaledDeltaTime { get; private set; }

    /// <summary>
    /// The total time in seconds that the game has been running.
    /// </summary>
    public static float TotalTime { get; private set; }

    /// <summary>
    /// Affects how quickly "time" passes in the game. 1.0f = realtime.
    /// </summary>
    public static float TimeScale { get; set; } = 1.0f;

    /// <summary>
    /// Updates the all time variables. Should be called once per frame.
    /// </summary>
    /// <param name="gameTime">The total <see cref="GameTime"/> that has elapsed so far.</param>
    public static void Update(GameTime gameTime) {

        UnscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        DeltaTime = UnscaledDeltaTime * TimeScale;

        TotalTime += DeltaTime;

    }
}
