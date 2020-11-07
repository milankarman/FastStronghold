using System;
using System.IO;

public static class Logger
{
    // Write exception to log file
    public static void Log(Exception ex)
    {
        File.AppendAllText(Path.Join(Environment.CurrentDirectory, "log.txt"), $"[{DateTime.Now}][{Constants.VERSION}] - {ex.ToString()}{Environment.NewLine}");
    }

    // Write handwritten string to log file
    public static void Log(String text)
    {
        File.AppendAllText(Path.Join(Environment.CurrentDirectory, "log.txt"), $"[{DateTime.Now}][{Constants.VERSION}] - {text}{Environment.NewLine}");
    }
}