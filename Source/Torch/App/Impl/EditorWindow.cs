using System;
using System.IO;
using System.Numerics;
using System.Text;
using Lyon.Common.Presenters;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Torch.Gui.Components;

namespace Torch.App.Impl;

public unsafe class EditorWindow(
    ITextureManager textureManager,
    IScenePresenter scenePresenter,
    IHud hud,
    ITerminal terminal,
    IBoard board,
    IWorld world,
    IWorldManager worldManager,
    IBoardList boards,
    IFacts facts,
    IWorldImporter worldImporter,
    IFadeMatrix fadeMatrix)
    : Window(Config)
{
    private readonly ITextureManager _textureManager = textureManager;
    private readonly IScenePresenter _scenePresenter = scenePresenter;
    private readonly ITerminal _terminal = terminal;
    private readonly IWorldManager _worldManager = worldManager;
    private readonly IBoardList _boards = boards;
    private readonly IFacts _facts = facts;
    private readonly IWorldImporter _worldImporter = worldImporter;
    private readonly IFadeMatrix _fadeMatrix = fadeMatrix;

    private string? _title;
    private bool _isOpen = true;
    private SDL_Texture* _texture;
    private readonly IWorld _world = world;

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
        window.Flags = _ => ImGuiWindowFlags.MenuBar |
                            ImGuiWindowFlags.HorizontalScrollbar;

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
                        new ComponentMap<IPackedBoard>(x =>
                        {
                            x.Mapper = (board, boardIndex) => new MenuItem(y =>
                            {
                                y.Label = _ => $"[{boardIndex:D3}] {board.Name}";
                                y.OnClick = _ => window.SwitchBoard(boardIndex);
                            });
                            x.Items = () => window._boards;
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

        window._terminal.SetSize(window._facts.BoardWidth, window._facts.BoardHeight, window._world.WorldType == -2);
        window._fadeMatrix.Randomize();
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
        if (index < 0 || index >= _boards.Count)
            return;

        _title = null;
        _worldManager.SetBoard(index);
        RedrawBoard();
    }

    private string BuildTitle()
    {
        var boardTitle = board.Name;
        var worldTitle = _world.Name;

        var sb = new StringBuilder();
        sb.Append(worldTitle);
        sb.Append(" - ");
        sb.Append(boardTitle);
        sb.Append("###");
        sb.Append(GetHashCode());
        return sb.ToString();
    }

    public void ClearWorld()
    {
        _worldManager.ClearWorld();
        RedrawBoard();
    }

    public void LoadWorld(int worldType, Stream stream)
    {
        _worldManager.ClearWorld();
        _worldImporter.ImportWorldData(worldType, stream);
        _worldManager.UnpackBoard(_world.BoardIndex);
        _title = null;
        RedrawBoard();
    }
}