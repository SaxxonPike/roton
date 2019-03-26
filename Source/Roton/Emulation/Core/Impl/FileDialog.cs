using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public class FileDialog : IFileDialog
    {
        private readonly Lazy<IHud> _hud;
        private readonly Lazy<IFileSystem> _fileSystem;

        public FileDialog(Lazy<IHud> hud, Lazy<IFileSystem> fileSystem)
        {
            _hud = hud;
            _fileSystem = fileSystem;
        }

        private IHud Hud
        {
            [DebuggerStepThrough] get => _hud.Value;
        }

        private IFileSystem FileSystem
        {
            [DebuggerStepThrough] get => _fileSystem.Value;
        }

        public string Open(string title, string extension)
        {
            var path = string.Empty;
            while (true)
            {
                var files = FileSystem
                    .GetFileNames(path, extension)
                    .Select(f => f.Substring(0, f.Length - extension.Length - 1))
                    .OrderBy(f => f)
                    .Concat(new[] {"Exit"})
                    .ToArray();

                var result = Hud.ShowScroll(false, title, files);
                if (result.Cancelled)
                    return null;
                
                // If the user selects "Exit", which is always at the bottom of the list:
                if (result.Index >= result.Lines.Count - 1)
                    return null;

                return result.Lines[result.Index];
            }
        }
    }
}