﻿using Roton;
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
    public partial class ActorEditor : UserControl
    {
        private Actor _actor;
        private int _suppressUpdateCount;

        public ActorEditor()
        {
            InitializeComponent();
        }

        public Actor Actor
        {
            get
            {
                return _actor;
            }
            set
            {
                _actor = value;
                UpdateData();
            }
        }

        void InitializeEvents()
        {
            SuppressUpdate = true;

            // textboxes
            //this.cameraXTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.Camera.X = int.Parse((sender as TextBox).Text); };
            this.cycleTextBox.TextChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _actor.Cycle = int.Parse((sender as TextBox).Text); };

            // checkboxes
            this.advancedEditingCheckBox.CheckStateChanged += (object sender, EventArgs e) => { UpdateAdvancedEdit(); };
            //this.zapRestartCheckBox.CheckStateChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.RestartOnZap = (sender as CheckBox).Checked; };

            // comboboxes
            //this.exitEastComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (!SuppressUpdate) _context.BoardData.ExitEast = (sender as ComboBox).SelectedIndex; };

            UpdateAdvancedEdit();
            SuppressUpdate = false;
        }

        bool SuppressUpdate
        {
            get
            {
                return _suppressUpdateCount > 0 || _actor == null;
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