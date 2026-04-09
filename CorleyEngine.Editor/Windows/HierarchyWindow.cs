using ImGuiNET;
using Microsoft.Xna.Framework;
using CorleyEngine.Core;
using System.Collections.Generic;

namespace CorleyEngine.Editor;

public class HierarchyWindow : EditorWindow {

    /// <summary>
    /// A reference to the inspector so it can tell the inspector what Entity is
    /// currently highlighted.
    /// </summary>
    private InspectorWindow _inspector;

    public HierarchyWindow(InspectorWindow inspector) : base("Hierarchy") {

        // Make this window fixed and unmodifiable.
        WindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
        _inspector = inspector;
    }

    protected override void BeforeBegin() {

        ImGuiViewportPtr viewport = ImGui.GetMainViewport();

        float topOffset = CorleyEditor.Preferences.TitleBarHeight + CorleyEditor.Preferences.MenuBarHeight;
        float bottomOffset = CorleyEditor.Preferences.StatusBarHeight;

        // Pin to the left side, just underneath the menu bar
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, topOffset));

        // Stretch all the way down to the status bar
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(CorleyEditor.Preferences.HierarchyWidth, viewport.WorkSize.Y - topOffset - bottomOffset));

    }

protected override void OnGui(GameTime gameTime) {

        Scene scene = SceneManager.ActiveScene;

        if (scene == null) {
            ImGui.TextDisabled("No active scene loaded.");
            return;
        }

        // Apply hierarchy-specific spacing style to keep things spread out a bit.
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(0, CorleyEditor.Preferences.HiearchyVerticalSpacing));
        ImGui.PushStyleVar(ImGuiStyleVar.IndentSpacing, CorleyEditor.Preferences.HiearchyIndentation);

        ImGuiTreeNodeFlags sceneFlags = ImGuiTreeNodeFlags.DefaultOpen |
                                        ImGuiTreeNodeFlags.OpenOnArrow |
                                        ImGuiTreeNodeFlags.SpanAvailWidth;

        // Draw the scene node (the parent of everything).
        bool isSceneOpen = ImGui.TreeNodeEx(SceneManager.ActiveScene.Data.Name, sceneFlags);

        // If the scene node is expanded, draw the entities inside it
        if (isSceneOpen) {
            foreach (var entity in scene.GetEntities()) {
                DrawEntityNode(entity);
            }
            ImGui.TreePop(); // Must pop the scene node!
        }

        // Pop the spacing styles
        ImGui.PopStyleVar(2);

        // Clicking on empty space in the window clears the selection
        if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left)) {

            // Only deselect if we aren't hovering over an actual item
            if (!ImGui.IsAnyItemHovered()) {
                _inspector.TargetEntity = null;
            }
        }
    }

    private void DrawEntityNode(Entity entity) {

        // Set up the behavior and look of the tree node
        ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.SpanAvailWidth;

        // If this entity has no children, draw it as a leaf (no dropdown arrow)
        if (entity.Children == null || entity.Children.Count == 0)
        {
            flags |= ImGuiTreeNodeFlags.Leaf;
        }

        // Highlight the node if it's the currently selected one
        if (entity == _inspector.TargetEntity) {
            flags |= ImGuiTreeNodeFlags.Selected;
        }

        // Draw the node
        bool isOpen = ImGui.TreeNodeEx(entity.Name, flags);

        // Check if the user just clicked the node we drew
        if (ImGui.IsItemClicked()) {
            _inspector.TargetEntity = entity;
        }

        // If the node is expanded, recursively draw its children
        if (isOpen)
        {
            foreach (var child in entity.Children)
            {
                DrawEntityNode(child);
            }
            ImGui.TreePop(); // Must pop if TreeNodeEx returns true!
        }
    }
}
