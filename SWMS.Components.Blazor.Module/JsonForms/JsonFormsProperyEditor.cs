using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Model;

namespace SWMS.Components.Blazor.Module.JsonForms;

public class JsonFormsProperyEditor : BlazorPropertyEditorBase
{
    private JsonFormsViewModel _componentModel;

    public JsonFormsProperyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) 
    { 
        _componentModel = new JsonFormsViewModel();
        _componentModel.JsonSchema = @"{
    ""type"": ""object"",
    ""properties"": {
        ""firstName"": { ""type"": ""string"" },
        ""lastName"": { ""type"": ""string"" },
        ""age"": { ""type"": ""integer"" },
        ""isEmployed"": { ""type"": ""boolean"" },
        ""birthDate"": { ""type"": ""string"", ""format"": ""date"" },
        ""address"": {
        ""type"": ""object"",
        ""properties"": {
            ""street"": { ""type"": ""string"" },
            ""city"": { ""type"": ""string"" },
            ""postalCode"": { ""type"": ""string"" }
        }
        }
    }
}";
    }

    protected override IComponentAdapter CreateComponentAdapter() => new JsonFormsAdapter(_componentModel);
}
