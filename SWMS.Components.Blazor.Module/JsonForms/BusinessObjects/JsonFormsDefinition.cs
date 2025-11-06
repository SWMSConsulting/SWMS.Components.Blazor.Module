using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace SWMS.Components.Blazor.Module.JsonForms.BusinessObjects;


[DefaultClassOptions]
[NavigationItem("System Settings")]
public class JsonFormsDefinition : BaseObject
{
    [FieldSize(255)]
    public virtual string RelatedProperty { get; set; }  
    

    [FieldSize(int.MaxValue)]
    public virtual string JsonSchema { get; set; }

    
    [FieldSize(int.MaxValue)]
    public virtual string JsonFormsUiSchema { get; set; }
}