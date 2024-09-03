using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace SWMS.Components.Blazor.Module.FileViewer;

// https://supportcenter.devexpress.com/ticket/details/t1017487/xaf-blazor-filedata-viewer

[PropertyEditor(typeof(string), false)]
public class FileDataPropertyEditor : BlazorPropertyEditorBase
{
    public FileDataPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

    protected override IComponentAdapter CreateComponentAdapter() => new FileDataAdapter(new FileDataModel());
}