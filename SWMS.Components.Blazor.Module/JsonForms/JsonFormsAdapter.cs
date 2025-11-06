using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Components;


namespace SWMS.Components.Blazor.Module.JsonForms;

internal class JsonFormsAdapter : ComponentAdapterBase
{
    public JsonFormsAdapter(JsonFormsViewModel componentModel)
    {
        ViewModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        ViewModel.ValueChanged += ComponentModel_ValueChanged;
    }
    public JsonFormsViewModel ViewModel { get; }

    public override IComponentModel ComponentModel => ViewModel;

    public override void SetAllowEdit(bool allowEdit)
    {
        ViewModel.ReadOnly = !allowEdit;
    }

    public override object GetValue()
    {
        return ViewModel.Value;
    }

    public override void SetValue(object value)
    {
        ViewModel.Value = (string)value;
    }

    protected override RenderFragment CreateComponent()
    {
        return ComponentModelObserver.Create(ViewModel, JsonFormsRenderer.Create(ViewModel));
    }

    private void ComponentModel_ValueChanged(object sender, EventArgs e) => RaiseValueChanged();
    public override void SetAllowNull(bool allowNull) { /* ...*/ }
    public override void SetDisplayFormat(string displayFormat) { /* ...*/ }
    public override void SetEditMask(string editMask) { /* ...*/ }
    public override void SetEditMaskType(EditMaskType editMaskType) { /* ...*/ }
    public override void SetErrorIcon(ImageInfo errorIcon) { /* ...*/ }
    public override void SetErrorMessage(string errorMessage) { /* ...*/ }
    public override void SetIsPassword(bool isPassword) { /* ...*/ }
    public override void SetMaxLength(int maxLength) { /* ...*/ }
    public override void SetNullText(string nullText) { /* ...*/ }
}
