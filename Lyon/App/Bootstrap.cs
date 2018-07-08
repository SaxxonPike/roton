using System;
using System.IO;
using Lyon.App.Dialogs;
using Roton.Core;
using Roton.Emulation.Data.Impl;
using Roton.Interface.Windows;

namespace Lyon.App
{
    public class Bootstrap : IBootstrap
    {
        private readonly ILauncher _launcher;
        private readonly IFileSystemFactory _fileSystemFactory;

        public Bootstrap(ILauncher launcher, IFileSystemFactory fileSystemFactory)
        {
            _launcher = launcher;
            _fileSystemFactory = fileSystemFactory;
        }
        
        public void Boot(string[] args)
        {
            string fileName = null;
            
            if (args.Length == 0)
            {
                var openWorldDialog = new OpenWorldDialog();
                if (openWorldDialog.ShowDialog() == FileDialogResult.Ok)
                    fileName = openWorldDialog.FileName;
            }

            if (fileName == null)
                fileName = "TOWN.ZZT";

            var fileSystem = _fileSystemFactory.Create(
                Path.GetDirectoryName(fileName),
                Path.GetFileNameWithoutExtension(fileName));
            
            if (fileName.EndsWith(".zzt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(ContextEngine.Zzt, fileSystem);                

            if (fileName.EndsWith(".szt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(ContextEngine.SuperZzt, fileSystem);                
        }
    }
}