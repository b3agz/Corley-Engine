using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Core;

namespace CorleyEngine.Editor;

/// <summary>
/// Class for drawing a custom title header bar in place of the OS default.
/// </summary>
public class TitleBar {

    private readonly CorleyEditor _game;
    private readonly GraphicsDeviceManager _graphics;

    private bool _isDragging = false;
    private int _dragOffsetX = 0;
    private int _dragOffsetY = 0;

    private IntPtr _iconPtr;
    private IntPtr _minIconPtr;
    private IntPtr _maxIconPtr;
    private IntPtr _closeIconPtr;

    private bool _isMaximized = false;

    public TitleBar(CorleyEditor game, GraphicsDeviceManager graphics, ImGuiRenderer imGuiRenderer) {
        _game = game;
        _graphics = graphics;

        Texture2D appIcon = _game.Content.Load<Texture2D>("Icons/corleyapp");
        Texture2D minIcon = _game.Content.Load<Texture2D>("Icons/minimise");
        Texture2D maxIcon = _game.Content.Load<Texture2D>("Icons/maximise");
        Texture2D closeIcon = _game.Content.Load<Texture2D>("Icons/close");

        _iconPtr = imGuiRenderer.BindTexture(appIcon);
        _minIconPtr = imGuiRenderer.BindTexture(minIcon);
        _maxIconPtr = imGuiRenderer.BindTexture(maxIcon);
        _closeIconPtr = imGuiRenderer.BindTexture(closeIcon);
    }

    public void Draw(System.Numerics.Vector4 colour) {

        ImGuiWindowFlags flags = ImGuiWindowFlags.NoCollapse |
                                 ImGuiWindowFlags.NoResize |
                                 ImGuiWindowFlags.NoMove |
                                 ImGuiWindowFlags.NoTitleBar |
                                 ImGuiWindowFlags.NoScrollbar;

        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0));
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(_game.GraphicsDevice.Viewport.Width, CorleyEditor.Preferences.TitleBarHeight));

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(10, 5));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);

        ImGui.PushStyleColor(ImGuiCol.WindowBg, colour);

        if (ImGui.Begin("CustomTitleBar", flags)) {

            float buttonSize = CorleyEditor.Preferences.TitleBarHeight * 0.75f;
            float halfButtonSize = buttonSize / 2f;

            ImGui.Image(_iconPtr, new System.Numerics.Vector2(buttonSize, buttonSize));
            ImGui.SameLine(0, 15);
            ImGui.Text($"Corley Editor - {ProjectManager.CurrentProject.ProjectName}");

            // Drag logic
            bool isHoveringTitleBar = ImGui.IsWindowHovered();
            bool isMouseDown = ImGui.IsMouseDown(ImGuiMouseButton.Left);
            bool isMouseClicked = ImGui.IsMouseClicked(ImGuiMouseButton.Left);

            // Start drag
            if (isHoveringTitleBar && isMouseClicked) {
                _isDragging = true;
                SDL.SDL_GetGlobalMouseState(out int globalMouseX, out int globalMouseY);
                _dragOffsetX = globalMouseX - _game.Window.Position.X;
                _dragOffsetY = globalMouseY - _game.Window.Position.Y;
            }

            // Continue and/or end the drag.
            if (_isDragging) {
                if (isMouseDown) {
                    // As long as the button is held, update the position, regardless of hover state
                    SDL.SDL_GetGlobalMouseState(out int globalMouseX, out int globalMouseY);
                    _game.Window.Position = new Point(globalMouseX - _dragOffsetX, globalMouseY - _dragOffsetY);
                }
                else {
                    // Button was released, drop the window
                    _isDragging = false;
                }
            }

            // Window controls (min, max, etc.)
            System.Numerics.Vector2 btnSize = new (buttonSize, buttonSize);

            ImGui.SameLine(ImGui.GetWindowWidth() - (buttonSize * 3) - buttonSize);

            float totalButtonWidth = (buttonSize * 3) + halfButtonSize;
            float rightPadding = halfButtonSize;

            // Move cursor to the right side of the title bar.
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - totalButtonWidth - rightPadding);

            // Make sure the buttons aren't padded and there's no grey box instead of transparency.
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0, 0, 0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(0, 0));

            // Minimise Button
            if (ImGui.ImageButton("btn_min", _minIconPtr, btnSize)) {
                SDL.SDL_MinimizeWindow(_game.Window.Handle);
            }

            ImGui.SameLine(0, 5);

            // Maximise Button
            if (ImGui.ImageButton("btn_max", _maxIconPtr, btnSize)) {
                _isMaximized = !_isMaximized;
                if (_isMaximized) {
                    SDL.SDL_MaximizeWindow(_game.Window.Handle);
                }
                else {
                    SDL.SDL_RestoreWindow(_game.Window.Handle);
                }
            }

            ImGui.SameLine(0, 5);

            // Close Button
            if (ImGui.ImageButton("btn_close", _closeIconPtr, btnSize)) {
                _game.Exit();
            }

            ImGui.PopStyleVar(4);
            ImGui.PopStyleColor(2);
            ImGui.End();

        }
    }
}