using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public class ImGuiStyleWindow : EditorWindow {

    public ImGuiStyleWindow() : base("Style Window") { }

    protected override void OnGui(GameTime gameTime) {

        ImGui.Begin("Style Editor");
        ImGui.ShowStyleEditor();
        ImGui.End();

    }
}