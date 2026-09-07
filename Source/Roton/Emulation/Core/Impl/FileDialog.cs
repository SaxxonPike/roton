using System.Linq;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class FileDialog(
    IFileSystem fileSystem,
    IScrollContent scrollContent,
    IScroll scroll,
    IState state)
    : IFileDialog
{
    public string? Open(string title, string extension, IFileTitles? fileTitles)
    {
        var path = string.Empty;

        var fileNames = fileSystem
            .GetFileNames(path, extension)
            .Select(f => f.Substring(0, f.Length - extension.Length - 1))
            .OrderBy(f => f)
            .Select(f => (Name: f, Special: fileTitles?.GetTitle(f)))
            .ToList();

        var titleMap = fileNames
            .Select((e, i) => (Element: e, Index: i))
            .ToDictionary(x => x.Index, x => x.Element.Name);

        var files = fileNames
            .Select(x => x.Special ?? x.Name)
            .Concat(["Exit"])
            .ToArray();

        scrollContent.AddLines(files);
        
        var lineCount = scrollContent.LineCount;

        var result = scroll.ShowMessage(title, false, 0);
        if (state.CancelScroll)
            return null;

        // If the user selects "Exit", which is always at the bottom of the list:
        if (result.Index >= lineCount - 1)
            return null;

        return titleMap[result.Index];
    }
}