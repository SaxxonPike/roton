using System.Linq;
using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class WorldEditor : UserControl
    {
        private Context _context;
        private int _suppressUpdateCount;

        public WorldEditor()
        {
            InitializeComponent();
            InitializeEvents();
        }

        public Context Context
        {
            get { return _context; }
            set
            {
                SuppressUpdate = true;
                _context = value;
                if (value != null)
                {
                    UpdateBoards();
                    UpdateData();
                }
                SuppressUpdate = false;
            }
        }

        private void InitializeEvents()
        {
            SuppressUpdate = true;

            // textboxes
            ammoTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Ammo = int.Parse((sender as TextBox).Text);
            };
            energizerTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.EnergyCycles = int.Parse((sender as TextBox).Text);
            };
            filenameTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Name = (sender as TextBox).Text;
            };
            gemsTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Gems = int.Parse((sender as TextBox).Text);
            };
            healthTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Health = int.Parse((sender as TextBox).Text);
            };
            scoreTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Score = int.Parse((sender as TextBox).Text);
            };
            stonesTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Stones = int.Parse((sender as TextBox).Text);
            };
            timePassedTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.TimePassed = int.Parse((sender as TextBox).Text);
            };
            torchCycleTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.TorchCycles = int.Parse((sender as TextBox).Text);
            };
            torchesTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Torches = int.Parse((sender as TextBox).Text);
            };

            // checkboxes
            advancedEditingCheckBox.CheckStateChanged += (sender, e) => { UpdateAdvancedEdit(); };
            blueKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[0] = (sender as CheckBox).Checked;
            };
            cyanKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[2] = (sender as CheckBox).Checked;
            };
            greenKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[1] = (sender as CheckBox).Checked;
            };
            lockedCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Locked = (sender as CheckBox).Checked;
            };
            purpleKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[4] = (sender as CheckBox).Checked;
            };
            redKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[3] = (sender as CheckBox).Checked;
            };
            whiteKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[6] = (sender as CheckBox).Checked;
            };
            yellowKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[5] = (sender as CheckBox).Checked;
            };

            // comboboxes
            startBoardComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Board = (sender as ComboBox).SelectedIndex;
            };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        private bool SuppressUpdate
        {
            get { return _suppressUpdateCount > 0 || _context == null; }
            set
            {
                _suppressUpdateCount += value ? 1 : -1;
                if (_suppressUpdateCount < 0)
                    _suppressUpdateCount = 0;
            }
        }

        public void UpdateBoards()
        {
            SuppressUpdate = true;

            // comboboxes
            startBoardComboBox.Items.Clear();
            startBoardComboBox.Items.AddRange(_context.Boards.ToArray());

            SuppressUpdate = false;
        }

        private void UpdateAdvancedEdit()
        {
            var enabled = advancedEditingCheckBox.Checked;
            filenameTextBox.Enabled = enabled;
            lockedCheckBox.Enabled = enabled;
            startBoardComboBox.Enabled = enabled;
        }

        public void UpdateData()
        {
            SuppressUpdate = true;

            // textboxes
            ammoTextBox.Text = _context.WorldData.Ammo.ToString();
            energizerTextBox.Text = _context.WorldData.EnergyCycles.ToString();
            filenameTextBox.Text = _context.WorldData.Name;
            gemsTextBox.Text = _context.WorldData.Gems.ToString();
            healthTextBox.Text = _context.WorldData.Health.ToString();
            scoreTextBox.Text = _context.WorldData.Score.ToString();
            stonesTextBox.Text = _context.WorldData.Stones.ToString();
            timePassedTextBox.Text = _context.WorldData.TimePassed.ToString();
            torchCycleTextBox.Text = _context.WorldData.TorchCycles.ToString();
            torchesTextBox.Text = _context.WorldData.Torches.ToString();

            // checkboxes
            blueKeyCheckBox.Checked = _context.WorldData.Keys[0];
            cyanKeyCheckBox.Checked = _context.WorldData.Keys[2];
            greenKeyCheckBox.Checked = _context.WorldData.Keys[1];
            lockedCheckBox.Checked = _context.WorldData.Locked;
            purpleKeyCheckBox.Checked = _context.WorldData.Keys[4];
            redKeyCheckBox.Checked = _context.WorldData.Keys[3];
            whiteKeyCheckBox.Checked = _context.WorldData.Keys[6];
            yellowKeyCheckBox.Checked = _context.WorldData.Keys[5];

            // comboboxes
            startBoardComboBox.SelectedIndex = _context.WorldData.Board;

            SuppressUpdate = false;
        }
    }
}