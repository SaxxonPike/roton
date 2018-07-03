using Lyon.App.Dialogs;
using Roton.Interface.Windows;

namespace Lyon.App
{
    public class Bootstrap : IBootstrap
    {
        private readonly IGame _game;

        public Bootstrap(IGame game)
        {
            _game = game;
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

            if (fileName != null)
                _game.Run(fileName);
        }
    }
}