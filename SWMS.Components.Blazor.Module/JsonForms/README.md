# Json Forms

Json Forms is a framework for building form-based web applications using JSON schema and UI schema. It allows developers to create dynamic forms that can be easily customized and extended.
This package is a Blazor/DevExpress based implementation of Json Forms (https://jsonforms.io/).

## How to use
1. Add JsonFormsDefinition to database context 

public DbSet<JsonFormsTemplate> JsonFormsDefinition { get; set; }

2. Editor referenzieren
    [ModelDefault("PropertyEditorType", "SWMS.Components.Blazor.Module.FileViewer.JsonFOrmsPropertyEditor")]
oder
    [EditorAlias("JsonFOrmsPropertyEditor")]