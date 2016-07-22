using System.Collections.Generic;
using System.Windows.Forms;
using Roton.Core;

namespace Torch
{
    public partial class BoardEditor : UserControl
    {
        private IContext _context;
        private int _suppressUpdateCount;

        public BoardEditor()
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

        private void InitializeEvents()
        {
            SuppressUpdate = true;

            // textboxes
            cameraXTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.Camera.X = int.Parse(((TextBox) sender).Text);
            };
            cameraYTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.Camera.Y = int.Parse(((TextBox) sender).Text);
            };
            enteredXTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.Enter.X = int.Parse(((TextBox) sender).Text);
            };
            enteredYTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.Enter.Y = int.Parse(((TextBox) sender).Text);
            };
            exitEastTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) SetComboControlValue(exitEastTextBox, exitEastComboBox, ((TextBox) sender).Text);
            };
            exitNorthTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate)
                    SetComboControlValue(exitNorthTextBox, exitNorthComboBox, ((TextBox) sender).Text);
            };
            exitSouthTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate)
                    SetComboControlValue(exitSouthTextBox, exitSouthComboBox, ((TextBox) sender).Text);
            };
            exitWestTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) SetComboControlValue(exitWestTextBox, exitWestComboBox, ((TextBox) sender).Text);
            };
            maxShotsTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.Shots = int.Parse(((TextBox) sender).Text);
            };
            timeLimitTextBox.TextChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.TimeLimit = int.Parse(((TextBox) sender).Text);
            };

            // checkboxes
            advancedEditingCheckBox.CheckStateChanged += (sender, e) => { UpdateAdvancedEdit(); };
            zapRestartCheckBox.CheckStateChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.RestartOnZap = ((CheckBox) sender).Checked;
            };

            // comboboxes
            exitNorthComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.ExitEast = ((ComboBox) sender).SelectedIndex;
            };
            exitSouthComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.ExitNorth = ((ComboBox) sender).SelectedIndex;
            };
            exitSouthComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.ExitSouth = ((ComboBox) sender).SelectedIndex;
            };
            exitWestComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (!SuppressUpdate) _context.BoardData.ExitWest = ((ComboBox) sender).SelectedIndex;
            };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        private int SetComboControlValue(Control textbox, ComboBox combobox, string value)
        {
            SuppressUpdate = true;

            textbox.Text = value;
            var index = -1;
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

        private void UpdateAdvancedEdit()
        {
            var enabled = advancedEditingCheckBox.Checked;
            cameraXTextBox.Enabled = enabled;
            cameraYTextBox.Enabled = enabled;
            enteredXTextBox.Enabled = enabled;
            enteredYTextBox.Enabled = enabled;
        }

        private void UpdateBoards()
        {
            SuppressUpdate = true;

            // get the board list and exclude the title board
            var boards = new List<string>();
            foreach (var board in _context.Boards)
            {
                boards.Add(board.ToString());
            }
            boards[0] = "";

            // comboboxes
            exitEastComboBox.Items.Clear();
            exitEastComboBox.Items.AddRange(boards.ToArray());
            exitNorthComboBox.Items.Clear();
            exitNorthComboBox.Items.AddRange(boards.ToArray());
            exitSouthComboBox.Items.Clear();
            exitSouthComboBox.Items.AddRange(boards.ToArray());
            exitWestComboBox.Items.Clear();
            exitWestComboBox.Items.AddRange(boards.ToArray());

            UpdateBoardComboBoxes();
            SuppressUpdate = false;
        }

        private void UpdateBoardComboBoxes()
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
            cameraXTextBox.Text = _context.BoardData.Camera.X.ToString();
            cameraYTextBox.Text = _context.BoardData.Camera.Y.ToString();
            enteredXTextBox.Text = _context.BoardData.Enter.X.ToString();
            enteredYTextBox.Text = _context.BoardData.Enter.Y.ToString();
            maxShotsTextBox.Text = _context.BoardData.Shots.ToString();
            timeLimitTextBox.Text = _context.BoardData.TimeLimit.ToString();

            // checkboxes
            zapRestartCheckBox.Checked = _context.BoardData.RestartOnZap;

            UpdateBoardComboBoxes();
            SuppressUpdate = false;
        }
    }
}