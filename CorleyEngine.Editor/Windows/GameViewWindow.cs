using ImGuiNET;
using Microsoft.Xna.Framework;
using CorleyEngine.Core;

namespace CorleyEngine.Editor;

/// <summary>
/// Creates the game view window.
/// </summary>
public class GameViewWindow : EditorWindow {

    private readonly CorleyGame _engineCore;
    private readonly ImGuiRenderer _imGuiRenderer;

    public GameViewWindow(CorleyGame engineCore, ImGuiRenderer imGuiRenderer) : base("Game View") {
        _engineCore = engineCore;
        _imGuiRenderer = imGuiRenderer;
    }

    protected override void OnGui(GameTime gameTime) {

        var size = ImGui.GetContentRegionAvail();
        if (size.X <= 0 || size.Y <= 0) return;

        _engineCore.BuildCanvas((int)size.X, (int)size.Y);

        var pos = ImGui.GetCursorScreenPos();
        Input.ViewportOffset = new Vector2(pos.X, pos.Y);
        Input.ViewportDisplaySize = new Vector2(size.X, size.Y);
        Input.ViewportScale = new Vector2(
            (float)GameView.DesignWidth / size.X,
            (float)GameView.DesignHeight / size.Y);

        if (_engineCore.GameCanvas != null) {
            ImGui.Image(_imGuiRenderer.BindTexture(_engineCore.GameCanvas), size);
        }
    }

    public void DrawCustom(GameTime gameTime) => OnGui(gameTime);

}