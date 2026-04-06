using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CorleyEngine.Components;

namespace CorleyEngine.IO;

/// <summary>
/// A class containing tools for converting <see cref="Component"/> to and from their base classes.
/// for the purpose of serialisation.
/// </summary>
/// <remarks>
/// To make scene management feasible, components all inherit from an abstract Component class. Since the scenes
/// only have access to the Component class and don't actually know that a give Component is a Camera or Transform
/// etc, JSON serialisation can't automatically serialise the individual components. This pulls the child class
/// forward so that the JSON serialisation grabs the child class data, rather than just the data exposed by the
/// abstract Component class.
/// </remarks>
public class ComponentConverter : JsonConverter<IComponent> {

    /// <summary>
    /// Writes the <paramref name="value"/> <see cref="Component"/> to JSON as its child type.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The object that implements inherits from Component.</param>
    /// <param name="options">An object that specifies serialisation options to use.</param>
    public override void Write(Utf8JsonWriter writer, IComponent value, JsonSerializerOptions options) {

        writer.WriteStartObject();

        // Get the actual class type.
        Type type = value.GetType();
        writer.WriteString("$type", type.Name);

        // Serialise the child class rather than the inherited class.
        using (JsonDocument doc = JsonSerializer.SerializeToDocument(value, type, options)) {
            foreach (var prop in doc.RootElement.EnumerateObject()) {
                prop.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reader">The reader .</param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"An object that specifies serialisation options to use.</param>
    /// <returns>An object of the required type.</returns>
    /// <exception cref="JsonException">Throws exception if the listed Type does not exist in CorleyEngine.Components.</exception>
    public override IComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

        using JsonDocument doc = JsonDocument.ParseValue(ref reader);

        // Get the Type tag from the JSON data.
        if (!doc.RootElement.TryGetProperty("$type", out JsonElement typeElement)) {
            throw new JsonException("Component JSON is missing a $type tag.");
        }

        string typeName = typeElement.GetString();

        // Get the type from the available CorleyEngine.Components classes.
        Type componentType = Type.GetType($"CorleyEngine.Components.{typeName}") ?? throw new JsonException($"Could not find a component class named {typeName}.");

        // Deserialise the JSON data into a new object of the required type and return it.
        return (IComponent)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), componentType, options);

    }
}