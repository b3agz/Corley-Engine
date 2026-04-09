using ImGuiNET;
using System.Numerics;
using Microsoft.Xna.Framework; // Needed for GameTime

namespace CorleyEngine.Editor;

public class MainWorkspace {

    public void Draw(GameTime gameTime, SceneViewWindow sceneView, GameViewWindow gameView, ConsoleWindow console) {

        var viewport = ImGui.GetMainViewport();

        // 1. Calculate the bounds of the central area
        float topOffset = CorleyEditor.Preferences.TitleBarHeight + CorleyEditor.Preferences.MenuBarHeight;
        float bottomOffset = CorleyEditor.Preferences.StatusBarHeight;
        float leftOffset = CorleyEditor.Preferences.HierarchyWidth;
        float rightOffset = CorleyEditor.Preferences.InspectorWidth; // Inspector width

        float availableWidth = viewport.WorkSize.X - leftOffset - rightOffset;
        float availableHeight = viewport.WorkSize.Y - topOffset - bottomOffset;

        // 2. Define the Split (75% Top, 25% Bottom)
        float stageHeight = (float)System.Math.Floor(availableHeight * 0.75f);
        float bottomPanelHeight = availableHeight - stageHeight;

        ImGuiWindowFlags flags = ImGuiWindowFlags.NoCollapse |
                                 ImGuiWindowFlags.NoResize |
                                 ImGuiWindowFlags.NoMove |
                                 ImGuiWindowFlags.NoTitleBar |
                                 ImGuiWindowFlags.NoBringToFrontOnFocus;

        // Strip the border for a flush fit
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f);

        // --- TOP WINDOW: THE STAGE (Scene / Game) ---
        ImGui.SetNextWindowPos(new (leftOffset, topOffset));
        ImGui.SetNextWindowSize(new (availableWidth, stageHeight));

        if (ImGui.Begin("MainStageWindow", flags)) {
            if (ImGui.BeginTabBar("MainStageTabs")) {

                if (ImGui.BeginTabItem("Scene View")) {
                    sceneView.DrawCustom(gameTime);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Game View")) {
                    gameView.DrawCustom(gameTime);
                    ImGui.EndTabItem();
                }

                // Future spot for your visual Node Editor tab!

                ImGui.EndTabBar();
            }
        }
        ImGui.End();

        // --- BOTTOM WINDOW: THE CONSOLE / ASSETS ---
        ImGui.SetNextWindowPos(new (leftOffset, topOffset + stageHeight));
        ImGui.SetNextWindowSize(new (availableWidth, bottomPanelHeight));

        if (ImGui.Begin("BottomPanelWindow", flags)) {
            if (ImGui.BeginTabBar("BottomPanelTabs")) {

                if (ImGui.BeginTabItem("Console")) {
                    console.DrawCustom(gameTime);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Assets")) {
                    ImGui.TextDisabled("Asset browser goes here...");
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }
        ImGui.End();

        ImGui.PopStyleVar();
    }
}