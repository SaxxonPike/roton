using Roton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            get
            {
                return _context;
            }
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

        void InitializeEvents()
        {
            SuppressUpdate = true;

            // textboxes
            this.ammoTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Ammo = int.Parse((sender as TextBox).Text); };
            this.energizerTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.EnergyCycles = int.Parse((sender as TextBox).Text); };
            this.filenameTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Name = ((sender as TextBox).Text); };
            this.gemsTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Gems = int.Parse((sender as TextBox).Text); };
            this.healthTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Health = int.Parse((sender as TextBox).Text); };
            this.scoreTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Score = int.Parse((sender as TextBox).Text); };
            this.stonesTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Stones = int.Parse((sender as TextBox).Text); };
            this.timePassedTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.TimePassed = int.Parse((sender as TextBox).Text); };
            this.torchCycleTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.TorchCycles = int.Parse((sender as TextBox).Text); };
            this.torchesTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Torches = int.Parse((sender as TextBox).Text); };

            // checkboxes
            this.advancedEditingCheckBox.CheckStateChanged += (object sender, EventArgs e) => { UpdateAdvancedEdit(); };
            this.blueKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[0] = (sender as CheckBox).Checked; };
            this.cyanKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[2] = (sender as CheckBox).Checked; };
            this.greenKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[1] = (sender as CheckBox).Checked; };
            this.lockedCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Locked = (sender as CheckBox).Checked; };
            this.purpleKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[4] = (sender as CheckBox).Checked; };
            this.redKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[3] = (sender as CheckBox).Checked; };
            this.whiteKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[6] = (sender as CheckBox).Checked; };
            this.yellowKeyCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Keys[5] = (sender as CheckBox).Checked; };

            // comboboxes
            this.startBoardComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.WorldData.Board = (sender as ComboBox).SelectedIndex; };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        bool SuppressUpdate
        {
            get 
            { 
                return _suppressUpdateCount > 0 || _context == null;
            }
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
            this.startBoardComboBox.Items.Clear();
            this.startBoardComboBox.Items.AddRange(_context.Boards.ToArray());

            SuppressUpdate = false;
        }

        void UpdateAdvancedEdit()
        {
            bool enabled = this.advancedEditingCheckBox.Checked;
            this.filenameTextBox.Enabled = enabled;
            this.lockedCheckBox.Enabled = enabled;
            this.startBoardComboBox.Enabled = enabled;
        }

        public void UpdateData()
        {
            SuppressUpdate = true;

            // textboxes
            this.ammoTextBox.Text = _context.WorldData.Ammo.ToString();
            this.energizerTextBox.Text = _context.WorldData.EnergyCycles.ToString();
            this.filenameTextBox.Text = _context.WorldData.Name;
            this.gemsTextBox.Text = _context.WorldData.Gems.ToString();
            this.healthTextBox.Text = _context.WorldData.Health.ToString();
            this.scoreTextBox.Text = _context.WorldData.Score.ToString();
            this.stonesTextBox.Text = _context.WorldData.Stones.ToString();
            this.timePassedTextBox.Text = _context.WorldData.TimePassed.ToString();
            this.torchCycleTextBox.Text = _context.WorldData.TorchCycles.ToString();
            this.torchesTextBox.Text = _context.WorldData.Torches.ToString();

            // checkboxes
            this.blueKeyCheckBox.Checked = _context.WorldData.Keys[0];
            this.cyanKeyCheckBox.Checked = _context.WorldData.Keys[2];
            this.greenKeyCheckBox.Checked = _context.WorldData.Keys[1];
            this.lockedCheckBox.Checked = _context.WorldData.Locked;
            this.purpleKeyCheckBox.Checked = _context.WorldData.Keys[4];
            this.redKeyCheckBox.Checked = _context.WorldData.Keys[3];
            this.whiteKeyCheckBox.Checked = _context.WorldData.Keys[6];
            this.yellowKeyCheckBox.Checked = _context.WorldData.Keys[5];

            // comboboxes
            this.startBoardComboBox.SelectedIndex = _context.WorldData.Board;

            SuppressUpdate = false;
        }
    }
}
