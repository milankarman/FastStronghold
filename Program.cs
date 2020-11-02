using System;
using System.Collections.Generic;
using System.Threading;
using TextCopy;

namespace speedrun_stronghold_finder
{
    public class Program
    {
        private static bool verbose = true;
        private static List<Point> throws = new List<Point>();

        public static void Main(string[] args)
        {
            WindowManager.InitializeWindow();
            ClearConsole();

            Thread clipboardDetectionThread = new Thread(ClipboardDetectionThread);
            clipboardDetectionThread.Start();

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "restart")
                {
                    throws.Clear();
                    ClearConsole();
                }
            }

        }

        private static void ClearConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Speedrunning Stronghold Finder by Milan Karman]");
            Console.ForegroundColor = ConsoleColor.White;
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
                        try
                        {
                            throws.Add(MinecraftCommandParser.PointFromCommand(clipboardString));
                            ClearConsole();

                            if (throws.Count >= 1)
                            {
                                Console.WriteLine($"Throw 1: {throws[0].ToString()}");
                            }

                            if (throws.Count >=2)
                            {
                                Console.WriteLine($"Throw 2: {throws[1].ToString()}");
                                (double x, double z) = CalculateStrongholdLocation(throws[throws.Count - 2], throws[throws.Count - 1]);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Stronghold: X: {Math.Round(x)} Z: {Math.Round(z)}");
                                Console.ForegroundColor = ConsoleColor.White;
                                
                                throws.RemoveAt(0);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Detected F3+C command on clipboard but it failed to parse.");

                            if (verbose)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }

                    lastClipboardString = clipboardString;
                }

                Thread.Sleep(250);
            }
        }

        private static (double, double) CalculateStrongholdLocation(Point throw1, Point throw2)
        {
            double z = (throw2.GetLine() - throw1.GetLine()) / (throw1.slope - throw2.slope);
            double x = throw1.slope * z + throw1.GetLine();

            return (x, z);
        }
    }
}
