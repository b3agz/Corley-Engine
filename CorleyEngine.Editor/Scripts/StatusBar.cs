using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

/// <summary>
/// A class for drawing the status bar to the editor UI.
/// </summary>
public class StatusBar {

    // Public properties so any system can push messages to the bar. Probably should change this.
    public string LeftMessage { get; set; } = "Ready";
    public string RightMessage { get; set; } = "Corley Engine v0.1";

    public void Draw(System.Numerics.Vector4 colour, GameTime gameTime) {

        var viewport = ImGui.GetMainViewport();

        // Place the status bar at the bottom of the window.
        ImGui.SetNextWindowPos(new (viewport.WorkPos.X, viewport.WorkPos.Y + viewport.WorkSize.Y - CorleyEditor.Preferences.StatusBarHeight));
        ImGui.SetNextWindowSize(new (viewport.WorkSize.X, CorleyEditor.Preferences.StatusBarHeight));

        // Make sure the status bar can't be moved or resized.
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoDecoration |
                                 ImGuiWindowFlags.NoDocking |
                                 ImGuiWindowFlags.NoSavedSettings |
                                 ImGuiWindowFlags.NoFocusOnAppearing |
                                 ImGuiWindowFlags.NoNav |
                                 ImGuiWindowFlags.NoMove;

        // Override certain general UI styling.
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(15, 5));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, colour);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
        ImGui.PushFont(CorleyEditor.StatusBarFont);


        if (ImGui.Begin("##MainStatusBar", flags)) {

            ImGui.Text(LeftMessage);

            RightMessage = $"FPS: {1000f / gameTime.ElapsedGameTime.TotalMilliseconds:0} | Corley Engine"; // TODO: Add version number from engine data somewhere.
            System.Numerics.Vector2 textSize = ImGui.CalcTextSize(RightMessage);
            ImGui.SameLine(ImGui.GetWindowWidth() - textSize.X - 10);
            ImGui.Text(RightMessage);

        }

        ImGui.End();

        ImGui.PopStyleColor();
        ImGui.PopFont();
        ImGui.PopStyleVar(3);
    }
}
