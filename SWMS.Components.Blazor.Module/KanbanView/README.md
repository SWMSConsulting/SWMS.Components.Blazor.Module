# Kanban View Component

The Kanban View component is a versatile and interactive UI element designed to help users visualize and manage tasks or items in a Kanban-style board. This component is ideal for project management, task tracking, and workflow organization.

## How to Use
1. Add the module to your Blazor application (git submodule)
2. Implement the required interfaces (IKanbanItem, IKanbanColumn) in your business objects.
3. Register the KanbanItemListEditor (in BlazorModule.cs):
```csharp
protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory)
    {
        editorDescriptorsFactory.RegisterListEditor(typeof(IKanbanItem), typeof(KanbanItemListEditor), false);
        base.RegisterEditorDescriptors(editorDescriptorsFactory);
    }
``` 
If you want the KanbanItemListEditor as default list editor for IKanbanItem, set the third parameter to true. Otherwise you need to set per view in the Model Editor.
4. (optional) Add a List View to your Model View and set the List Editor to KanbanItemListEditor.

