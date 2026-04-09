using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public class ImGuiStyleWindow : EditorWindow {

    public ImGuiStyleWindow() : base("Style Window") { }

    protected override void OnGui(GameTime gameTime) {

        ImGui.ShowStyleEditor();

    }
}