using DevExpress.ExpressApp.Blazor.Components.Models;

namespace SWMS.Components.Blazor.Module.JsonForms;

public class JsonFormsViewModel : ComponentModelBase
{
    public JsonFormsViewModel(string value = "", string jsonSchema = "", string uiSchema = "", bool readOnly = false)
    {
        Value = value;
        JsonSchema = jsonSchema;
        UiSchema = uiSchema;
        ReadOnly = readOnly;
    }

    public string Value
    {
        get => GetPropertyValue<string>();
        set => SetPropertyValue(value);
    }

    public bool ReadOnly
    {
        get => GetPropertyValue<bool>();
        set => SetPropertyValue(value);
    }

    public string JsonSchema
    {
        get => GetPropertyValue<string>();
        set => SetPropertyValue(value);
    }

    public string UiSchema
    {
        get => GetPropertyValue<string>();
        set => SetPropertyValue(value);
    }

    public void OnJsonDataChanged(string jsonData)
    {
        SetPropertyValue(jsonData, true, nameof(Value));
        ValueChanged?.Invoke(this, jsonData);
    }
    public event EventHandler<string> ValueChanged;
}