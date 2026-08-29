using System;
using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;

namespace CorleyEngine.UI;

    /// <summary>
    /// A UI object that displays hints based on the object under the mouse cursor.
    /// </summary>
    public class HintObject : TextObject {

        /// <summary>
        /// Initializes a new instance of the <see cref="HintObject"/> class.
        /// </summary>
        public HintObject(FontSize fontSize = FontSize.Small) : base(fontSize, true) {
            Name = "HintObject";
        }

        /// <summary>
        /// Called each frame to update logic.
        /// </summary>
        /// <param name="deltaTime">The amount of time in seconds that has elapsed since the last frame.</param>
        public override void OnUpdate(float deltaTime) {
            base.OnUpdate(deltaTime);

            // Get world position to check for objects
            Vector2 worldMousePosition = SceneManager.ActiveScene.Camera.ScreenToWorld(Input.GetVirtualMousePosition());

            if (RenderManager.GetObjectAtPoint(worldMousePosition, out RenderableObject? foundObject) && foundObject != null && !string.IsNullOrEmpty(foundObject.HintText)) {

                Vector2 objectSize = foundObject.GetSize();

                // Position above the object, centered horizontally
                // Convert object position to screen space to account for camera
                Vector2 screenObjectPosition = SceneManager.ActiveScene.Camera.WorldToScreen(foundObject.Position);

                Vector2 targetPosition = new Vector2(
                    screenObjectPosition.X + (objectSize.X / 2),
                    screenObjectPosition.Y - 2
                );

                // Calculate text size for centering and clamping
                Vector2 textSize = Raylib.MeasureTextEx(_font, foundObject.HintText, (int)FontSize, 1f);

                // Adjust target to center the hint text
                targetPosition.X -= (textSize.X / 2);
                targetPosition.Y -= textSize.Y;

                // Clamp to screen bounds
                float clampedX = Math.Clamp(targetPosition.X, 0, EngineConstants.RESOLUTION_WIDTH - textSize.X);
                float clampedY = Math.Clamp(targetPosition.Y, 0, EngineConstants.RESOLUTION_HEIGHT - textSize.Y);

                Position = new Vector2(clampedX, clampedY);
                Text = foundObject.HintText;
            }
            else {
                Text = "";
            }
        }
    }