using DevExpress.ExpressApp.Blazor.Components.Models;

namespace SWMS.Components.Blazor.Module.JsonForms;

public class JsonFormsViewModel : ComponentModelBase
{
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

    public void SetValueFromUI(string value)
    {
        SetPropertyValue(value, notify: false, nameof(Value));
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler ValueChanged;
}