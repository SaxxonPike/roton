using Roton;
using System;
using System.Windows.Forms;

namespace Torch
{
    public partial class CodeEditor : UserControl
    {
        private IActor _actor;

        public event EventHandler Closed;

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
                    codeTextBox.Text = new string(_actor.Code ?? new char[0]);
                }
            }
        }

        private void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void Initialize()
        {
            cancelButton.Click += (sender, e) => { Close(); };
            okButton.Click += (sender, e) => { SaveAndClose(); };
        }

        private void SaveAndClose()
        {
            if (_actor != null)
            {
                _actor.Code = codeTextBox.Text.ToCharArray();
            }
            Close();
        }
    }
}