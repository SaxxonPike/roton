using System;
using System.Collections.Generic;
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
    private readonly Dictionary<int, string> _friendlyNames = new();

    public void InitializeAll()
    {
        _friendlyNames.Clear();

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
            element.Name = string.Empty;
            element.EditorCategory = string.Empty;
            element.P1EditText = string.Empty;
            element.P2EditText = string.Empty;
            element.P3EditText = string.Empty;
            element.BoardEditText = string.Empty;
            element.StepEditText = string.Empty;
            element.CodeEditText = string.Empty;
            element.Points = 0;

            var kind = Get(id);
            kind?.Initialize(element);

            if (kind?.FriendlyName is { } friendlyName)
                _friendlyNames.Add(id, friendlyName);
        }
    }

    public string? GetFriendlyName(int index) =>
        _friendlyNames.TryGetValue(index, out var friendlyName)
            ? friendlyName
            : null;
}