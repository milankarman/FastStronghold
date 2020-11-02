using System;
using System.Text;
using System.Collections.Generic;

public static class Text
{
    private static string title = "FastStronghold by Milan Karman";
    private static string controls = "[R] Reset Throws - [S] Reset Window Size";

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

        sb.Append(new String('-', (int)((Console.BufferWidth - title.Length) / 2) - 1));
        sb.Append($" {title} ");
        sb.Append(new String('-', (int)((Console.BufferWidth - title.Length) / 2) - 1));

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(sb.ToString());
        Console.ForegroundColor = ConsoleColor.White;

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
                Console.WriteLine(controls);
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}