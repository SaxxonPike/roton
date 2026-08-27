using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class FileDialog(
    IHud hud, 
    IFileSystem fileSystem,
    IScrollContent scrollContent)
    : IFileDialog
{
    private IHud Hud
    {
        [DebuggerStepThrough] get => hud;
    }

    private IFileSystem FileSystem
    {
        [DebuggerStepThrough] get => fileSystem;
    }

    public string? Open(string title, string extension)
    {
        var line = (stackalloc char[256]);
        var path = string.Empty;

        while (true)
        {
            var files = FileSystem
                .GetFileNames(path, extension)
                .Select(f => f.Substring(0, f.Length - extension.Length - 1))
                .OrderBy(f => f)
                .Concat(["Exit"])
                .ToArray();

            var result = Hud.ShowScroll(false, title, files);
            if (result.Cancelled)
                return null;
                
            // If the user selects "Exit", which is always at the bottom of the list:
            if (result.Index >= scrollContent.LineCount - 1)
                return null;

            return scrollContent.GetLine(result.Index, line).ToString();
        }
    }
}