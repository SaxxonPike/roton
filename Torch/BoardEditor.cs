using Roton;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Torch
{
    public partial class BoardEditor : UserControl
    {
        private Context _context;
        private int _suppressUpdateCount;

        public BoardEditor()
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
            this.cameraXTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Camera.X = int.Parse((sender as TextBox).Text); };
            this.cameraYTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Camera.Y = int.Parse((sender as TextBox).Text); };
            this.enteredXTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Enter.X = int.Parse((sender as TextBox).Text); };
            this.enteredYTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Enter.Y = int.Parse((sender as TextBox).Text); };
            this.exitEastTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) SetComboControlValue(exitEastTextBox, exitEastComboBox, (sender as TextBox).Text); };
            this.exitNorthTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) SetComboControlValue(exitNorthTextBox, exitNorthComboBox, (sender as TextBox).Text); };
            this.exitSouthTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) SetComboControlValue(exitSouthTextBox, exitSouthComboBox, (sender as TextBox).Text); };
            this.exitWestTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) SetComboControlValue(exitWestTextBox, exitWestComboBox, (sender as TextBox).Text); };
            this.maxShotsTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Shots = int.Parse((sender as TextBox).Text); };
            this.timeLimitTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.TimeLimit = int.Parse((sender as TextBox).Text); };

            // checkboxes
            this.advancedEditingCheckBox.CheckStateChanged += (object sender, EventArgs e) => { UpdateAdvancedEdit(); };
            this.zapRestartCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.RestartOnZap = (sender as CheckBox).Checked; };

            // comboboxes
            this.exitNorthComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.ExitEast = (sender as ComboBox).SelectedIndex; };
            this.exitSouthComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.ExitNorth = (sender as ComboBox).SelectedIndex; };
            this.exitSouthComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.ExitSouth = (sender as ComboBox).SelectedIndex; };
            this.exitWestComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.ExitWest = (sender as ComboBox).SelectedIndex; };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        int SetComboControlValue(TextBox textbox, ComboBox combobox, string value)
        {
            SuppressUpdate = true;

            textbox.Text = value;
            int index = -1;
            if (string.IsNullOrWhiteSpace(value))
            {
                combobox.SelectedIndex = -1;
            }
            else if (int.TryParse(value, out index))
            {
                if (index >= 0 && index < combobox.Items.Count)
                {
                    combobox.SelectedIndex = index;
                }
                else
                {
                    combobox.SelectedIndex = -1;
                }
            }

            SuppressUpdate = false;
            return index;
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

        void UpdateAdvancedEdit()
        {
            bool enabled = this.advancedEditingCheckBox.Checked;
            this.cameraXTextBox.Enabled = enabled;
            this.cameraYTextBox.Enabled = enabled;
            this.enteredXTextBox.Enabled = enabled;
            this.enteredYTextBox.Enabled = enabled;
        }

        public void UpdateBoards()
        {
            SuppressUpdate = true;

            // get the board list and exclude the title board
            List<string> boards = new List<string>();
            foreach (var board in _context.Boards)
            {
                boards.Add(board.ToString());
            }
            boards[0] = "";

            // comboboxes
            this.exitEastComboBox.Items.Clear();
            this.exitEastComboBox.Items.AddRange(boards.ToArray());
            this.exitNorthComboBox.Items.Clear();
            this.exitNorthComboBox.Items.AddRange(boards.ToArray());
            this.exitSouthComboBox.Items.Clear();
            this.exitSouthComboBox.Items.AddRange(boards.ToArray());
            this.exitWestComboBox.Items.Clear();
            this.exitWestComboBox.Items.AddRange(boards.ToArray());

            UpdateBoardComboBoxes();
            SuppressUpdate = false;
        }

        void UpdateBoardComboBoxes()
        {
            SuppressUpdate = true;
            SetComboControlValue(exitEastTextBox, exitEastComboBox, _context.BoardData.ExitEast.ToString());
            SetComboControlValue(exitNorthTextBox, exitNorthComboBox, _context.BoardData.ExitNorth.ToString());
            SetComboControlValue(exitSouthTextBox, exitSouthComboBox, _context.BoardData.ExitSouth.ToString());
            SetComboControlValue(exitWestTextBox, exitWestComboBox, _context.BoardData.ExitWest.ToString());
            SuppressUpdate = false;
        }

        public void UpdateData()
        {
            SuppressUpdate = true;

            // textboxes
            this.cameraXTextBox.Text = _context.BoardData.Camera.X.ToString();
            this.cameraYTextBox.Text = _context.BoardData.Camera.Y.ToString();
            this.enteredXTextBox.Text = _context.BoardData.Enter.X.ToString();
            this.enteredYTextBox.Text = _context.BoardData.Enter.Y.ToString();
            this.maxShotsTextBox.Text = _context.BoardData.Shots.ToString();
            this.timeLimitTextBox.Text = _context.BoardData.TimeLimit.ToString();

            // checkboxes
            this.zapRestartCheckBox.Checked = _context.BoardData.RestartOnZap;

            UpdateBoardComboBoxes();
            SuppressUpdate = false;
        }
    }
}
