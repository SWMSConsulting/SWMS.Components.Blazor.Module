using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Components;

namespace SWMS.Components.Blazor.Module.FileViewer;

public class FileDataAdapter : ComponentAdapterBase
{
    public FileDataAdapter(FileDataModel componentModel)
    {
        FileDataModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        FileDataModel.ValueChanged += ComponentModel_ValueChanged;
    }
    public FileDataModel FileDataModel { get; }

    public override IComponentModel ComponentModel => FileDataModel;

    public override void SetAllowEdit(bool allowEdit)
    {
        FileDataModel.ReadOnly = !allowEdit;
    }

    public override object GetValue()
    {
        return FileDataModel.Value;
    }

    public override void SetValue(object value)
    {
        FileDataModel.Value = (string)value;
    }

    protected override RenderFragment CreateComponent()
    {
        return ComponentModelObserver.Create(FileDataModel, FileDataRenderer.Create(FileDataModel));
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
