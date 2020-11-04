using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using TextCopy;

public class Program
{
    private static List<Point> throws = new List<Point>();
    private static Point netherPortalPoint = null;

    public static void Main(string[] args)
    {
        try
        {
            Logger.Log("Application started.");

            // Set proper window size and title
            Console.Title = "FastStronghold";
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(60, 10);
            Console.SetWindowSize(60, 10);

            // Because I'm not fully confident in these features working on every machine
            // they are caught because they are nice to have, but not essential to the program.
            try
            {
                WindowManager.SetAlwaysOnTop();
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to set the window always on top.");
                Logger.Log(ex);
            }

            try
            {
                WindowManager.DisableQuickEdit();
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to disable QuickEdit.");
                Logger.Log(ex);
            }

            // Render default text
            Text.Update();

            // Start a new thread that checks the clipboard and does the required math
            Thread clipboardDetectionThread = new Thread(ClipboardDetectionThread);
            clipboardDetectionThread.Start();

            // Check for user input and handle each key appropriately
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.R:
                        throws.Clear();
                        netherPortalPoint = null;
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
                        Text.Update();
                        break;
                    default:
                        Text.Update();
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw ex;
        }
    }

    // Thread that detects clipboard input and handles it appropriately
    private static void ClipboardDetectionThread()
    {
        // Check if we are able to get text from the clipboard, otherwise make it an empty string and save it as our starting clipboard
        string lastClipboardString = ClipboardService.GetText() == null ? String.Empty : ClipboardService.GetText();

        // Loop with a 250ms interval to check the clipboard.
        while (true)
        {
            string clipboardString = ClipboardService.GetText();

            // Check if the clipboard is set and not equal to our last clipboard (aka, it has been updated)
            if (clipboardString != null && !clipboardString.Equals(lastClipboardString))
            {
                // Check if the clipboard contains a minecraft F3+C command by checking if it starts with "/execute"
                if (clipboardString.StartsWith("/execute"))
                {
                    // Check if the command was run in the overworld or nether, and handle appropriate
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
            // Parse command into a point object with coordinates
            throws.Add(MinecraftCommandParser.PointFromCommand(command));
            Text.Clear();

            // If we have done one throw, write it out
            if (throws.Count >= 1)
            {
                Text.Write($"Throw 1: {throws[0].ToString()}");
            }

            // If we have done two throws, write out the second throw and triangulate using the two throws
            if (throws.Count >= 2)
            {
                Text.Write($"Throw 2: {throws[1].ToString()}");
                Text.Write("");

                // Find the stronghold coordinates and print them
                (double x, double z) = MinecraftStrongHoldCalculator.Find(throws[throws.Count - 2], throws[throws.Count - 1]);
                Text.Write($"Stronghold: X: {Math.Round(x)} Z: {Math.Round(z)}", ConsoleColor.Green);

                // Remove our first throw to make room for another if needed
                throws.RemoveAt(0);
            }
        }
        catch (Exception ex)
        {
            Text.Write("Detected F3+C command on clipboard but it failed to parse.", ConsoleColor.Red);
            Logger.Log(ex);

        }
    }

    private static void HandleNetherCommand(string command)
    {
        try
        {
            throws.Clear();
            Text.Clear();

            // If we don't have any coordinates set in the nether yet, register our first F3+C input as our portal location
            if (netherPortalPoint == null)
            {
                netherPortalPoint = MinecraftCommandParser.PointFromCommand(command);
                Text.Write($"Nether coordinates: {netherPortalPoint.ToString()}", ConsoleColor.Green);
            }
            else
            {
                Point currentPoint = MinecraftCommandParser.PointFromCommand(command);

                // Calculate the angle from out current location to our registered portal location
                double angle = (Math.Atan2(currentPoint.x - netherPortalPoint.x, currentPoint.z - netherPortalPoint.z));
                angle = (-(angle / Math.PI) * 360.0d) / 2.0 + 180.0;

                if (angle > 180)
                {
                    angle = -180 + (angle - 180);
                }

                // Calculate the height difference between our current height and the portal height
                int heightDifference = (int)Math.Round(netherPortalPoint.y - currentPoint.y);

                Text.Write($"Portal coordinates: {netherPortalPoint.ToString()}", ConsoleColor.Green);
                Text.Write($"Angle to portal: {Math.Round(angle, 1)}");
                Text.Write($"Portal is {Math.Abs(heightDifference)}{(heightDifference > 0 ? "▲ above" : "▼ below")} you.");
            }
        }
        catch (Exception ex)
        {
            Text.Write("Detected F3+C command on clipboard but it failed to parse.", ConsoleColor.Red);
            Logger.Log(ex);
        }
    }
}
