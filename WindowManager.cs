using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class WindowManager
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    private const int HWND_TOPMOST = -1;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int MF_BYCOMMAND = 0x00000000;

    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;

    public static void InitializeWindow()
    {
        Console.Title = "Speedrunning Stronghold Finder";
        WindowManager.AlwaysOnTop();
        Console.SetWindowSize(60, 10);
        Console.SetBufferSize(60, 10);
    }

    private static void AlwaysOnTop()
    {
        IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

        SetWindowPos(hWnd, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
    }
}