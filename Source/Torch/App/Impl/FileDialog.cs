using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Torch.App.Impl;

public static class FileDialog
{
    private class Context
    {
        public Action<string[]>? Callback { get; set; }
        public SDL_DialogFileFilter[]? Filters { get; set; }
        public GCHandle FiltersHandle { get; set; }
    }

    private static readonly Dictionary<IntPtr, Context> Contexts = new();

    public static unsafe void Show(
        SDL_Window* window,
        (string Name, string Pattern)[] filters,
        bool allowMany,
        Action<string[]> callback)
    {
        var userData = (IntPtr)Random.Shared.Next();
        var filterArray = new SDL_DialogFileFilter[filters.Length];

        for (var i = 0; i < filters.Length; i++)
            filterArray[i] = new SDL_DialogFileFilter
            {
                name = (byte*)Marshal.StringToCoTaskMemUTF8($"{filters[i].Name}\0"),
                pattern = (byte*)Marshal.StringToCoTaskMemUTF8($"{filters[i].Pattern}\0")
            };

        var filtersHandle = GCHandle.Alloc(filterArray, GCHandleType.Pinned);

        Contexts.Add(userData, new Context
        {
            Callback = callback,
            Filters = filterArray,
            FiltersHandle = filtersHandle
        });

        SDL_ShowOpenFileDialog(
            &SdlCallback,
            userData,
            window,
            (SDL_DialogFileFilter*)filtersHandle.AddrOfPinnedObject(),
            filterArray.Length,
            (byte*)null,
            allowMany
        );
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void SdlCallback(IntPtr userData, byte** files, int selectedFilter)
    {
        if (!Contexts.Remove(userData, out var context))
            return;

        if (files == null)
            return;

        var count = 0;
        while (files[count] != null)
            count++;

        var result = new string[count];
        for (var i = 0; i < count; i++)
            result[i] = Marshal.PtrToStringUTF8((IntPtr)files[i])!;

        context.Callback?.Invoke(result);
        context.Filters = null;
        context.FiltersHandle.Free();
    }
}