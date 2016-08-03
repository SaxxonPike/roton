using Roton.Interface;
using Roton.Interface.Audio;
using Roton.Interface.Input;
using Roton.Interface.Synchronization;

namespace Torch
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zZTWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.superZZTWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.putFancyItemsHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.scale2xButton = new System.Windows.Forms.ToolStripMenuItem();
            this.boardsMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButton6 = new System.Windows.Forms.ToolStripDropDownButton();
            this.consolidateOOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consolidateOOPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.highImpactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeAllEmptyToColor0x00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testMenuButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton5 = new System.Windows.Forms.ToolStripDropDownButton();
            this.zZTHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.torchHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.codeEditor = new Torch.CodeEditor();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.mainPanel = new Torch.ScrollPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.refreshInfoButton = new System.Windows.Forms.ToolStripButton();
            this.boardInfoLabel = new System.Windows.Forms.ToolStripLabel();
            this.worldInfoLabel = new System.Windows.Forms.ToolStripLabel();
            this.actorInfoLabel = new System.Windows.Forms.ToolStripLabel();
            this.statsEnabledButton = new System.Windows.Forms.ToolStripButton();
            this.defaultElementPropertiesButton = new System.Windows.Forms.ToolStripButton();
            this.backgroundColorButton = new System.Windows.Forms.ToolStripButton();
            this.foregroundColorButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.elementComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.showUndefinedElementsButton = new System.Windows.Forms.ToolStripButton();
            this.textEnabledButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.drawToolButton = new System.Windows.Forms.ToolStripButton();
            this.randomToolButton = new System.Windows.Forms.ToolStripButton();
            this.inspectToolButton = new System.Windows.Forms.ToolStripButton();
            this.eraseToolButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.editTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.editCodeButton = new System.Windows.Forms.Button();
            this.editStepButton = new System.Windows.Forms.Button();
            this.editBoardButton = new System.Windows.Forms.Button();
            this.editP3Button = new System.Windows.Forms.Button();
            this.editP2Button = new System.Windows.Forms.Button();
            this.editP1Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.actorSourceLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.actorEditor = new Torch.ActorEditor();
            this.boardTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.boardEditor = new Torch.BoardEditor();
            this.worldTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.worldEditor = new Torch.WorldEditor();
            this.keyboard = new Keyboard(this.components);
            this.speaker = new Speaker(this.components);
            this.timerDaemon = new TimerDaemon(this.components);
            this.saveScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.editTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.boardTab.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.worldTab.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3,
            this.boardsMenu,
            this.toolStripDropDownButton6,
            this.testMenuButton,
            this.toolStripDropDownButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.saveScreenshotToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 22);
            this.toolStripDropDownButton1.Text = "&File";
            this.toolStripDropDownButton1.ToolTipText = " ";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zZTWorldToolStripMenuItem,
            this.superZZTWorldToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.testToolStripMenuItem.Text = "&New";
            // 
            // zZTWorldToolStripMenuItem
            // 
            this.zZTWorldToolStripMenuItem.Name = "zZTWorldToolStripMenuItem";
            this.zZTWorldToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.zZTWorldToolStripMenuItem.Text = "&ZZT World";
            // 
            // superZZTWorldToolStripMenuItem
            // 
            this.superZZTWorldToolStripMenuItem.Name = "superZZTWorldToolStripMenuItem";
            this.superZZTWorldToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.superZZTWorldToolStripMenuItem.Text = "&Super ZZT World";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.importToolStripMenuItem.Text = "&Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.putFancyItemsHereToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(56, 22);
            this.toolStripDropDownButton2.Text = "&Edit";
            // 
            // putFancyItemsHereToolStripMenuItem
            // 
            this.putFancyItemsHereToolStripMenuItem.Name = "putFancyItemsHereToolStripMenuItem";
            this.putFancyItemsHereToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.putFancyItemsHereToolStripMenuItem.Text = "(put fancy items here)";
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scale2xButton});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(61, 22);
            this.toolStripDropDownButton3.Text = "&View";
            // 
            // scale2xButton
            // 
            this.scale2xButton.CheckOnClick = true;
            this.scale2xButton.Name = "scale2xButton";
            this.scale2xButton.Size = new System.Drawing.Size(115, 22);
            this.scale2xButton.Text = "Scale &2x";
            // 
            // boardsMenu
            // 
            this.boardsMenu.Image = ((System.Drawing.Image)(resources.GetObject("boardsMenu.Image")));
            this.boardsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boardsMenu.Name = "boardsMenu";
            this.boardsMenu.Size = new System.Drawing.Size(72, 22);
            this.boardsMenu.Text = "&Boards";
            // 
            // toolStripDropDownButton6
            // 
            this.toolStripDropDownButton6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consolidateOOPToolStripMenuItem,
            this.highImpactToolStripMenuItem});
            this.toolStripDropDownButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton6.Image")));
            this.toolStripDropDownButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton6.Name = "toolStripDropDownButton6";
            this.toolStripDropDownButton6.Size = new System.Drawing.Size(84, 22);
            this.toolStripDropDownButton6.Text = "&Optimize";
            // 
            // consolidateOOPToolStripMenuItem
            // 
            this.consolidateOOPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consolidateOOPToolStripMenuItem1});
            this.consolidateOOPToolStripMenuItem.Name = "consolidateOOPToolStripMenuItem";
            this.consolidateOOPToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.consolidateOOPToolStripMenuItem.Text = "&Low Impact";
            // 
            // consolidateOOPToolStripMenuItem1
            // 
            this.consolidateOOPToolStripMenuItem1.Name = "consolidateOOPToolStripMenuItem1";
            this.consolidateOOPToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.consolidateOOPToolStripMenuItem1.Text = "&Consolidate OOP";
            // 
            // highImpactToolStripMenuItem
            // 
            this.highImpactToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeAllEmptyToColor0x00ToolStripMenuItem});
            this.highImpactToolStripMenuItem.Name = "highImpactToolStripMenuItem";
            this.highImpactToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.highImpactToolStripMenuItem.Text = "&High Impact";
            // 
            // changeAllEmptyToColor0x00ToolStripMenuItem
            // 
            this.changeAllEmptyToColor0x00ToolStripMenuItem.Name = "changeAllEmptyToColor0x00ToolStripMenuItem";
            this.changeAllEmptyToColor0x00ToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.changeAllEmptyToColor0x00ToolStripMenuItem.Text = "Change all &Empty to color 0x00";
            // 
            // testMenuButton
            // 
            this.testMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("testMenuButton.Image")));
            this.testMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.testMenuButton.Name = "testMenuButton";
            this.testMenuButton.Size = new System.Drawing.Size(48, 22);
            this.testMenuButton.Text = "&Test";
            // 
            // toolStripDropDownButton5
            // 
            this.toolStripDropDownButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zZTHelpToolStripMenuItem,
            this.torchHelpToolStripMenuItem,
            this.toolStripMenuItem3,
            this.aboutToolStripMenuItem});
            this.toolStripDropDownButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton5.Image")));
            this.toolStripDropDownButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton5.Name = "toolStripDropDownButton5";
            this.toolStripDropDownButton5.Size = new System.Drawing.Size(61, 22);
            this.toolStripDropDownButton5.Text = "&Help";
            // 
            // zZTHelpToolStripMenuItem
            // 
            this.zZTHelpToolStripMenuItem.Name = "zZTHelpToolStripMenuItem";
            this.zZTHelpToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.zZTHelpToolStripMenuItem.Text = "&ZZT Help";
            // 
            // torchHelpToolStripMenuItem
            // 
            this.torchHelpToolStripMenuItem.Name = "torchHelpToolStripMenuItem";
            this.torchHelpToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.torchHelpToolStripMenuItem.Text = "&Torch Help";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(129, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.codeEditor);
            this.mainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.mainSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(1, 0, 0, 1);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tabControl);
            this.mainSplitContainer.Panel2MinSize = 300;
            this.mainSplitContainer.Size = new System.Drawing.Size(984, 536);
            this.mainSplitContainer.SplitterDistance = 680;
            this.mainSplitContainer.TabIndex = 3;
            // 
            // codeEditor
            // 
            this.codeEditor.Actor = null;
            this.codeEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeEditor.Location = new System.Drawing.Point(100, 50);
            this.codeEditor.Name = "codeEditor";
            this.codeEditor.Size = new System.Drawing.Size(276, 176);
            this.codeEditor.TabIndex = 6;
            this.codeEditor.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.mainPanel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(679, 535);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(3, 28);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(673, 454);
            this.mainPanel.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshInfoButton,
            this.boardInfoLabel,
            this.worldInfoLabel,
            this.actorInfoLabel,
            this.statsEnabledButton,
            this.defaultElementPropertiesButton,
            this.backgroundColorButton,
            this.foregroundColorButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 510);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(679, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // refreshInfoButton
            // 
            this.refreshInfoButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshInfoButton.Image")));
            this.refreshInfoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshInfoButton.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.refreshInfoButton.Name = "refreshInfoButton";
            this.refreshInfoButton.Size = new System.Drawing.Size(66, 22);
            this.refreshInfoButton.Text = "&Refresh";
            this.refreshInfoButton.ToolTipText = "Refresh board and world info.";
            // 
            // boardInfoLabel
            // 
            this.boardInfoLabel.Image = ((System.Drawing.Image)(resources.GetObject("boardInfoLabel.Image")));
            this.boardInfoLabel.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.boardInfoLabel.Name = "boardInfoLabel";
            this.boardInfoLabel.Size = new System.Drawing.Size(64, 22);
            this.boardInfoLabel.Text = "0/20000";
            this.boardInfoLabel.ToolTipText = "Packed size of the board.";
            // 
            // worldInfoLabel
            // 
            this.worldInfoLabel.Image = ((System.Drawing.Image)(resources.GetObject("worldInfoLabel.Image")));
            this.worldInfoLabel.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.worldInfoLabel.Name = "worldInfoLabel";
            this.worldInfoLabel.Size = new System.Drawing.Size(70, 22);
            this.worldInfoLabel.Text = "0/320000";
            this.worldInfoLabel.ToolTipText = "Total size of the world.";
            // 
            // actorInfoLabel
            // 
            this.actorInfoLabel.Image = ((System.Drawing.Image)(resources.GetObject("actorInfoLabel.Image")));
            this.actorInfoLabel.Name = "actorInfoLabel";
            this.actorInfoLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.actorInfoLabel.Size = new System.Drawing.Size(52, 22);
            this.actorInfoLabel.Text = "0/151";
            this.actorInfoLabel.ToolTipText = "Actor count on the board.";
            // 
            // statsEnabledButton
            // 
            this.statsEnabledButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statsEnabledButton.Checked = true;
            this.statsEnabledButton.CheckOnClick = true;
            this.statsEnabledButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statsEnabledButton.Image = ((System.Drawing.Image)(resources.GetObject("statsEnabledButton.Image")));
            this.statsEnabledButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.statsEnabledButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.statsEnabledButton.Name = "statsEnabledButton";
            this.statsEnabledButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.statsEnabledButton.Size = new System.Drawing.Size(52, 22);
            this.statsEnabledButton.Text = "Stats";
            this.statsEnabledButton.ToolTipText = "If enabled, an actor is created when plotting.";
            // 
            // defaultElementPropertiesButton
            // 
            this.defaultElementPropertiesButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.defaultElementPropertiesButton.Checked = true;
            this.defaultElementPropertiesButton.CheckOnClick = true;
            this.defaultElementPropertiesButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultElementPropertiesButton.Image = ((System.Drawing.Image)(resources.GetObject("defaultElementPropertiesButton.Image")));
            this.defaultElementPropertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.defaultElementPropertiesButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.defaultElementPropertiesButton.Name = "defaultElementPropertiesButton";
            this.defaultElementPropertiesButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.defaultElementPropertiesButton.Size = new System.Drawing.Size(65, 22);
            this.defaultElementPropertiesButton.Text = "Default";
            this.defaultElementPropertiesButton.ToolTipText = "If enabled, default color will be forced when plotting.";
            // 
            // backgroundColorButton
            // 
            this.backgroundColorButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.backgroundColorButton.BackColor = System.Drawing.Color.Black;
            this.backgroundColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backgroundColorButton.ForeColor = System.Drawing.Color.White;
            this.backgroundColorButton.Image = ((System.Drawing.Image)(resources.GetObject("backgroundColorButton.Image")));
            this.backgroundColorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backgroundColorButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.backgroundColorButton.Name = "backgroundColorButton";
            this.backgroundColorButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.backgroundColorButton.Size = new System.Drawing.Size(23, 22);
            this.backgroundColorButton.Text = "B";
            this.backgroundColorButton.ToolTipText = "Background color.";
            // 
            // foregroundColorButton
            // 
            this.foregroundColorButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.foregroundColorButton.BackColor = System.Drawing.Color.Black;
            this.foregroundColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.foregroundColorButton.ForeColor = System.Drawing.Color.White;
            this.foregroundColorButton.Image = ((System.Drawing.Image)(resources.GetObject("foregroundColorButton.Image")));
            this.foregroundColorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.foregroundColorButton.Margin = new System.Windows.Forms.Padding(0, 1, 1, 2);
            this.foregroundColorButton.Name = "foregroundColorButton";
            this.foregroundColorButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.foregroundColorButton.Size = new System.Drawing.Size(23, 22);
            this.foregroundColorButton.Text = "F";
            this.foregroundColorButton.ToolTipText = "Foreground color.";
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.elementComboBox,
            this.showUndefinedElementsButton,
            this.textEnabledButton});
            this.toolStrip3.Location = new System.Drawing.Point(0, 485);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip3.Size = new System.Drawing.Size(679, 25);
            this.toolStrip3.TabIndex = 2;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // elementComboBox
            // 
            this.elementComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.elementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.elementComboBox.DropDownWidth = 200;
            this.elementComboBox.Margin = new System.Windows.Forms.Padding(1, 0, 4, 0);
            this.elementComboBox.Name = "elementComboBox";
            this.elementComboBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.elementComboBox.Size = new System.Drawing.Size(200, 25);
            this.elementComboBox.Sorted = true;
            this.elementComboBox.ToolTipText = "Change the currently selected element.";
            // 
            // showUndefinedElementsButton
            // 
            this.showUndefinedElementsButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.showUndefinedElementsButton.CheckOnClick = true;
            this.showUndefinedElementsButton.Image = ((System.Drawing.Image)(resources.GetObject("showUndefinedElementsButton.Image")));
            this.showUndefinedElementsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showUndefinedElementsButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.showUndefinedElementsButton.Name = "showUndefinedElementsButton";
            this.showUndefinedElementsButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.showUndefinedElementsButton.Size = new System.Drawing.Size(41, 22);
            this.showUndefinedElementsButton.Text = "All";
            this.showUndefinedElementsButton.ToolTipText = "Toggle undefined elements.";
            // 
            // textEnabledButton
            // 
            this.textEnabledButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.textEnabledButton.CheckOnClick = true;
            this.textEnabledButton.Image = ((System.Drawing.Image)(resources.GetObject("textEnabledButton.Image")));
            this.textEnabledButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.textEnabledButton.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.textEnabledButton.Name = "textEnabledButton";
            this.textEnabledButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.textEnabledButton.Size = new System.Drawing.Size(48, 22);
            this.textEnabledButton.Text = "Text";
            this.textEnabledButton.ToolTipText = "If enabled, typed text will be written to the board.";
            // 
            // toolStrip4
            // 
            this.toolStrip4.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawToolButton,
            this.randomToolButton,
            this.inspectToolButton,
            this.eraseToolButton});
            this.toolStrip4.Location = new System.Drawing.Point(0, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip4.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip4.Size = new System.Drawing.Size(679, 25);
            this.toolStrip4.TabIndex = 3;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // drawToolButton
            // 
            this.drawToolButton.Image = ((System.Drawing.Image)(resources.GetObject("drawToolButton.Image")));
            this.drawToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.drawToolButton.Name = "drawToolButton";
            this.drawToolButton.Size = new System.Drawing.Size(54, 22);
            this.drawToolButton.Text = "Draw";
            this.drawToolButton.ToolTipText = "Plot tiles.";
            // 
            // randomToolButton
            // 
            this.randomToolButton.Image = ((System.Drawing.Image)(resources.GetObject("randomToolButton.Image")));
            this.randomToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.randomToolButton.Name = "randomToolButton";
            this.randomToolButton.Size = new System.Drawing.Size(72, 22);
            this.randomToolButton.Text = "Random";
            this.randomToolButton.ToolTipText = "Plot random tiles from the buffer.";
            // 
            // inspectToolButton
            // 
            this.inspectToolButton.Image = ((System.Drawing.Image)(resources.GetObject("inspectToolButton.Image")));
            this.inspectToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.inspectToolButton.Name = "inspectToolButton";
            this.inspectToolButton.Size = new System.Drawing.Size(65, 22);
            this.inspectToolButton.Text = "Inspect";
            this.inspectToolButton.ToolTipText = "Edit tile properties.";
            // 
            // eraseToolButton
            // 
            this.eraseToolButton.Image = ((System.Drawing.Image)(resources.GetObject("eraseToolButton.Image")));
            this.eraseToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eraseToolButton.Name = "eraseToolButton";
            this.eraseToolButton.Size = new System.Drawing.Size(54, 22);
            this.eraseToolButton.Text = "Erase";
            this.eraseToolButton.ToolTipText = "Erase tiles.";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.editTab);
            this.tabControl.Controls.Add(this.boardTab);
            this.tabControl.Controls.Add(this.worldTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(300, 536);
            this.tabControl.TabIndex = 0;
            // 
            // editTab
            // 
            this.editTab.Controls.Add(this.tableLayoutPanel1);
            this.editTab.Location = new System.Drawing.Point(4, 22);
            this.editTab.Name = "editTab";
            this.editTab.Size = new System.Drawing.Size(292, 510);
            this.editTab.TabIndex = 0;
            this.editTab.Text = "Edit";
            this.editTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.editCodeButton, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.editStepButton, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.editBoardButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.editP3Button, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.editP2Button, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.editP1Button, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 510);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // editCodeButton
            // 
            this.editCodeButton.AutoSize = true;
            this.editCodeButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.editCodeButton.Location = new System.Drawing.Point(3, 484);
            this.editCodeButton.Name = "editCodeButton";
            this.editCodeButton.Size = new System.Drawing.Size(286, 23);
            this.editCodeButton.TabIndex = 7;
            this.editCodeButton.Text = "(Code)";
            this.editCodeButton.UseVisualStyleBackColor = true;
            this.editCodeButton.Visible = false;
            // 
            // editStepButton
            // 
            this.editStepButton.AutoSize = true;
            this.editStepButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.editStepButton.Location = new System.Drawing.Point(3, 455);
            this.editStepButton.Name = "editStepButton";
            this.editStepButton.Size = new System.Drawing.Size(286, 23);
            this.editStepButton.TabIndex = 6;
            this.editStepButton.Text = "(Step)";
            this.editStepButton.UseVisualStyleBackColor = true;
            this.editStepButton.Visible = false;
            // 
            // editBoardButton
            // 
            this.editBoardButton.AutoSize = true;
            this.editBoardButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.editBoardButton.Location = new System.Drawing.Point(3, 426);
            this.editBoardButton.Name = "editBoardButton";
            this.editBoardButton.Size = new System.Drawing.Size(286, 23);
            this.editBoardButton.TabIndex = 5;
            this.editBoardButton.Text = "(Board)";
            this.editBoardButton.UseVisualStyleBackColor = true;
            this.editBoardButton.Visible = false;
            // 
            // editP3Button
            // 
            this.editP3Button.AutoSize = true;
            this.editP3Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.editP3Button.Location = new System.Drawing.Point(3, 397);
            this.editP3Button.Name = "editP3Button";
            this.editP3Button.Size = new System.Drawing.Size(286, 23);
            this.editP3Button.TabIndex = 4;
            this.editP3Button.Text = "(Parameter 3)";
            this.editP3Button.UseVisualStyleBackColor = true;
            this.editP3Button.Visible = false;
            // 
            // editP2Button
            // 
            this.editP2Button.AutoSize = true;
            this.editP2Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.editP2Button.Location = new System.Drawing.Point(3, 368);
            this.editP2Button.Name = "editP2Button";
            this.editP2Button.Size = new System.Drawing.Size(286, 23);
            this.editP2Button.TabIndex = 3;
            this.editP2Button.Text = "(Parameter 2)";
            this.editP2Button.UseVisualStyleBackColor = true;
            this.editP2Button.Visible = false;
            // 
            // editP1Button
            // 
            this.editP1Button.AutoSize = true;
            this.editP1Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.editP1Button.Location = new System.Drawing.Point(3, 339);
            this.editP1Button.Name = "editP1Button";
            this.editP1Button.Size = new System.Drawing.Size(286, 23);
            this.editP1Button.TabIndex = 2;
            this.editP1Button.Text = "(Parameter 1)";
            this.editP1Button.UseVisualStyleBackColor = true;
            this.editP1Button.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 330);
            this.panel1.TabIndex = 8;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.actorSourceLabel, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(284, 328);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // actorSourceLabel
            // 
            this.actorSourceLabel.AutoSize = true;
            this.actorSourceLabel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.actorSourceLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.actorSourceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actorSourceLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.actorSourceLabel.Location = new System.Drawing.Point(0, 0);
            this.actorSourceLabel.Margin = new System.Windows.Forms.Padding(0);
            this.actorSourceLabel.Name = "actorSourceLabel";
            this.actorSourceLabel.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.actorSourceLabel.Size = new System.Drawing.Size(284, 22);
            this.actorSourceLabel.TabIndex = 0;
            this.actorSourceLabel.Text = "(actor source)";
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.Controls.Add(this.actorEditor);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 22);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(284, 306);
            this.panel4.TabIndex = 1;
            // 
            // actorEditor
            // 
            this.actorEditor.Actor = null;
            this.actorEditor.AutoSize = true;
            this.actorEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.actorEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.actorEditor.Location = new System.Drawing.Point(0, 0);
            this.actorEditor.Margin = new System.Windows.Forms.Padding(0);
            this.actorEditor.Name = "actorEditor";
            this.actorEditor.Size = new System.Drawing.Size(284, 278);
            this.actorEditor.TabIndex = 3;
            // 
            // boardTab
            // 
            this.boardTab.Controls.Add(this.tableLayoutPanel3);
            this.boardTab.Location = new System.Drawing.Point(4, 22);
            this.boardTab.Name = "boardTab";
            this.boardTab.Size = new System.Drawing.Size(292, 510);
            this.boardTab.TabIndex = 1;
            this.boardTab.Text = "Board";
            this.boardTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(292, 510);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.boardEditor);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(286, 504);
            this.panel2.TabIndex = 0;
            // 
            // boardEditor
            // 
            this.boardEditor.AutoSize = true;
            this.boardEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.boardEditor.Context = null;
            this.boardEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.boardEditor.Location = new System.Drawing.Point(0, 0);
            this.boardEditor.Name = "boardEditor";
            this.boardEditor.Size = new System.Drawing.Size(284, 367);
            this.boardEditor.TabIndex = 0;
            // 
            // worldTab
            // 
            this.worldTab.Controls.Add(this.tableLayoutPanel4);
            this.worldTab.Location = new System.Drawing.Point(4, 22);
            this.worldTab.Name = "worldTab";
            this.worldTab.Size = new System.Drawing.Size(292, 510);
            this.worldTab.TabIndex = 2;
            this.worldTab.Text = "World";
            this.worldTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(292, 510);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.worldEditor);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(286, 504);
            this.panel3.TabIndex = 0;
            // 
            // worldEditor
            // 
            this.worldEditor.AutoSize = true;
            this.worldEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.worldEditor.Context = null;
            this.worldEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.worldEditor.Location = new System.Drawing.Point(0, 0);
            this.worldEditor.Name = "worldEditor";
            this.worldEditor.Size = new System.Drawing.Size(267, 507);
            this.worldEditor.TabIndex = 0;
            // 
            // timerDaemon
            // 
            this.timerDaemon.Paused = false;
            // 
            // saveScreenshotToolStripMenuItem
            // 
            this.saveScreenshotToolStripMenuItem.Name = "saveScreenshotToolStripMenuItem";
            this.saveScreenshotToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveScreenshotToolStripMenuItem.Text = "Save Screens&hot";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Name = "Editor";
            this.Text = "Torch";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.editTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.boardTab.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.worldTab.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage editTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage boardTab;
        private System.Windows.Forms.TabPage worldTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Torch.ScrollPanel mainPanel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripMenuItem zZTWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem superZZTWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem putFancyItemsHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem scale2xButton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton5;
        private System.Windows.Forms.ToolStripMenuItem zZTHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem torchHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton6;
        private System.Windows.Forms.ToolStripMenuItem consolidateOOPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consolidateOOPToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem highImpactToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeAllEmptyToColor0x00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton boardsMenu;
        private System.Windows.Forms.Button editCodeButton;
        private System.Windows.Forms.Button editStepButton;
        private System.Windows.Forms.Button editBoardButton;
        private System.Windows.Forms.Button editP3Button;
        private System.Windows.Forms.Button editP2Button;
        private System.Windows.Forms.Button editP1Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ToolStripLabel boardInfoLabel;
        private System.Windows.Forms.ToolStripLabel worldInfoLabel;
        private System.Windows.Forms.ToolStripLabel actorInfoLabel;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton refreshInfoButton;
        private System.Windows.Forms.ToolStripComboBox elementComboBox;
        private Keyboard keyboard;
        private Speaker speaker;
        private System.Windows.Forms.ToolStripButton showUndefinedElementsButton;
        private System.Windows.Forms.ToolStripButton statsEnabledButton;
        private System.Windows.Forms.ToolStripButton defaultElementPropertiesButton;
        private System.Windows.Forms.ToolStripButton backgroundColorButton;
        private System.Windows.Forms.ToolStripButton foregroundColorButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private BoardEditor boardEditor;
        private WorldEditor worldEditor;
        private CodeEditor codeEditor;
        private System.Windows.Forms.ToolStripButton textEnabledButton;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton drawToolButton;
        private System.Windows.Forms.ToolStripButton inspectToolButton;
        private System.Windows.Forms.ToolStripButton eraseToolButton;
        private System.Windows.Forms.ToolStripButton randomToolButton;
        private System.Windows.Forms.ToolStripButton testMenuButton;
        private TimerDaemon timerDaemon;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label actorSourceLabel;
        private System.Windows.Forms.Panel panel4;
        private ActorEditor actorEditor;
        private System.Windows.Forms.ToolStripMenuItem saveScreenshotToolStripMenuItem;
    }
}

