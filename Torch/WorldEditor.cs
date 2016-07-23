using System.Linq;
using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class WorldEditor : UserControl
    {
        private IContext _context;
        private int _suppressUpdateCount;

        public WorldEditor()
        {
            InitializeComponent();
            InitializeEvents();
        }

        public IContext Context
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

        private void InitializeEvents()
        {
            SuppressUpdate = true;

            // textboxes
            ammoTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Ammo = int.Parse(((TextBox) sender).Text);
            };
            energizerTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.EnergyCycles = int.Parse(((TextBox) sender).Text);
            };
            filenameTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Name = ((TextBox) sender).Text;
            };
            gemsTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Gems = int.Parse(((TextBox) sender).Text);
            };
            healthTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Health = int.Parse(((TextBox) sender).Text);
            };
            scoreTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Score = int.Parse(((TextBox) sender).Text);
            };
            stonesTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Stones = int.Parse(((TextBox) sender).Text);
            };
            timePassedTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.TimePassed = int.Parse(((TextBox) sender).Text);
            };
            torchCycleTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.TorchCycles = int.Parse(((TextBox) sender).Text);
            };
            torchesTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Torches = int.Parse(((TextBox) sender).Text);
            };

            // checkboxes
            advancedEditingCheckBox.CheckStateChanged += (sender, e) => { UpdateAdvancedEdit(); };
            blueKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[0] = ((CheckBox) sender).Checked;
            };
            cyanKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[2] = ((CheckBox) sender).Checked;
            };
            greenKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[1] = ((CheckBox) sender).Checked;
            };
            lockedCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Locked = ((CheckBox) sender).Checked;
            };
            purpleKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[4] = ((CheckBox) sender).Checked;
            };
            redKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[3] = ((CheckBox) sender).Checked;
            };
            whiteKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[6] = ((CheckBox) sender).Checked;
            };
            yellowKeyCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Keys[5] = ((CheckBox) sender).Checked;
            };

            // comboboxes
            startBoardComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.WorldData.Board = ((ComboBox) sender).SelectedIndex;
            };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        private void UpdateAdvancedEdit()
        {
            var enabled = advancedEditingCheckBox.Checked;
            filenameTextBox.Enabled = enabled;
            lockedCheckBox.Enabled = enabled;
            startBoardComboBox.Enabled = enabled;
        }

        public void UpdateBoards()
        {
            SuppressUpdate = true;

            // comboboxes
            startBoardComboBox.Items.Clear();
            startBoardComboBox.Items.AddRange(_context.Boards.ToArray());

            SuppressUpdate = false;
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