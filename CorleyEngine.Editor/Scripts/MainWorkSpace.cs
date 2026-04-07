using ImGuiNET;
using System.Numerics;

namespace CorleyEngine.Editor;

/// <summary>
/// Creates the main workspace where <see cref="EditorWindow"/>s live.
/// </summary>
public class MainWorkspace {

    // TODO: Centralise this variable somewhere... like EditorPreferences maybe.
    private const float STATUS_BAR_HEIGHT = 40f;

    public void Draw() {

        var viewport = ImGui.GetMainViewport();

        Vector2 workspaceSize = new(viewport.WorkSize.X, viewport.WorkSize.Y - STATUS_BAR_HEIGHT);

        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(workspaceSize);
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDocking;
        windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
        windowFlags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoBackground;

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));

        ImGui.Begin("MainDockSpaceWindow", windowFlags);
        ImGui.PopStyleVar();

        var io = ImGui.GetIO();
        if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0) {
            uint dockspaceId = ImGui.GetID("MainDockSpace");

            // Swap NoWindowMenuButton for AutoHideTabBar
            ImGuiDockNodeFlags dockFlags = ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.AutoHideTabBar;

            ImGui.DockSpace(dockspaceId, new Vector2(0.0f, 0.0f), dockFlags);
        }

        ImGui.End();
    }
}