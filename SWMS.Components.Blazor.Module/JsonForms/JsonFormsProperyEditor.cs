using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Model;

namespace SWMS.Components.Blazor.Module.JsonForms;

public class JsonFormsProperyEditor : BlazorPropertyEditorBase
{
    public JsonFormsProperyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

    protected override IComponentAdapter CreateComponentAdapter() => new JsonFormsAdapter(new JsonFormsViewModel());
}
