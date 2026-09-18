using System;
using System.Numerics;
using System.Text;
using Lyon.Common.Presenters;
using Roton.Editors;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Torch.Gui.Components;

namespace Torch.App.Impl;

public unsafe class EditorWindow(
    Editor editor,
    ITextureManager textureManager,
    IScenePresenter scenePresenter,
    IHud hud,
    ITerminal terminal,
    IBoard board)
    : Window(Config)
{
    private readonly Editor _editor = editor;
    private readonly ITextureManager _textureManager = textureManager;
    private readonly IScenePresenter _scenePresenter = scenePresenter;

    private string? _title;
    private Board? _currentBoard;
    private bool _isOpen = true;
    private SDL_Texture* _texture;

    private static void Config(Window obj)
    {
        var window = (EditorWindow)obj;
        window.Label = _ =>
        {
            window._title ??= window.BuildTitle();
            return window._title;
        };
        window.IsOpen = _ => window._isOpen;
        window.OnClose = _ => window._isOpen = false;
        window.Flags = _ => ImGuiWindowFlags.MenuBar;

        window.Components.Add(
            new MenuBar(
                new Menu(
                    x => { x.Label = _ => "World"; },
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Save...";
                        x.OnClick = OnWorldSave;
                    }),
                    new Separator(),
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Close";
                        x.OnClick = _ => window.OnClose?.Invoke(window);
                    })
                ),
                new Menu(
                    x => { x.Label = _ => "Board"; },
                    new Menu(
                        x => { x.Label = _ => "Go to"; },
                        new ComponentMap<Board>(x =>
                        {
                            x.Mapper = (board, boardIndex) => new MenuItem(y =>
                            {
                                y.Label = _ => $"[{boardIndex:D3}] {board.Name}";
                                y.OnClick = _ => window.SwitchBoard(boardIndex);
                            });
                            x.Items = () => window._editor.World!.Boards;
                        })
                    ),
                    new Separator(),
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Import...";
                        x.OnClick = OnBoardImport;
                    }),
                    new MenuItem(x =>
                    {
                        x.Label = _ => "Export...";
                        x.OnClick = OnBoardExport;
                    })
                )
            )
        );

        window.Components.Add(
            new Image(x =>
            {
                Vector2 GetSize()
                {
                    if (window._texture == null)
                        return default;

                    float w, h;
                    SDL_GetTextureSize(window._texture, &w, &h);
                    return new Vector2(w, h);
                }

                x.Texture = _ =>
                {
                    var bitmap = window._scenePresenter.Render();
                    if (bitmap == null)
                        return (IntPtr)window._texture;

                    var currentSize = GetSize();
                    if (bitmap.Width != (int)currentSize.X || bitmap.Height != (int)currentSize.Y)
                    {
                        var next = window._textureManager.Create(
                            bitmap.Width,
                            bitmap.Height,
                            SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING
                        );

                        var last = window._texture;
                        window._texture = next;
                        window._textureManager.Destroy(last);
                    }

                    fixed (void* bits = bitmap.Bits)
                        window._textureManager.Update(window._texture, bits, bitmap.Stride);

                    return (IntPtr)window._texture;
                };

                x.Size = _ => GetSize();
            })
        );

        window.SwitchBoard(window._editor.World!.StartBoard);
    }

    private static void OnBoardExport(MenuItem obj)
    {
    }

    private static void OnBoardImport(MenuItem obj)
    {
    }

    private static void OnWorldSave(MenuItem obj)
    {
    }

    private void RedrawBoard()
    {
        hud.RedrawBoard();
    }

    private void SwitchBoard(int index)
    {
        if (index < 0 || index >= _editor.World!.Boards.Count)
            return;

        _currentBoard = _editor.World!.Boards[index];
        terminal.SetSize(_currentBoard.Width, _currentBoard.Height, false);
        _title = null;
        RedrawBoard();
    }

    private string BuildTitle()
    {
        var boardTitle = _currentBoard is { IsCorrupted: false } cb
            ? cb.Name
            : string.Empty;

        var worldTitle = _editor.World?.Name ?? string.Empty;

        var sb = new StringBuilder();
        sb.Append(worldTitle);
        sb.Append(" - ");
        sb.Append(boardTitle);
        sb.Append("###");
        sb.Append(_editor.World?.GetHashCode());
        return sb.ToString();
    }
}