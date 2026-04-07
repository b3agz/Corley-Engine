using ImGuiNET;
using System.Numerics;

namespace CorleyEngine.Editor;

/// <summary>
/// A class for drawing the status bar to the editor UI.
/// </summary>
public class StatusBar {

    // Public properties so any system can push messages to the bar. Probably should change this.
    public string LeftMessage { get; set; } = "Ready";
    public string RightMessage { get; set; } = "Corley Engine v0.1";

    private const float HEIGHT = 36f;

    public void Draw() {

        var viewport = ImGui.GetMainViewport();

        // Place the status bar at the bottom of the window.
        ImGui.SetNextWindowPos(new Vector2(viewport.WorkPos.X, viewport.WorkPos.Y + viewport.WorkSize.Y - HEIGHT));
        ImGui.SetNextWindowSize(new Vector2(viewport.WorkSize.X, HEIGHT));

        // Make sure the status bar can't be moved or resized.
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoDecoration |
                                 ImGuiWindowFlags.NoDocking |
                                 ImGuiWindowFlags.NoSavedSettings |
                                 ImGuiWindowFlags.NoFocusOnAppearing |
                                 ImGuiWindowFlags.NoNav |
                                 ImGuiWindowFlags.NoMove;

        // Override certain general UI styling.
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(10, 6));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(0, 0));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.12f, 0.12f, 0.12f, 1.0f));

        if (ImGui.Begin("##MainStatusBar", flags)) {
            // Left-aligned status text
            ImGui.Text(LeftMessage);

            // Right-aligned engine info
            if (!string.IsNullOrEmpty(RightMessage)) {
                var textSize = ImGui.CalcTextSize(RightMessage);
                // Move the cursor to the far right, minus the text width and some padding
                ImGui.SameLine(ImGui.GetWindowWidth() - textSize.X - 10);
                ImGui.TextDisabled(RightMessage);
            }
        }
        ImGui.End();

        // Put the style back to general UI styling so the rest of the UI doesn't inherit this one.
        ImGui.PopStyleColor();
        ImGui.PopStyleVar(3);
    }
}
