using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class ActorEditor : UserControl
    {
        private IActor _actor;
        private int _suppressUpdateCount;

        public ActorEditor()
        {
            InitializeComponent();
        }

        public IActor Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                UpdateData();
            }
        }

        private bool SuppressUpdate
        {
            get { return _suppressUpdateCount > 0 || _actor == null; }
            set
            {
                _suppressUpdateCount += value ? 1 : -1;
                if (_suppressUpdateCount < 0)
                    _suppressUpdateCount = 0;
            }
        }

        public void UpdateData()
        {
            SuppressUpdate = true;

            // textboxes

            // checkboxes

            SuppressUpdate = false;
        }
    }
}