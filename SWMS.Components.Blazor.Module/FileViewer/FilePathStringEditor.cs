using DevExpress.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace SWMS.Components.Blazor.Module.FileViewer;

[PropertyEditor(typeof(string), nameof(FilePathStringEditor), false)]
public class FilePathStringEditor : BlazorPropertyEditorBase
{
    public FilePathStringEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
    protected override IComponentModel CreateComponentModel()
    {
        var basePath = Environment.GetEnvironmentVariable("FILES_BASE_PATH") ?? "";

        List<string> files = Directory.GetFiles(basePath, "*", SearchOption.AllDirectories).ToList();

        DxComboBoxModel<string, string> componentModel = new DxComboBoxModel<string, string>();

        componentModel.Data = files;
        componentModel.AllowUserInput = true;
        componentModel.FilteringMode = DataGridFilteringMode.Contains;
        componentModel.ClearButtonDisplayMode = AllowNull ? DataEditorClearButtonDisplayMode.Auto : DataEditorClearButtonDisplayMode.Never;
        componentModel.NullText = NullText;
        return componentModel;
    }
}