using System;
using System.Windows.Forms;
using Roton.Core;
using Roton.Extensions;

namespace Torch
{
    public partial class CodeEditor : UserControl
    {
        private IActor _actor;

        public CodeEditor()
        {
            InitializeComponent();
            Initialize();
        }

        public CodeEditor(IActor actor)
        {
            InitializeComponent();
            Initialize();
            Actor = actor;
        }

        public IActor Actor
        {
            get { return _actor; }
            set
            {
                if (value != null)
                {
                    _actor = value;
                    codeTextBox.Text = _actor.GetCodeAsString();
                }
            }
        }

        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Closed;

        private void Initialize()
        {
            cancelButton.Click += (sender, e) => { Close(); };
            okButton.Click += (sender, e) => { SaveAndClose(); };
        }

        private void SaveAndClose()
        {
            _actor?.ModifyCodeAsString(codeTextBox.Text);
            Close();
        }
    }
}