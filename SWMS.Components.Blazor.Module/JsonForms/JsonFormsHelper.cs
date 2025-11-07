using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SWMS.Components.Blazor.Module.JsonForms;

public static class JsonFormsHelper
{
    public static JsonObject GenerateUiSchema(string jsonSchema)
    {
        using var doc = JsonDocument.Parse(jsonSchema);
        var rootElement = doc.RootElement;

        var uiSchema = new JsonObject
        {
            ["type"] = "VerticalLayout",
            ["elements"] = GenerateElements(rootElement, "#/properties")
        };

        return uiSchema;
    }

    private static JsonArray GenerateElements(JsonElement schema, string parentScope)
    {
        var elements = new JsonArray();

        if (!schema.TryGetProperty("properties", out var props))
            return elements;

        var propertyDefs = props.EnumerateObject().ToList();

        JsonObject? currentRow = null;

        foreach (var prop in propertyDefs)
        {
            var propName = prop.Name;
            var propValue = prop.Value;
            var scope = $"{parentScope}/{propName}";

            // If property is an object => separate group
            if (propValue.TryGetProperty("type", out var typeProp) && typeProp.GetString() == "object")
            {
                // Flush any active row before adding group
                if (currentRow != null && ((JsonArray)currentRow["elements"]!).Count > 0)
                {
                    elements.Add(currentRow);
                    currentRow = null;
                }

                // Add the group (nested object)
                var group = new JsonObject
                {
                    ["type"] = "Group",
                    ["label"] = propName,
                    ["elements"] = GenerateElements(propValue, $"{scope}/properties")
                };
                elements.Add(group);
            }
            else
            {
                // Start new row if needed
                if (currentRow == null)
                {
                    currentRow = new JsonObject
                    {
                        ["type"] = "HorizontalLayout",
                        ["elements"] = new JsonArray()
                    };
                }

                // Add control to current row
                ((JsonArray)currentRow["elements"]!).Add(new JsonObject
                {
                    ["type"] = "Control",
                    ["scope"] = scope
                });

                // After 2 controls, close the row
                if (((JsonArray)currentRow["elements"]!).Count == 2)
                {
                    elements.Add(currentRow);
                    currentRow = null;
                }
            }
        }

        // Flush any remaining open row
        if (currentRow != null && ((JsonArray)currentRow["elements"]!).Count > 0)
            elements.Add(currentRow);

        return elements;
    }


    public static JsonObject GetPropertySchema(JsonObject jsonSchema, string scope)
    {
        var parts = scope.TrimStart('#').Split("/properties/");
        JsonObject current = jsonSchema;

        foreach (var part in parts)
        {
            if (current["properties"] != null && current["properties"].AsObject().ContainsKey(part))
            {
                current = current["properties"][part].AsObject();
            }
        }
        return current;
    }
}
