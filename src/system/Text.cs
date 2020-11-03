using System;
using System.Text;
using System.Collections.Generic;

public static class Text
{
    private const string TITLE = "FastStronghold by Milan Karman";
    private const string CONTROLS = "[R] Reset Throws - [S] Reset Window Size - [H] Help";

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

        StringBuilder sb = new StringBuilder();

        sb.Append(new String('-', (int)((Console.BufferWidth - TITLE.Length) / 2) - 1));
        sb.Append($" {TITLE} ");
        sb.Append(new String('-', (int)((Console.BufferWidth - TITLE.Length) / 2) - 1));

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(sb.ToString());
        Console.ForegroundColor = ConsoleColor.White;

        if (lines.Count <= 0)
        {
            Console.WriteLine("Ready and awaiting clipboard...");
        }

        for (int i = 0; i < Console.BufferHeight - 4; i++)
        {
            if (i < lines.Count)
            {
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