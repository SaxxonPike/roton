using Roton;
using Roton.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Torch
{
    public partial class Editor : Form
    {
        Actor _actor;
        int _color;
        Context _context;
        IEditorTerminal _terminal;

        public Editor(bool openGL = false)
        {
            var font1 = new Roton.Common.Font();
            var palette1 = new Roton.Common.Palette();

            InitializeComponent();
            this.Load += (object sender, EventArgs e) => { OnLoad(); };

            toolStrip3.Items.Add(new TileBufferToolStripItem());

            // Select and initialize the appropriate terminal.
            if (!openGL)
                _terminal = new Roton.WinForms.Terminal();
            else
                _terminal = new Roton.WinForms.OpenGL.Terminal();

            _terminal.Top = 0;
            _terminal.Left = 0;
            _terminal.Width = 640;
            _terminal.Height = 350;
            _terminal.AutoSize = true;
            _terminal.TerminalFont = font1;
            _terminal.TerminalPalette = palette1;
            mainPanel.Controls.Add((UserControl)_terminal);
        }

        Actor Actor
        {
            get
            {
                return _actor;
            }
            set
            {
                _actor = value;
                actorEditor.Actor = value;
                if (_actor.IsAttached)
                {
                    actorSourceLabel.Text = "Editing on-board actor";
                }
                else
                {
                    actorSourceLabel.Text = "Editing in-buffer actor";
                }
            }
        }

        ContextMenuStrip BuildBoardContextMenu(int parameterIndex)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
            int index = 0;

            foreach (var board in Context.Boards)
            {
                var item = new ToolStripMenuItem();
                item.Text = board.Name;
                item.Tag = index;
                item.Click += (object sender, EventArgs e) => { SetParameter(parameterIndex, (int)(sender as ToolStripItem).Tag); };
                items.Add(item);
                index++;
            }

            result.Items.AddRange(items.ToArray());
            return result;
        }

        void BuildBoardList()
        {
            int index = 0;
            boardsMenu.DropDownItems.Clear();
            foreach (var board in Context.Boards)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = "[" + index.ToString() + "] " + board.Name;
                item.Tag = index;
                item.Click += (object sender, EventArgs e) => { SetBoard((int)(sender as ToolStripItem).Tag); };
                boardsMenu.DropDownItems.Add(item);
                index++;
            }
        }

        ContextMenuStrip BuildCharacterContextMenu(int parameterIndex)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            for (int i = 0; i < 256; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = "Char #" + i.ToString() + " / " + i.ToHex8() + "h";
                item.Image = _terminal.RenderSingle(i, Color);
                item.ImageScaling = ToolStripItemImageScaling.None;
                item.Tag = i;
                item.Click += (object sender, EventArgs e) => { SetParameter(parameterIndex, (int)(sender as ToolStripMenuItem).Tag); };
                items.Add(item);
            }

            result.Items.AddRange(items.ToArray());
            result.Items[Actor.P1].Select();
            return result;
        }

        ToolStripItem BuildContextMenuItem(string text, Action click = null)
        {
            if (text == "-")
            {
                return new ToolStripSeparator();
            }
            return new ToolStripMenuItem(text, null, (object sender, EventArgs e) => { if (click != null) click(); });
        }

        ContextMenuStrip BuildElementContextMenu(int menuNumber)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            int index = 0;

            foreach (var element in Context.Elements)
            {
                if (element.Menu == menuNumber)
                {
                    if (element.Category.Length > 0)
                    {
                        ToolStripMenuItem categoryItem = new ToolStripMenuItem();
                        categoryItem.Text = element.Category;
                        categoryItem.Enabled = false;
                        if (result.Items.Count > 0)
                        {
                            result.Items.Add(new ToolStripSeparator());
                        }
                        result.Items.Add(categoryItem);
                    }
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = "(&" + char.ConvertFromUtf32(element.Key) + ") " + element.Name;
                    item.Tag = index;
                    item.Image = _terminal.RenderSingle(element.Character, GetDefaultColor(element));
                    item.ImageScaling = ToolStripItemImageScaling.None;
                    item.Click += (object sender, EventArgs e) => { SelectElement((int)(sender as ToolStripMenuItem).Tag); };
                    result.Items.Add(item);
                }
                index++;
            }

            if (result.Items.Count > 0)
                return result;
            else
                return null;
        }

        void BuildElementList()
        {
            elementComboBox.Items.Clear();
            foreach (var element in Context.Elements)
            {
                if (showUndefinedElementsButton.Checked || !string.IsNullOrWhiteSpace(element.ToString()))
                {
                    elementComboBox.Items.Add(new ElementItem(element));
                }
            }
        }

        ContextMenuStrip BuildParameterContextMenu(int parameterIndex, bool includeHeader)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            ToolStripMenuItem category = new ToolStripMenuItem();
            int preferredValue = 0;

            switch (parameterIndex)
            {
                case 1:
                    category.Text = SelectedElement.P1;
                    preferredValue = Actor.P1;
                    break;
                case 2:
                    category.Text = SelectedElement.P2;
                    preferredValue = Actor.P2;
                    break;
                case 3:
                    category.Text = SelectedElement.P3;
                    preferredValue = Actor.P3;
                    break;
            }

            if (string.IsNullOrWhiteSpace(category.Text))
                return null;

            category.Enabled = false;

            if (includeHeader)
            {
                result.Items.Add(category);
                result.Items.Add(new ToolStripSeparator());
                preferredValue += 2;
            }

            for (int i = 1; i <= 9; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = i.ToString();
                item.Tag = i - 1;
                item.Click += (object sender, EventArgs e) => { SetParameter(parameterIndex, (int)(sender as ToolStripMenuItem).Tag); };
                result.Items.Add(item);
            }

            if (preferredValue < result.Items.Count && preferredValue >= 0)
            {
                result.Items[preferredValue].Select();
                result.Items[preferredValue].Text += " (current)";
            }
            return result;
        }

        ContextMenuStrip BuildStepContextMenu(bool includeHeader)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            ToolStripMenuItem category = new ToolStripMenuItem();

            category.Text = SelectedElement.Step;
            if (string.IsNullOrWhiteSpace(category.Text))
                return null;

            category.Enabled = false;
            if (includeHeader)
            {
                result.Items.Add(category);
                result.Items.Add(new ToolStripSeparator());
            }

            for (int i = 0; i < 4; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                switch (i)
                {
                    case 0:
                        item.Text = "North";
                        item.Tag = Vector.North;
                        break;
                    case 1:
                        item.Text = "South";
                        item.Tag = Vector.South;
                        break;
                    case 2:
                        item.Text = "West";
                        item.Tag = Vector.West;
                        break;
                    case 3:
                        item.Text = "East";
                        item.Tag = Vector.East;
                        break;
                }
                item.Click += (object sender, EventArgs e) => { SetStep((Vector)(sender as ToolStripMenuItem).Tag); };
                result.Items.Add(item);
            }

            result.Items[2].Select();
            return result;
        }

        ContextMenuStrip BuildTileContextMenu(int x, int y)
        {
            ContextMenuStrip result = new ContextMenuStrip();
            bool actorPresent = false;
            Tile tile = Context.TileAt(x + 1, y + 1);

            foreach (var actor in Context.Actors)
            {
                if (actor.Location.X == x + 1 && actor.Location.Y == y + 1)
                {
                    actorPresent = true;
                    break;
                }
            }

            if (actorPresent)
            {
                var element = Context.Elements[tile.Id];
                if (!string.IsNullOrWhiteSpace(element.P1))
                {
                    var submenu = GetParameterMenu(1);
                    var item = new ToolStripMenuItem();
                    item.Text = element.P1;
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.P2))
                {
                    var submenu = GetParameterMenu(2);
                    var item = new ToolStripMenuItem();
                    item.Text = element.P2;
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.P3))
                {
                    var submenu = GetParameterMenu(3);
                    var item = new ToolStripMenuItem();
                    item.Text = element.P3;
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.Board))
                {
                    var submenu = BuildBoardContextMenu(3);
                    var item = new ToolStripMenuItem();
                    item.Text = element.Board;
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.Step))
                {
                    var submenu = BuildStepContextMenu(false);
                    var item = new ToolStripMenuItem();
                    item.Text = element.Step;
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.Code))
                {
                    result.Items.Add(BuildContextMenuItem(element.Code, ShowTileCode));
                }
                if (result.Items.Count > 0)
                {
                    result.Items.Add(BuildContextMenuItem("-"));
                    result.Items.Add(BuildContextMenuItem("Close"));
                }
            }

            return (result.Items.Count > 0) ? result : null;
        }

        int Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        Context Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
                LoadContext();
            }
        }

        void CopyCursorColor()
        {
            var tile = Context.TileAt(_terminal.CursorX + 1, _terminal.CursorY + 1);
            Color = tile.Color;
            UpdateColor();
        }

        void CopyCursorElement()
        {
            var tile = Context.TileAt(_terminal.CursorX + 1, _terminal.CursorY + 1);
            {
                SelectElement(tile.Id);
            }
        }

        void CopyCursorTile()
        {
            CopyCursorElement();
            CopyCursorColor();
            var actor = Context.CreateActor();
            actor.DuplicateFrom(Actor);
            this.Actor = actor;
        }

        void CycleAdvance()
        {
            if (Context != null)
            {
                lock (Context)
                {
                    Context.ExecuteOnce();
                }
            }
        }

        void CycleBackgroundColor()
        {
            Color = (Color + 0x10) & 0xFF;
            UpdateColor();
        }

        void CycleForegroundColor()
        {
            Color = (Color & 0xF0) | ((Color + 1) & 0x0F);
            UpdateColor();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            timerDaemon.Dispose();
            base.Dispose(disposing);
        }

        void EditCode()
        {
            codeEditor.Visible = true;
            codeEditor.Actor = Actor;
        }

        string FileFilters
        {
            get { return "Game Worlds (*.zzt;*.szt)|*.zzt;*.szt;*.ZZT;*.SZT|ZZT Worlds (*.zzt)|*.zzt;*.ZZT|Super ZZT Worlds (*.szt)|*.szt;*.SZT|Saved Games (*.sav)|*.sav;*.SAV|All Openable Files (*.zzt;*.szt;*.sav)|*.zzt;*.szt;*.sav;*.ZZT;*.SZT;*.SAV|All Files (*.*)|*.*"; }
        }

        void GetCursorActor()
        {
            Actor = Context.CreateActor();

            int x = _terminal.CursorX + 1;
            int y = _terminal.CursorY + 1;
            foreach (var actor in Context.Actors)
            {
                if (actor.Location.X == x && actor.Location.Y == y)
                {
                    Actor = actor;
                    break;
                }
            }
            actorEditor.Actor = Actor;
        }

        int GetDefaultColor(Element element)
        {
            if (defaultElementPropertiesButton.Checked)
            {
                if (element.Color < 0xF0)
                {
                    return element.Color;
                }
                else if (element.Color == 0xFE)
                {
                    return (((Color - 8) << 4) + 0x0F) & 0xFF;
                }
            }
            return Color;
        }

        ContextMenuStrip GetParameterMenu(int index)
        {
            string selectedParameter = null;
            switch (index)
            {
                case 1: selectedParameter = SelectedElement.P1; break;
                case 2: selectedParameter = SelectedElement.P2; break;
                case 3: selectedParameter = SelectedElement.P3; break;
                default: return null;
            }

            switch (selectedParameter.ToLowerInvariant())
            {
                case "character?":
                    return BuildCharacterContextMenu(index);
                default:
                    return BuildParameterContextMenu(index, false);
            }
        }

        void LoadContext()
        {
            timerDaemon.Pause();
            _terminal.Visible = true;
            Color = 0x0F;
            UpdateColor();
            SelectElement(0);
            UpdateElement();
            Actor = Context.CreateActor();
            Context.Terminal = _terminal;
            Context.Keyboard = keyboard;
            Context.Speaker = speaker;
            BuildElementList();
            BuildBoardList();
            UpdateInfo();
            boardEditor.Context = Context;
            worldEditor.Context = Context;
            Context.Refresh();
            timerDaemon.Resume();
        }

        void OnLoad()
        {
            // control events
            this.codeEditor.Closed += (object sender, EventArgs e) => { this.codeEditor.Visible = false; };
            this.elementComboBox.SelectedIndexChanged += (object sender, EventArgs e) => { if (elementComboBox.SelectedIndex >= 0) { SelectElement((elementComboBox.SelectedItem as ElementItem).Index); } };
            this.editBoardButton.Click += (object sender, EventArgs e) => { SelectBoard(3); };
            this.editCodeButton.Click += (object sender, EventArgs e) => { EditCode(); };
            this.editP1Button.Click += (object sender, EventArgs e) => { SelectParameter(1); };
            this.editP2Button.Click += (object sender, EventArgs e) => { SelectParameter(2); };
            this.editP3Button.Click += (object sender, EventArgs e) => { SelectParameter(3); };
            this.editStepButton.Click += (object sender, EventArgs e) => { SelectStep(); };
            this.openToolStripMenuItem.Click += (object sender, EventArgs e) => { ShowOpenWorld(); };
            this.saveToolStripMenuItem.Click += (object sender, EventArgs e) => { SaveWorld(); };
            this.saveAsToolStripMenuItem.Click += (object sender, EventArgs e) => { SaveWorldAs(); };
            this.scale2xButton.Click += (object sender, EventArgs e) => { int scale = this.scale2xButton.Checked ? 2 : 1; _terminal.SetScale(scale, scale); };
            this.showUndefinedElementsButton.CheckedChanged += (object sender, EventArgs e) => { BuildElementList(); };
            this.statsEnabledButton.CheckedChanged += (object sender, EventArgs e) => { UpdateElement(); };
            this.testMenuButton.Click += (object sender, EventArgs e) => { TestWorld(); };

            // tools
            foreach (var tool in Tools)
            {
                tool.Click += (object sender, EventArgs e) => { SetTool(sender as ToolStripButton); };
            }

            // terminal events
            _terminal.MouseDown += TerminalMouseDown;

            // other settings
            _terminal.Visible = false;
            _terminal.CursorEnabled = true;
            _terminal.AttachKeyHandler(this);
            mainPanel.HorizontalScroll.Visible = true;
            mainPanel.VerticalScroll.Visible = true;
            codeEditor.Dock = DockStyle.Fill;
            Actor = new Actor();

            // timer settings
            timerDaemon.Start(CycleAdvance, 71.8f / 9f);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys modifier = keyData & Keys.Modifiers;
            Keys code = keyData & Keys.KeyCode;

            if (msg.Msg == 0x100) // WM_KEYDOWN
            {
                if (textEnabledButton.Checked)
                {
                    if (modifier == Keys.None)
                    {
                        switch (code)
                        {
                            case Keys.F4:
                                textEnabledButton.CheckState = CheckState.Unchecked;
                                break;
                        }
                    }
                }
                else if (modifier == Keys.None)
                {
                    switch (code)
                    {
                        case Keys.C:
                            CycleForegroundColor();
                            break;
                        case Keys.P:
                            break;
                        case Keys.F1:
                        case Keys.F2:
                        case Keys.F3:
                            ShowElementContextMenu((code - Keys.F1) + 1);
                            break;
                        case Keys.F4:
                            textEnabledButton.CheckState = CheckState.Checked;
                            break;
                        case Keys.F5:
                        case Keys.F6:
                            ShowElementContextMenu((code - Keys.F1));
                            break;
                    }
                }
                else if (modifier == Keys.Shift)
                {
                    switch (code)
                    {
                        case Keys.C:
                            CycleBackgroundColor();
                            break;
                        case Keys.P:
                            break;
                    }
                }
                else if (modifier == Keys.Control)
                {
                    switch (code)
                    {
                        case Keys.D1:
                        case Keys.D2:
                        case Keys.D3:
                            SelectParameter((code - Keys.D1) + 1);
                            break;
                        case Keys.D4:
                            SelectBoard(3);
                            break;
                        case Keys.D5:
                            SelectStep();
                            break;
                        case Keys.D6:
                            EditCode();
                            break;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        void SaveWorld()
        {
            if (string.IsNullOrWhiteSpace(WorldFileName))
            {
                SaveWorldAs();
            }
            else
            {
                Context.Save(WorldFileName);
            }
        }

        void SaveWorldAs()
        {
            if (ShowSaveWorld() == System.Windows.Forms.DialogResult.OK)
            {
                Context.Save(WorldFileName);
            }
        }

        void SelectBoard(int parameterIndex)
        {
            var element = SelectedElement;

            if (element != null && !string.IsNullOrWhiteSpace(element.Board))
            {
                var menu = BuildBoardContextMenu(parameterIndex);
                ShowParameterDropdown(parameterIndex, menu);
            }
        }

        Element SelectedElement
        {
            get
            {
                if (elementComboBox.SelectedIndex < 0)
                    return null;
                return (elementComboBox.SelectedItem as ElementItem).Element;
            }
        }

        int SelectedElementIndex
        {
            get
            {
                if (elementComboBox.SelectedIndex < 0)
                    return -1;
                return (elementComboBox.SelectedItem as ElementItem).Index;
            }
        }

        void SelectElement(int index)
        {
            foreach (var item in elementComboBox.Items)
            {
                var eitem = (item as ElementItem);
                if (eitem.Index == index)
                {
                    elementComboBox.SelectedItem = item;
                }
            }

            var element = Context.Elements[index];
            statsEnabledButton.Checked = (element.Cycle >= 0);
            UpdateElement();
        }

        void SelectParameter(int index)
        {
            var menu = GetParameterMenu(index);
            if (menu != null)
            {
                ShowParameterDropdown(index, menu);
            }
        }

        void SelectStep()
        {
        }

        void SetBoard(int index)
        {
            // make a backup copy of the current actor
            Actor actor = Context.CreateActor();
            actor.CopyFrom(this.Actor);
            actor.Code = this.Actor.Code;
            this.Actor = actor;

            try
            {
                Context.SetBoard(index);
                boardEditor.UpdateData();
                worldEditor.UpdateData();
            }
            catch (Exception e)
            {
                var result = MessageBox.Show("An error occurred when switching boards: " + Environment.NewLine +
                    e.Message + Environment.NewLine + Environment.NewLine +
                    "Would you like to attempt to reload the previously packed version of the board?",
                    "Board Switch Error", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        Context.UnpackBoard();
                    }
                    catch (Exception f)
                    {
                        MessageBox.Show("An error occurred during board recovery:" +
                            Environment.NewLine + f.Message, "Board Recovery Error", MessageBoxButtons.OK);
                    }
                }
            }
            finally
            {
                UpdateInfo();
                Context.Refresh();
            }
        }

        void SetBoardParameter(int value)
        {
            Actor.P3 = value;
        }

        void SetParameter(int index, int value)
        {
            if (index == 1)
            {
                Actor.P1 = value;
            }
            else if (index == 2)
            {
                Actor.P2 = value;
            }
            else if (index == 3)
            {
                Actor.P3 = value;
            }
        }

        void SetStep(Vector vector)
        {
            Actor.Vector.CopyFrom(vector);
        }

        void SetTool(ToolStripButton button)
        {
            foreach (var tool in Tools)
            {
                tool.Checked = (tool == button);
            }
        }

        void ShowElementContextMenu(int index)
        {
            var menu = BuildElementContextMenu(index);
            if (menu != null)
            {
                menu.Show(tabControl, new Point(0, 0));
            }
        }

        void ShowOpenWorld()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = FileFilters;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Context = new Context(ofd.FileName, true);
                WorldFileName = ofd.FileName;
            }
        }

        void ShowParameterDropdown(int index, ContextMenuStrip menu)
        {
            if (menu != null)
            {
                if (index == 1)
                {
                    menu.Show(editP1Button, new Point(0, 0));
                }
                else if (index == 2)
                {
                    menu.Show(editP2Button, new Point(0, 0));
                }
                else if (index == 3)
                {
                    menu.Show(editP3Button, new Point(0, 0));
                }
            }
        }

        DialogResult ShowSaveWorld()
        {
            if (Context != null)
            {
                var sfd = new SaveFileDialog();
                sfd.AddExtension = true;
                sfd.Filter = FileFilters;
                if (Context.WorldData.Locked)
                {
                    sfd.FilterIndex = 4; // saved game
                }
                else
                {
                    switch (Context.WorldData.WorldType)
                    {
                        case -1: // ZZT
                            sfd.FilterIndex = 2;
                            break;
                        case -2: // Super ZZT
                            sfd.FilterIndex = 3;
                            break;
                    }
                }
                var result = sfd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WorldFileName = sfd.FileName;
                }
                return result;
            }
            return System.Windows.Forms.DialogResult.Cancel;
        }

        void ShowTileCode()
        {
            GetCursorActor();
            EditCode();
        }

        void ShowTileProperties()
        {
            GetCursorActor();
            CopyCursorElement();
            tabControl.SelectTab(editTab);
        }

        void TerminalMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                GetCursorActor();
                CopyCursorElement();
                CopyCursorColor();
                var menu = BuildTileContextMenu(_terminal.CursorX, _terminal.CursorY);
                if (menu != null)
                {
                    menu.Show(sender as Control, e.Location);
                }
            }
        }

        void TestWorld()
        {
            if (Context != null)
            {
                using (MemoryStream mem = new MemoryStream(Context.Save()))
                {
                    var gameForm = new Lyon.GameForm(mem);
                    gameForm.Show();
                }
            }
        }

        IList<ToolStripButton> Tools
        {
            get
            {
                return new List<ToolStripButton>(new ToolStripButton[] {
                    drawToolButton,
                    eraseToolButton,
                    inspectToolButton,
                    randomToolButton
                });
            }
        }

        void UpdateActors()
        {
            // we subtract 1 from the capacity because there must always be room for the messenger object
            actorInfoLabel.Text = Context.Actors.Count.ToString() + "/" + (Context.ActorCapacity - 1).ToString();
        }

        void UpdateColor()
        {
            UpdateColorButton(foregroundColorButton, _terminal.TerminalPalette[Color & 0x0F]);
            UpdateColorButton(backgroundColorButton, _terminal.TerminalPalette[(Color >> 4) & 0x0F]);
        }

        void UpdateColorButton(ToolStripButton button, Color backgroundColor)
        {
            button.BackColor = backgroundColor;
            
            // ensure foreground text is always visible by contrast
            if (((backgroundColor.R + backgroundColor.G + backgroundColor.B) / 3) >= 0x80)
            {
                button.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                button.ForeColor = System.Drawing.Color.White;
            }
        }

        void UpdateElement()
        {
            if (elementComboBox.SelectedIndex >= 0)
            {
                var element = Context.Elements[(elementComboBox.SelectedItem as ElementItem).Index];
                var editBoard = element.Board;
                var editCode = element.Code;
                var editP1 = element.P1;
                var editP2 = element.P2;
                var editP3 = element.P3;
                var editStep = element.Step;

                editBoardButton.Text = "<Ctrl-4> " + editBoard;
                editBoardButton.Visible = !string.IsNullOrWhiteSpace(editBoard) && statsEnabledButton.Checked;
                editCodeButton.Text = "<Ctrl-6> " + editCode;
                editCodeButton.Visible = !string.IsNullOrWhiteSpace(editCode) && statsEnabledButton.Checked;
                editP1Button.Text = "<Ctrl-1> " + editP1;
                editP1Button.Visible = !string.IsNullOrWhiteSpace(editP1) && statsEnabledButton.Checked;
                editP2Button.Text = "<Ctrl-2> " + editP2;
                editP2Button.Visible = !string.IsNullOrWhiteSpace(editP2) && statsEnabledButton.Checked;
                editP3Button.Text = "<Ctrl-3> " + editP3;
                editP3Button.Visible = !string.IsNullOrWhiteSpace(editP3) && statsEnabledButton.Checked;
                editStepButton.Text = "<Ctrl-5> " + editStep;
                editStepButton.Visible = !string.IsNullOrWhiteSpace(editStep) && statsEnabledButton.Checked;

                if (string.IsNullOrWhiteSpace(editCode))
                {
                    Actor.Pointer = -1;
                }
                if (string.IsNullOrWhiteSpace(editP1))
                {
                    Actor.P1 = 0;
                }
                if (string.IsNullOrWhiteSpace(editP2))
                {
                    Actor.P2 = 0;
                }
                if (string.IsNullOrWhiteSpace(editBoard) && string.IsNullOrWhiteSpace(editP3))
                {
                    Actor.P3 = 0;
                }
                if (string.IsNullOrWhiteSpace(editStep))
                {
                    Actor.Vector.X = 0;
                    Actor.Vector.Y = 0;
                }
            }
        }

        void UpdateInfo()
        {
            Context.PackBoard();
            boardInfoLabel.Text = (Context.Boards[Context.Board].Data.Length + 2).ToString() + "/20000";
            worldInfoLabel.Text = Context.WorldSize.ToString() + "/360000";
            UpdateActors();
        }

        string WorldFileName
        {
            get;
            set;
        }
    }
}
