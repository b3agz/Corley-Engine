using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public class StatusWindow : EditorWindow {

    public StatusWindow() : base("Engine Status") { }

    protected override void OnGui(GameTime gameTime) {

        ImGui.Text("Corley Editor is online.");
        ImGui.Separator();
        ImGui.Text($"FPS: {1000f / gameTime.ElapsedGameTime.TotalMilliseconds:0.0}");

    }
}