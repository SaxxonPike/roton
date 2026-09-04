using System;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class KindList(
    IContextMetadataService contextMetadataService,
    IServiceProvider serviceProvider,
    IElementList elementList)
    : TypeList<IKind>(contextMetadataService, serviceProvider), IKindList
{
    public void InitializeAll()
    {
        var startId = MinId;
        var endId = MaxId;

        for (var id = startId; id <= endId; id++)
        {
            var element = elementList[id];
            element.Character = 0x20;
            element.Color = 0xFF;
            element.IsDestructible = false;
            element.IsPushable = false;
            element.IsAlwaysVisible = false;
            element.IsFloor = false;
            element.IsEditorFloor = false;
            element.HasDrawCode = false;
            element.Cycle = -1;
            element.MenuIndex = 0;
            element.MenuKey = 0;
            element.Name = "";
            element.EditorCategory = "";
            element.P1EditText = "";
            element.P2EditText = "";
            element.P3EditText = "";
            element.BoardEditText = "";
            element.StepEditText = "";
            element.CodeEditText = "";
            element.Points = 0;

            var kind = Get(id);
            kind?.Initialize(element);
        }
    }
}