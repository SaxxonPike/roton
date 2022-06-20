using System;
using Roton.Composers.Video.Palettes.Impl;
using Roton.Composers.Video.Scenes.Impl;
using Roton.Emulation.Core;

namespace Roton.Composers.Video.Scenes;

public interface ISceneComposer : ITerminal
{
    event EventHandler<FontDataChangedEventArgs> FontDataChanged;
    event EventHandler<PaletteDataChangedEventArgs> PaletteDataChanged; 
    event EventHandler<ResizedEventArgs> Resized;
    event EventHandler<SceneUpdatedEventArgs> SceneUpdated;
        
    int Rows { get; }
    void Update(int x, int y);
    int Columns { get; }
        
    IBitmap Bitmap { get; }
}