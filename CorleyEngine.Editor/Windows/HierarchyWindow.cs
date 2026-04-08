using ImGuiNET;
using Microsoft.Xna.Framework;
using CorleyEngine.Core;
using System.Collections.Generic;

namespace CorleyEngine.Editor;

public class HierarchyWindow : EditorWindow {
    // A temporary reference to whatever the user clicks on.
    // Eventually, this should probably be moved to a global "Selection" manager
    // so the Inspector window knows what to draw.
    private object _selectedEntity;

    public HierarchyWindow() : base("Hierarchy") {
    }

    protected override void OnGui(GameTime gameTime) {
        var scene = SceneManager.ActiveScene;

        if (scene == null) {
            ImGui.TextDisabled("No active scene loaded.");
            return;
        }

        // Assuming your Scene has a list of root-level entities
        // If your scene is just a flat list of objects, loop through that instead.
        foreach (var entity in scene.GetEntities()) {
            DrawEntityNode(entity);
        }

        // Optional: Clicking on empty space in the window clears the selection
        if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left)) {
            // Only deselect if we aren't hovering over an actual item
            if (!ImGui.IsAnyItemHovered()) {
                _selectedEntity = null;
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
        if (entity == _selectedEntity) {
            flags |= ImGuiTreeNodeFlags.Selected;
        }

        // Draw the node
        bool isOpen = ImGui.TreeNodeEx(entity.Name, flags);

        // Check if the user just clicked the node we drew
        if (ImGui.IsItemClicked()) {
            _selectedEntity = entity;
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
