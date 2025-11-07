using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using SWMS.Components.Blazor.Module.JsonForms.BusinessObjects;

namespace SWMS.Components.Blazor.Module.JsonForms;

public class JsonFormsProperyEditor : BlazorPropertyEditorBase, IComplexViewItem
{
    public IObjectSpace ObjectSpace { get; set; }
    public XafApplication Application { get; set; }

    private string relatedProperty = "";
    public JsonFormsProperyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) 
    {
        relatedProperty = $"{objectType.FullName}.{model.PropertyName}";
    }

    public void Setup(IObjectSpace objectSpace, XafApplication application)
    {
        ObjectSpace = objectSpace;
        Application = application;
    }

    protected override IComponentAdapter CreateComponentAdapter() 
    {
        var jsonFormsDefinition = ObjectSpace.GetObjects<JsonFormsDefinition>(
            CriteriaOperator.Parse("RelatedProperty = ?", relatedProperty)).FirstOrDefault();

        if(jsonFormsDefinition == null)
        {
            jsonFormsDefinition = ObjectSpace.CreateObject<JsonFormsDefinition>();
            jsonFormsDefinition.RelatedProperty = relatedProperty;
            jsonFormsDefinition.JsonSchema = "";
            jsonFormsDefinition.JsonFormsUiSchema = "";
            ObjectSpace.CommitChanges();
        }

        if (string.IsNullOrEmpty(jsonFormsDefinition.JsonFormsUiSchema))
        {
            jsonFormsDefinition.JsonFormsUiSchema = JsonFormsHelper.GenerateUiSchema(jsonFormsDefinition.JsonSchema).ToJsonString();
            ObjectSpace.CommitChanges();
        }

        var componentModel = new JsonFormsViewModel();
        componentModel.JsonSchema = jsonFormsDefinition?.JsonSchema ?? "";
        componentModel.UiSchema = jsonFormsDefinition?.JsonFormsUiSchema ?? "";

        return new JsonFormsAdapter(componentModel);
    }
}
