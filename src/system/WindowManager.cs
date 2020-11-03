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

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    private const uint ENABLE_QUICK_EDIT = 0x0040;
    private const int STD_INPUT_HANDLE = -10;
    private const int HWND_TOPMOST = -1;
    private const int HWND_NORMAL = -0;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int MF_BYCOMMAND = 0x00000000;

    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;

    // Sets the window to always be on top
    public static void SetAlwaysOnTop()
    {
        IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

        SetWindowPos(hWnd, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
    }

    // Disables quick edit on the console window to avoid the program being stalled by awaiting edit inputs
    public static void DisableQuickEdit()
    {
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

        uint consoleMode;
        GetConsoleMode(consoleHandle, out consoleMode);
        consoleMode &= ~ENABLE_QUICK_EDIT;
        SetConsoleMode(consoleHandle, consoleMode);
    }
}