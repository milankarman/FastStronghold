using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using TextCopy;

public class Program
{
    private static bool verbose = true;
    private static List<Point> throws = new List<Point>();
    private static Point netherPortalPoint = null;

    public static void Main(string[] args)
    {
        Console.Title = "FastStronghold";
        Console.SetWindowSize(1, 1);
        Console.SetBufferSize(60, 10);
        Console.SetWindowSize(60, 10);
        WindowManager.SetAlwaysOnTop();
        WindowManager.DisableQuickEdit();

        Text.Update();

        Thread clipboardDetectionThread = new Thread(ClipboardDetectionThread);
        clipboardDetectionThread.Start();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.R:
                    throws.Clear();
                    Text.Clear();
                    break;
                case ConsoleKey.S:
                    Console.SetWindowSize(1, 1);
                    Console.SetBufferSize(60, 10);
                    Console.SetWindowSize(60, 10);
                    Text.Update();
                    break;
                case ConsoleKey.H:
                    Process.Start("explorer.exe", "https://github.com/milankarman/fast-stronghold#usage");
                    break;
                default:
                    break;
            }
        }
    }

    private static void ClipboardDetectionThread()
    {
        string lastClipboardString = ClipboardService.GetText() == null ? String.Empty : ClipboardService.GetText();

        while (true)
        {
            string clipboardString = ClipboardService.GetText();

            if (clipboardString != null && !clipboardString.Equals(lastClipboardString))
            {
                if (clipboardString.StartsWith("/execute"))
                {
                    if (clipboardString.Contains("minecraft:overworld"))
                    {
                        HandleOverworldCommand(clipboardString);
                    }
                    else if (clipboardString.Contains("minecraft:the_nether"))
                    {
                        HandleNetherCommand(clipboardString);
                    }
                }

                lastClipboardString = clipboardString;
            }

            Thread.Sleep(250);
        }
    }

    private static void HandleOverworldCommand(string command)
    {
        try
        {
            throws.Add(MinecraftCommandParser.PointFromCommand(command));
            Text.Clear();

            if (throws.Count >= 1)
            {
                Text.Write($"Throw 1: {throws[0].ToString()}");
            }

            if (throws.Count >= 2)
            {
                Text.Write($"Throw 2: {throws[1].ToString()}");
                Text.Write("");

                (double x, double z) = MinecraftStrongHoldCalculator.Find(throws[throws.Count - 2], throws[throws.Count - 1]);
                Text.Write($"Stronghold: X: {Math.Round(x)} Z: {Math.Round(z)}", ConsoleColor.Green);

                throws.RemoveAt(0);
            }
        }
        catch (Exception ex)
        {
            Text.Write("Detected F3+C command on clipboard but it failed to parse.", ConsoleColor.Red);

            if (verbose)
            {
                Text.Write(ex.ToString(), ConsoleColor.DarkRed);
            }
        }
    }

    private static void HandleNetherCommand(string command)
    {
        try
        {
            throws.Clear();
            Text.Clear();

            if (netherPortalPoint == null)
            {
                netherPortalPoint = MinecraftCommandParser.PointFromCommand(command);
                Text.Write($"Nether coordinates: {netherPortalPoint.ToString()}", ConsoleColor.Green);
            }
            else
            {
                Point currentPoint = MinecraftCommandParser.PointFromCommand(command);

                double angle = (Math.Atan2(currentPoint.x - netherPortalPoint.x, currentPoint.z - netherPortalPoint.z));
                angle = (-(angle / Math.PI) * 360.0d) / 2.0 + 180.0;

                if (angle > 180)
                {
                    angle = -180 + (angle - 180);
                }

                int heightDifference = (int)Math.Round(netherPortalPoint.y - currentPoint.y);

                Text.Write($"Portal coordinates: {netherPortalPoint.ToString()}", ConsoleColor.Green);
                Text.Write($"Angle to portal: {Math.Round(angle, 1)}");
                Text.Write($"Portal is {Math.Abs(heightDifference)}{(heightDifference > 0 ? "▲ above" : "▼ below")} you.");
            }
        }
        catch (Exception ex)
        {
            Text.Write("Detected F3+C command on clipboard but it failed to parse.", ConsoleColor.Red);

            if (verbose)
            {
                Text.Write(ex.ToString(), ConsoleColor.DarkRed);
            }
        }
    }
}
