using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public abstract class EditorWindow(string title) {

    /// <summary>
    /// The title shown at the top of the window.
    /// </summary>
    public string Title { get; protected set; } = title;

    /// <summary>
    /// Whether the window is visible in the editor.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Whether or not we should draw a border around this window.
    /// </summary>
    protected bool HasBorder { get; set; } = false;

    /// <summary>
    /// Allow subclasses to set their own flags.
    /// </summary>
    protected ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.None;

    /// <summary>
    ///
    /// </summary>
    /// <param name="gameTime"></param>
    public void UpdateAndDraw(GameTime gameTime) {

        if (!IsVisible) return;

        // ref bool allows the 'X' button to work
        if (ImGui.Begin(Title, ref _visible)) {
            OnGui(gameTime);
        }
        ImGui.End();

        IsVisible = _visible;
    }

    private bool _visible = true;

    public void Draw(GameTime gameTime) {

        if (!IsVisible) return;

        BeforeBegin();

        if (!HasBorder)
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);


        bool isOpen = ImGui.Begin(Title, WindowFlags);

        if (isOpen) {
            OnGui(gameTime);
        }

        ImGui.End();

        if (!HasBorder)
            ImGui.PopStyleVar();

    }

    /// <summary>
    /// Overridden by subclasses, allows them to set their own details.
    /// </summary>
    protected virtual void BeforeBegin() { }

    /// <summary>
    /// Called from the GUI draw method.
    /// </summary>
    /// <param name="gameTime">MonoGame's <see cref="GameTime"/>.</param>
    protected abstract void OnGui(GameTime gameTime);

}