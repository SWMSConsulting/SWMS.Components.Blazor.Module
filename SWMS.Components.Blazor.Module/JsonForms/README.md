# Json Forms

Json Forms is a framework for building form-based web applications using JSON schema and UI schema. It allows developers to create dynamic forms that can be easily customized and extended.
This package is a Blazor/DevExpress based implementation of Json Forms (https://jsonforms.io/).

## How to use
1. Register the JsonFormsDefinition class in your module
```csharp
AdditionalExportedTypes.Add(typeof(JsonFormsDefinition));
```

2. Add JsonFormsDefinition to database context 
```csharp
public DbSet<JsonFormsTemplate> JsonFormsDefinition { get; set; }
```

3. Register the JsonFormsPropertyEditor for the desired property. Make sure to set the FieldSize attribute to int.MaxValue to allow for large JSON content.
```csharp
[FieldSize(int.MaxValue)]
[ModelDefault("PropertyEditorType", "SWMS.Components.Blazor.Module.FileViewer.JsonFOrmsPropertyEditor")]
public string ExampleProperty { get; set; }
```