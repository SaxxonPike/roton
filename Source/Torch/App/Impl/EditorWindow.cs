using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Roton.Editors;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Torch.App.Impl;

public class EditorWindow(
    Editor editor,
    IServiceScope scope,
    ITerminal terminal,
    IFacts facts)
    : IDisposable
{
    private string? _title;
    private int _currentBoard = 0;

    public void Init()
    {
        _currentBoard = editor.World?.StartBoard ?? 0;
        terminal.SetSize(facts.BoardWidth, facts.BoardHeight, false);
        _title = BuildTitle();
    }

    private string BuildTitle()
    {
        var board = editor.World!.Boards[_currentBoard];
        var sb = new StringBuilder();
        sb.Append(board.Name);
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

    public void Dispose()
    {
        scope.Dispose();
    }
}