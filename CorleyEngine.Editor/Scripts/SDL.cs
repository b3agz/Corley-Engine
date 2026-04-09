using System;
using System.Runtime.InteropServices;

namespace CorleyEngine.Editor;

public static class SDL {

    private const string nativeLibName = "SDL2.dll"; // MonoGame handles the OS-specific extension resolution

    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint SDL_GetGlobalMouseState(out int x, out int y);

    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_MinimizeWindow(IntPtr window);

    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_MaximizeWindow(IntPtr window);

    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_RestoreWindow(IntPtr window);

}