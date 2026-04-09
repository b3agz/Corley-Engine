using CorleyEngine.Core;
using ImGuiNET;

namespace CorleyEngine.Editor;

/// <summary>
/// Draws the top menu bar of the editor (file menu, edit menu, etc)
/// </summary>
/// <param name="editor">The main editor Game class</param>
public class MenuBar(CorleyEditor editor) {

    private CorleyEditor _game = editor;

    public void Draw() {

        ImGuiWindowFlags menuFlags = ImGuiWindowFlags.NoCollapse |
                                     ImGuiWindowFlags.NoResize |
                                     ImGuiWindowFlags.NoMove |
                                     ImGuiWindowFlags.NoTitleBar |
                                     ImGuiWindowFlags.NoScrollbar |
                                     ImGuiWindowFlags.MenuBar;

        // Pin it directly underneath the Title Bar
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, CorleyEditor.Preferences.TitleBarHeight));
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(_game.GraphicsDevice.Viewport.Width, CorleyEditor.Preferences.TitleBarHeight));

        // Set up padding and borders.
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(20f, 20f));
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(20f, 20f));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);

        if (ImGui.Begin("CustomMenuBarWindow", menuFlags)) {
            if (ImGui.BeginMenuBar()) {
                if (ImGui.BeginMenu("File")) {
                    if (ImGui.MenuItem("Save Scene", "Ctrl+S")) { SceneManager.SaveScene(); }
                    if (ImGui.MenuItem("Save Project", "Ctrl+S")) { ProjectManager.SaveProject(); }
                    if (ImGui.MenuItem("Exit Editor")) {
                        // TODO: Check save project warning if changes have been made.
                        _game.Exit();
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("View")) {
                    if (ImGui.MenuItem("Something")) { /* Open Inspector */ }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Tools")) {
                    if (ImGui.MenuItem("Style Window")) { _game.ToggleImGuiStyleWindow(); }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
            ImGui.End();
        }

        ImGui.PopStyleVar(4);
    }

}