using System;
using System.IO;

public static class Logger
{
    public static void Log(Exception ex)
    {
        File.AppendAllText("./log.txt", $"[{DateTime.Now}][{Constants.VERSION}] - {ex.ToString()}{Environment.NewLine}");
    }

    public static void Log(String text)
    {
        File.AppendAllText("./log.txt", $"[{DateTime.Now}][{Constants.VERSION}] - {text}{Environment.NewLine}");
    }
}