using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Lyon;
using Roton.Common;
using Roton.Core;
using Roton.Extensions;
using Roton.FileIo;
using Roton.WinForms;
using Message = System.Windows.Forms.Message;

namespace Torch
{
    public partial class Editor : Form
    {
        private static readonly string WorldFileFilters = string.Join("|",
            "Game Worlds (*.zzt;*.szt)", "*.zzt;*.szt;*.ZZT;*.SZT",
            "ZZT Worlds (*.zzt)", "*.zzt;*.ZZT",
            "Super ZZT Worlds (*.szt)", "*.szt;*.SZT",
            "Saved Games (*.sav)", "*.sav;*.SAV",
            "All Openable Files (*.zzt;*.szt;*.sav)", "*.zzt;*.szt;*.sav;*.ZZT;*.SZT;*.SAV",
            "All Files (*.*)", "*.*"
            );

        private static readonly string ScreenshotFileFilters = string.Join("|",
            "PNG Images (*.png)", "*.png;*.PNG"
            );

        private readonly IEditorTerminal _terminal;
        private IActor _actor;
        private IContext _context;

        public Editor(bool openGl = false)
        {
            var font1 = new RasterFont();
            var palette1 = new Palette();

            InitializeComponent();
            Load += (sender, e) => { OnLoad(); };

            toolStrip3.Items.Add(new TileBufferToolStripItem());

            // Select and initialize the appropriate terminal.
            if (!openGl)
                _terminal = new Terminal();
            else
                _terminal = new Roton.WinForms.OpenGL.Terminal();

            _terminal.Top = 0;
            _terminal.Left = 0;
            _terminal.Width = 640;
            _terminal.Height = 350;
            _terminal.AutoSize = true;
            _terminal.TerminalFont = font1;
            _terminal.TerminalPalette = palette1;
            mainPanel.Controls.Add((UserControl) _terminal);
        }

        private IElement SelectedElement => elementComboBox.SelectedIndex < 0
            ? null
            : ((ElementItem) elementComboBox.SelectedItem).Element;

        private IEnumerable<ToolStripButton> Tools => new List<ToolStripButton>(new[]
        {
            drawToolButton,
            eraseToolButton,
            inspectToolButton,
            randomToolButton
        });

        private IActor Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                actorEditor.Actor = value;
                actorSourceLabel.Text = "Editing actor";
            }
        }

        private int Color { get; set; }

        private IContext Context
        {
            get { return _context; }
            set
            {
                _context = value;
                LoadContext();
            }
        }

        private string WorldFileName { get; set; }

        private ContextMenuStrip BuildBoardContextMenu(int parameterIndex)
        {
            var result = new ContextMenuStrip();
            var items = new List<ToolStripMenuItem>();
            var index = 0;

            foreach (var board in Context.Boards)
            {
                var item = new ToolStripMenuItem
                {
                    Text = board.Name,
                    Tag = index
                };
                item.Click +=
                    (sender, e) => { SetParameter(parameterIndex, (int) ((ToolStripItem) sender).Tag); };
                items.Add(item);
                index++;
            }

            result.Items.AddRange(items.ToArray());
            return result;
        }

        private void BuildBoardList()
        {
            var index = 0;
            boardsMenu.DropDownItems.Clear();
            foreach (var board in Context.Boards)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = "[" + index + "] " + board.Name;
                item.Tag = index;
                item.Click += (sender, e) => { SetBoard((int) ((ToolStripItem) sender).Tag); };
                boardsMenu.DropDownItems.Add(item);
                index++;
            }
        }

        private ContextMenuStrip BuildCharacterContextMenu(int parameterIndex)
        {
            var result = new ContextMenuStrip();
            var items = new List<ToolStripMenuItem>();

            for (var i = 0; i < 256; i++)
            {
                var item = new ToolStripMenuItem
                {
                    Text = $"Char #{i} / {i:x2}h",
                    Image = _terminal.RenderSingle(i, Color),
                    ImageScaling = ToolStripItemImageScaling.None,
                    Tag = i
                };
                item.Click +=
                    (sender, e) => { SetParameter(parameterIndex, (int) ((ToolStripMenuItem) sender).Tag); };
                items.Add(item);
            }

            result.Items.AddRange(items.ToArray());
            result.Items[Actor.P1].Select();
            return result;
        }

        private static ToolStripItem BuildContextMenuItem(string text, Action click = null)
        {
            if (text == "-")
            {
                return new ToolStripSeparator();
            }
            return new ToolStripMenuItem(text, null, (sender, e) => { click?.Invoke(); });
        }

        private ContextMenuStrip BuildElementContextMenu(int menuNumber)
        {
            var result = new ContextMenuStrip();
            var index = 0;

            foreach (var element in Context.Elements)
            {
                if (element.MenuIndex == menuNumber)
                {
                    if (element.EditorCategory.Length > 0)
                    {
                        var categoryItem = new ToolStripMenuItem
                        {
                            Text = element.EditorCategory,
                            Enabled = false
                        };
                        if (result.Items.Count > 0)
                        {
                            result.Items.Add(new ToolStripSeparator());
                        }
                        result.Items.Add(categoryItem);
                    }
                    var item = new ToolStripMenuItem
                    {
                        Text = "(&" + char.ConvertFromUtf32(element.MenuKey) + ") " + element.Name,
                        Tag = index,
                        Image = _terminal.RenderSingle(element.Character, GetDefaultColor(element)),
                        ImageScaling = ToolStripItemImageScaling.None
                    };
                    item.Click +=
                        (sender, e) => { SelectElement((int) ((ToolStripMenuItem) sender).Tag); };
                    result.Items.Add(item);
                }
                index++;
            }

            if (result.Items.Count > 0)
                return result;
            return null;
        }

        private void BuildElementList()
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

        private ContextMenuStrip BuildParameterContextMenu(int parameterIndex, bool includeHeader)
        {
            var result = new ContextMenuStrip();
            var category = new ToolStripMenuItem();
            var preferredValue = 0;

            switch (parameterIndex)
            {
                case 1:
                    category.Text = SelectedElement.P1EditText;
                    preferredValue = Actor.P1;
                    break;
                case 2:
                    category.Text = SelectedElement.P2EditText;
                    preferredValue = Actor.P2;
                    break;
                case 3:
                    category.Text = SelectedElement.P3EditText;
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

            for (var i = 1; i <= 9; i++)
            {
                var item = new ToolStripMenuItem
                {
                    Text = i.ToString(),
                    Tag = i - 1
                };
                item.Click +=
                    (sender, e) => { SetParameter(parameterIndex, (int) ((ToolStripMenuItem) sender).Tag); };
                result.Items.Add(item);
            }

            if (preferredValue < result.Items.Count && preferredValue >= 0)
            {
                result.Items[preferredValue].Select();
                result.Items[preferredValue].Text += " (current)";
            }
            return result;
        }

        private ContextMenuStrip BuildStepContextMenu(bool includeHeader)
        {
            var result = new ContextMenuStrip();
            var category = new ToolStripMenuItem {Text = SelectedElement.StepEditText};

            if (string.IsNullOrWhiteSpace(category.Text))
                return null;

            category.Enabled = false;
            if (includeHeader)
            {
                result.Items.Add(category);
                result.Items.Add(new ToolStripSeparator());
            }

            for (var i = 0; i < 4; i++)
            {
                var item = new ToolStripMenuItem();
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
                item.Click += (sender, e) => { SetStep((Vector) ((ToolStripMenuItem) sender).Tag); };
                result.Items.Add(item);
            }

            result.Items[2].Select();
            return result;
        }

        private ContextMenuStrip BuildTileContextMenu(int x, int y)
        {
            var result = new ContextMenuStrip();
            var actorPresent = false;
            var tile = Context.Tiles[new Location(x + 1, y + 1)];

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
                if (!string.IsNullOrWhiteSpace(element.P1EditText))
                {
                    var submenu = GetParameterMenu(1);
                    var item = new ToolStripMenuItem {Text = element.P1EditText};
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.P2EditText))
                {
                    var submenu = GetParameterMenu(2);
                    var item = new ToolStripMenuItem {Text = element.P2EditText};
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.P3EditText))
                {
                    var submenu = GetParameterMenu(3);
                    var item = new ToolStripMenuItem {Text = element.P3EditText};
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.BoardEditText))
                {
                    var submenu = BuildBoardContextMenu(3);
                    var item = new ToolStripMenuItem {Text = element.BoardEditText};
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.StepEditText))
                {
                    var submenu = BuildStepContextMenu(false);
                    var item = new ToolStripMenuItem {Text = element.StepEditText};
                    item.DropDownItems.AddRange(submenu.Items.ToArray());
                    result.Items.Add(item);
                }
                if (!string.IsNullOrWhiteSpace(element.CodeEditText))
                {
                    result.Items.Add(BuildContextMenuItem(element.CodeEditText, ShowTileCode));
                }
                if (result.Items.Count > 0)
                {
                    result.Items.Add(BuildContextMenuItem("-"));
                    result.Items.Add(BuildContextMenuItem("Close"));
                }
            }

            return result.Items.Count > 0 ? result : null;
        }

        private void CopyCursorColor()
        {
            var tile = Context.Tiles[new Location(_terminal.CursorX + 1, _terminal.CursorY + 1)];
            Color = tile.Color;
            UpdateColor();
        }

        private void CopyCursorElement()
        {
            var tile = Context.Tiles[new Location(_terminal.CursorX + 1, _terminal.CursorY + 1)];
            {
                SelectElement(tile.Id);
            }
        }

        private void CycleAdvance()
        {
            if (Context != null)
            {
                lock (Context)
                {
                    Context.ExecuteOnce();
                }
            }
        }

        private void CycleBackgroundColor()
        {
            Color = (Color + 0x10) & 0xFF;
            UpdateColor();
        }

        private void CycleForegroundColor()
        {
            Color = (Color & 0xF0) | ((Color + 1) & 0x0F);
            UpdateColor();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            timerDaemon.Dispose();
            base.Dispose(disposing);
        }

        private void EditCode()
        {
            codeEditor.Visible = true;
            codeEditor.Actor = Actor;
        }

        private void GetCursorActor()
        {
            Actor = new Actor();

            var x = _terminal.CursorX + 1;
            var y = _terminal.CursorY + 1;
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

        private int GetDefaultColor(IElement element)
        {
            if (defaultElementPropertiesButton.Checked)
            {
                if (element.Color < 0xF0)
                {
                    return element.Color;
                }
                if (element.Color == 0xFE)
                {
                    return (((Color - 8) << 4) + 0x0F) & 0xFF;
                }
            }
            return Color;
        }

        private ContextMenuStrip GetParameterMenu(int index)
        {
            string selectedParameter;
            switch (index)
            {
                case 1:
                    selectedParameter = SelectedElement.P1EditText;
                    break;
                case 2:
                    selectedParameter = SelectedElement.P2EditText;
                    break;
                case 3:
                    selectedParameter = SelectedElement.P3EditText;
                    break;
                default:
                    return null;
            }

            switch (selectedParameter.ToLowerInvariant())
            {
                case "character?":
                    return BuildCharacterContextMenu(index);
                default:
                    return BuildParameterContextMenu(index, false);
            }
        }

        private void LoadContext()
        {
            timerDaemon.Pause();
            _terminal.Visible = true;
            Color = 0x0F;
            UpdateColor();
            SelectElement(0);
            UpdateElement();
            Actor = new Actor();
            BuildElementList();
            BuildBoardList();
            UpdateInfo();
            boardEditor.Context = Context;
            worldEditor.Context = Context;
            Context.Refresh();
            timerDaemon.Resume();
        }

        private void OnLoad()
        {
            // control events
            codeEditor.Closed += (sender, e) => { codeEditor.Visible = false; };
            elementComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (elementComboBox.SelectedIndex >= 0)
                {
                    SelectElement(((ElementItem) elementComboBox.SelectedItem).Index);
                }
            };
            editBoardButton.Click += (sender, e) => { SelectBoard(3); };
            editCodeButton.Click += (sender, e) => { EditCode(); };
            editP1Button.Click += (sender, e) => { SelectParameter(1); };
            editP2Button.Click += (sender, e) => { SelectParameter(2); };
            editP3Button.Click += (sender, e) => { SelectParameter(3); };
            editStepButton.Click += (sender, e) => { SelectStep(); };
            openToolStripMenuItem.Click += (sender, e) => { ShowOpenWorld(); };
            saveToolStripMenuItem.Click += (sender, e) => { SaveWorld(); };
            saveAsToolStripMenuItem.Click += (sender, e) => { SaveWorldAs(); };
            saveScreenshotToolStripMenuItem.Click += (sender, e) => { SaveScreenshot(); };

            scale2xButton.Click += (sender, e) =>
            {
                var scale = scale2xButton.Checked ? 2 : 1;
                _terminal.SetScale(scale, scale);
            };
            showUndefinedElementsButton.CheckedChanged += (sender, e) => { BuildElementList(); };
            statsEnabledButton.CheckedChanged += (sender, e) => { UpdateElement(); };
            testMenuButton.Click += (sender, e) => { TestWorld(); };

            // tools
            foreach (var tool in Tools)
            {
                tool.Click += (sender, e) => { SetTool(sender as ToolStripButton); };
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
            Actor = default(IActor);

            // timer settings
            timerDaemon.Start(CycleAdvance, 71.8f/9f);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var modifier = keyData & Keys.Modifiers;
            var code = keyData & Keys.KeyCode;

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
                            ShowElementContextMenu(code - Keys.F1 + 1);
                            break;
                        case Keys.F4:
                            textEnabledButton.CheckState = CheckState.Checked;
                            break;
                        case Keys.F5:
                        case Keys.F6:
                            ShowElementContextMenu(code - Keys.F1);
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
                            SelectParameter(code - Keys.D1 + 1);
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

        private void SaveScreenshot()
        {
            var result = ShowSaveScreenshot();
            if (result.Result == DialogResult.OK)
            {
                using (var bitmap = _terminal.RenderAll())
                {
                    bitmap.Save(result.FileName);
                }
            }
        }

        private void SaveWorld()
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

        private void SaveWorldAs()
        {
            if (ShowSaveWorld() == DialogResult.OK)
            {
                Context.Save(WorldFileName);
            }
        }

        private void SelectBoard(int parameterIndex)
        {
            var element = SelectedElement;

            if (!string.IsNullOrWhiteSpace(element?.BoardEditText))
            {
                var menu = BuildBoardContextMenu(parameterIndex);
                ShowParameterDropdown(parameterIndex, menu);
            }
        }

        private void SelectElement(int index)
        {
            foreach (var item in elementComboBox.Items.OfType<ElementItem>())
            {
                if (item.Index == index)
                {
                    elementComboBox.SelectedItem = item;
                }
            }

            var element = Context.Elements[index];
            statsEnabledButton.Checked = element.Cycle >= 0;
            UpdateElement();
        }

        private void SelectParameter(int index)
        {
            var menu = GetParameterMenu(index);
            if (menu != null)
            {
                ShowParameterDropdown(index, menu);
            }
        }

        private void SelectStep()
        {
        }

        private void SetBoard(int index)
        {
            // make a backup copy of the current actor
            var actor = new Actor();
            actor.CopyFrom(Actor);
            actor.Code = Actor.Code;
            Actor = actor;

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
                if (result == DialogResult.Yes)
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

        private void SetParameter(int index, int value)
        {
            switch (index)
            {
                case 1:
                    Actor.P1 = value;
                    break;
                case 2:
                    Actor.P2 = value;
                    break;
                case 3:
                    Actor.P3 = value;
                    break;
            }
        }

        private void SetStep(IXyPair vector)
        {
            Actor.Vector.CopyFrom(vector);
        }

        private void SetTool(ToolStripButton button)
        {
            foreach (var tool in Tools)
            {
                tool.Checked = tool == button;
            }
        }

        private void ShowElementContextMenu(int index)
        {
            var menu = BuildElementContextMenu(index);
            menu?.Show(tabControl, new Point(0, 0));
        }

        private void ShowOpenWorld()
        {
            var ofd = new OpenFileDialog {Filter = WorldFileFilters};
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var config = new EngineConfiguration
            {
                Disk = new DiskFileSystem(),
                EditorMode = true,
                Keyboard = keyboard,
                Speaker = speaker,
                Terminal = _terminal
            };

            Context = new Context(config, File.ReadAllBytes(ofd.FileName));
            WorldFileName = ofd.FileName;
        }

        private void ShowParameterDropdown(int index, ToolStripDropDown menu)
        {
            if (menu == null)
                return;

            switch (index)
            {
                case 1:
                    menu.Show(editP1Button, new Point(0, 0));
                    break;
                case 2:
                    menu.Show(editP2Button, new Point(0, 0));
                    break;
                case 3:
                    menu.Show(editP3Button, new Point(0, 0));
                    break;
            }
        }

        private SaveResult ShowSaveScreenshot()
        {
            if (Context == null)
                return new SaveResult { Result = DialogResult.Cancel };

            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = ScreenshotFileFilters
            };

            var result = sfd.ShowDialog();
            return new SaveResult { FileName = sfd.FileName, Result = result };
        }

        private DialogResult ShowSaveWorld()
        {
            if (Context == null)
                return DialogResult.Cancel;

            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = WorldFileFilters
            };

            if (Context.WorldData.IsLocked)
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
            if (result == DialogResult.OK)
            {
                WorldFileName = sfd.FileName;
            }
            return result;
        }

        private void ShowTileCode()
        {
            GetCursorActor();
            EditCode();
        }

        private void TerminalMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GetCursorActor();
                CopyCursorElement();
                CopyCursorColor();
                var menu = BuildTileContextMenu(_terminal.CursorX, _terminal.CursorY);
                menu?.Show((Control) sender, e.Location);
            }
        }

        private void TestWorld()
        {
            if (Context != null)
            {
                using (var mem = new MemoryStream(Context.Save()))
                {
                    var gameForm = new GameForm(mem);
                    gameForm.Show();
                }
            }
        }

        private void UpdateActors()
        {
            // we subtract 1 from the capacity because there must always be room for the messenger object
            actorInfoLabel.Text = $"{Context.Actors.Count}/{Context.ActorCapacity - 1}";
        }

        private void UpdateColor()
        {
            UpdateColorButton(foregroundColorButton, _terminal.TerminalPalette[Color & 0x0F]);
            UpdateColorButton(backgroundColorButton, _terminal.TerminalPalette[(Color >> 4) & 0x0F]);
        }

        private void UpdateColorButton(ToolStripButton button, Color backgroundColor)
        {
            button.BackColor = backgroundColor;

            // ensure foreground text is always visible by contrast
            button.ForeColor = (backgroundColor.R + backgroundColor.G + backgroundColor.B)/3 >= 0x80
                ? System.Drawing.Color.Black
                : System.Drawing.Color.White;
        }

        private void UpdateElement()
        {
            if (elementComboBox.SelectedIndex >= 0)
            {
                var element = Context.Elements[((ElementItem) elementComboBox.SelectedItem).Index];
                var editBoard = element.BoardEditText;
                var editCode = element.CodeEditText;
                var editP1 = element.P1EditText;
                var editP2 = element.P2EditText;
                var editP3 = element.P3EditText;
                var editStep = element.StepEditText;

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

        private void UpdateInfo()
        {
            Context.PackBoard();
            boardInfoLabel.Text = $"{Context.Boards[Context.WorldData.BoardIndex].Data.Length + 2}/20000";
            worldInfoLabel.Text = $"{Context.WorldSize}/360000";
            UpdateActors();
        }
    }
}