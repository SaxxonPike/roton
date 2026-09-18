using System.Text;
using Roton.Editors;
using Roton.Emulation.Core;

namespace Torch.App.Impl;

public class EditorWindow(Editor editor, ITerminal terminal)
{
    private string? _title;
    private Board? _currentBoard;

    public void Init()
    {
        SwitchBoard(editor.World!.StartBoard);
        _title = BuildTitle();
    }

    private void SwitchBoard(int index)
    {
        if (index < 0 || index >= editor.World!.Boards.Count)
            return;

        _currentBoard = editor.World!.Boards[index];
        terminal.SetSize(_currentBoard.Width, _currentBoard.Height, false);
    }

    private string BuildTitle()
    {
        var boardTitle = _currentBoard is { IsCorrupted: false } cb
            ? cb.Name
            : string.Empty;
        
        var sb = new StringBuilder();
        sb.Append(boardTitle);
        sb.Append("##");
        sb.Append(editor.World?.GetHashCode());
        return sb.ToString();
    }

    public unsafe bool Render()
    {
        var isOpen = true;

        if (ImGui.Begin(_title, ref isOpen, ImGuiWindowFlags.MenuBar))
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("World"))
                {
                    if (ImGui.MenuItem("Save..."))
                    {
                    }

                    ImGui.Separator();

                    if (ImGui.MenuItem("Close"))
                    {
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Board"))
                {
                    if (ImGui.MenuItem("Import..."))
                    {
                    }

                    if (ImGui.MenuItem("Export..."))
                    {
                    }

                    ImGui.SeparatorText("Boards");

                    foreach (var board in editor.World?.Boards ?? [])
                    {
                        if (ImGui.MenuItem(board.Name))
                        {
                        }
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        ImGui.End();

        return isOpen;
    }
}