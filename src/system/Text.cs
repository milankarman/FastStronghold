using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

// Class for rendering text to the console window
public static class Text
{
    private static string TITLE = $"FastStronghold {Constants.VERSION} by Milan Karman";
    private static string CONTROLS = "[R] Reset - [W] Reset Window - [H] Help - [C] Config";

    // List to track lines to be output on the screen
    private static List<(string text, ConsoleColor color)> lines = new List<(string text, ConsoleColor color)>();

    public static void Write(string text, ConsoleColor color = ConsoleColor.White)
    {
        lines.Add((text, color));
        Update();
    }

    public static void Clear()
    {
        lines.Clear();
        Update();
    }

    public static void Update()
    {
        Console.Clear();
        Console.WriteLine();

        // Build the window header and write it
        StringBuilder sb = new StringBuilder();

        sb.Append(new String('-', (int)((Console.BufferWidth - TITLE.Length) / 2) - 1));
        sb.Append($" {TITLE} ");
        sb.Append(new String('-', (int)((Console.BufferWidth - TITLE.Length) / 2) - 1));

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(sb.ToString());
        Console.ForegroundColor = ConsoleColor.White;

        // Default message if no other line is output
        if (lines.Count <= 0)
        {
            Console.WriteLine("Ready and awaiting clipboard...");
        }

        if (Config.WriteOutputToFile)
        {
            File.WriteAllText(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output.txt"), String.Empty);
        }

        // Fill the console with written lines and pad it with blank lines so the controls can always be at the bottom
        for (int i = 0; i < Console.BufferHeight - 4; i++)
        {
            if (i < lines.Count)
            {
                // Write current line to output file, if enabled
                if (Config.WriteOutputToFile)
                {
                    File.AppendAllText(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "output.txt"), lines[i].text + Environment.NewLine);
                }

                Console.ForegroundColor = lines[i].color;
                Console.WriteLine(lines[i].text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (i == Console.BufferHeight - 5)
            {
                Console.WriteLine(CONTROLS);
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}