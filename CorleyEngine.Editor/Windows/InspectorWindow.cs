using ImGuiNET;
using System;
using System.Reflection;
using System.Linq;
using System.Numerics;
using CorleyEngine.Core;
using CorleyEngine.Components;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public class InspectorWindow : EditorWindow {


    // The object we are currently looking at
    public Entity TargetEntity { get; set; }

    public InspectorWindow() : base("Inspector") {
        WindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
    }

    protected override void BeforeBegin() {
        var viewport = ImGui.GetMainViewport();

        float topOffset = CorleyEditor.Preferences.TitleBarHeight + CorleyEditor.Preferences.MenuBarHeight;
        float bottomOffset = CorleyEditor.Preferences.StatusBarHeight;

        // Pin to the right side
        float xPos = viewport.WorkSize.X - CorleyEditor.Preferences.InspectorWidth;
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(xPos, topOffset));

        ImGui.SetNextWindowSize(new System.Numerics.Vector2(CorleyEditor.Preferences.InspectorWidth, viewport.WorkSize.Y - topOffset - bottomOffset));

    }

    protected override void OnGui(GameTime gameTime) {

        if (TargetEntity == null) {
            ImGui.TextColored(new (0.5f, 0.5f, 0.5f, 1.0f), "No Entity Selected");
            return;
        }

        ImGui.TextDisabled($"ID: {TargetEntity.Id}");
        ImGui.Separator();

        bool isActive = TargetEntity.IsActive;
        if (ImGui.Checkbox("##EntityActive", ref isActive)) TargetEntity.IsActive = isActive;

        ImGui.SameLine();
        ImGui.Text(TargetEntity.Name);
        ImGui.Separator();

        // Loop through all attached components
        foreach (IComponent component in TargetEntity.Components) {
            DrawComponentNode(component);
        }

        ImGui.Spacing();
        if (ImGui.Button("Add Component", new(ImGui.GetContentRegionAvail().X, 30))) {
            // TODO: Component Search Menu
        }
    }

    // Change 'Component' to 'IComponent' here
    private void DrawComponentNode(IComponent component) {

        // Returns the actual Type of the component even though it only has reference to an interface.
        Type type = component.GetType();

        ImGui.PushID(component.GetHashCode());

        if (ImGui.CollapsingHeader(type.Name, ImGuiTreeNodeFlags.DefaultOpen)) {
            DrawFieldsViaReflection(component, type);
        }

        ImGui.PopID();
    }

    private void DrawFieldsViaReflection(object target, Type type) {

        // Raw fields
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields) {
            DrawWidget(target, field.Name, field.FieldType,
                () => field.GetValue(target),
                (val) => field.SetValue(target, val));
        }

        // We only want properties we can actually read AND write
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.CanRead && p.CanWrite);

        foreach (var prop in properties) {
            DrawWidget(target, prop.Name, prop.PropertyType,
                () => prop.GetValue(target),
                (val) => prop.SetValue(target, val));
        }
    }

    // A reusable helper so we don't write the ImGui logic twice
    private void DrawWidget(object target, string name, Type dataType, Func<object> getValue, Action<object> setValue) {

        var value = getValue();

        if (dataType == typeof(float)) {
            float val = (float)value;
            if (ImGui.DragFloat(name, ref val, 0.1f)) setValue(val);
        }
        else if (dataType == typeof(int)) {
            int val = (int)value;
            if (ImGui.DragInt(name, ref val)) setValue(val);
        }
        else if (dataType == typeof(bool)) {
            bool val = (bool)value;
            if (ImGui.Checkbox(name, ref val)) setValue(val);
        }
        else if (dataType == typeof(Microsoft.Xna.Framework.Vector2)) {
            var mgVec = (Microsoft.Xna.Framework.Vector2)value;
            var sysVec = new System.Numerics.Vector2(mgVec.X, mgVec.Y);

            if (ImGui.DragFloat2(name, ref sysVec, 0.1f)) {
                setValue(new Microsoft.Xna.Framework.Vector2(sysVec.X, sysVec.Y));
            }
        }
        else if (dataType == typeof(Microsoft.Xna.Framework.Vector3)) {
            var mgVec = (Microsoft.Xna.Framework.Vector3)value;
            var sysVec = new System.Numerics.Vector3(mgVec.X, mgVec.Y, mgVec.Z);

            if (ImGui.DragFloat3(name, ref sysVec, 0.1f)) {
                setValue(new Microsoft.Xna.Framework.Vector3(sysVec.X, sysVec.Y, sysVec.Z));
            }
        }
        // TODO: Add strings, Colors, Enums, etc...
    }
}
