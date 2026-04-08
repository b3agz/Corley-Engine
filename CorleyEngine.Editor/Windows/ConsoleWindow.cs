using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.Editor;

public class ConsoleWindow : EditorWindow {
    private bool _autoScroll = true;
    private bool _scrollToBottomNextFrame = false;

    public ConsoleWindow() : base("Console") {

        // Subscribe to new log events.
        EngineLogger.OnLogAdded += () => {
            if (_autoScroll) _scrollToBottomNextFrame = true;
        };

        // Just something to show it works.
        EngineLogger.Info("Console initialized successfully.");
    }

    protected override void OnGui(GameTime gameTime) {

        if (ImGui.Button("Clear")) {
            EngineLogger.Clear();
        }

        ImGui.SameLine();
        ImGui.Checkbox("Auto-Scroll", ref _autoScroll);

        ImGui.Separator();

        ImGui.BeginChild("LogScrollRegion", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar);

        foreach (LogEntry log in EngineLogger.Logs) {

            System.Numerics.Vector4 color = log.Type switch {
                LogType.Info => new System.Numerics.Vector4(0.8f, 0.8f, 0.8f, 1.0f),
                LogType.Warning => new System.Numerics.Vector4(1.0f, 0.8f, 0.2f, 1.0f),
                LogType.Error => new System.Numerics.Vector4(1.0f, 0.3f, 0.3f, 1.0f),
                _ => new System.Numerics.Vector4(1, 1, 1, 1)
            };

            ImGui.PushStyleColor(ImGuiCol.Text, color);

            // If a log contains a '%' symbol, standard ImGui.Text will try to parse it as a variable and crash the C++ backend.
            // Found that out the hard way.
            ImGui.TextUnformatted($"[{log.Timestamp}] {log.Message}");

            ImGui.PopStyleColor();
        }

        // If a new log was added and auto-scroll is on, snap to the bottom
        if (_scrollToBottomNextFrame) {
            ImGui.SetScrollHereY(1.0f);
            _scrollToBottomNextFrame = false;
        }

        ImGui.EndChild();
    }
}