using System;
using System.Collections.Generic;
using System.Threading;
using TextCopy;

namespace speedrun_stronghold_finder
{
    public class Program
    {
        private static bool verbose = true;
        private static List<Coordinates> throws = new List<Coordinates>();

        public class Coordinates
        {
            public double x;
            public double y;
            public double z;

            public double angle;
            public double slope;

            public Coordinates(double x, double y, double z, double angle)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.angle = angle;
                this.slope = Math.Tan(-angle * Math.PI / 180);
            }

            public double GetLine()
            {
                return x - slope * z;
            }

            public override string ToString()
            {
                return $"X: {x} Y: {y} Z: {z} Angle: {angle}";
            }
        }

        public static void Main(string[] args)
        {
            string lastClipboardString = ClipboardService.GetText() == null ? String.Empty : ClipboardService.GetText();

            Console.WriteLine($"Speedrunning Stronghold Finder Pre-alpha version by Milan Karman");
            Console.WriteLine($"Awaiting clipboard...");

            while (true)
            {
                string clipboardString = ClipboardService.GetText();

                if (clipboardString != null && !clipboardString.Equals(lastClipboardString))
                {
                    if (clipboardString.StartsWith("/execute"))
                    {
                        try
                        {
                            Coordinates coordinates = CoordinatesFromCommand(clipboardString);
                            throws.Add(coordinates);

                            Console.WriteLine($"Throw coordinates: {coordinates.ToString()}");

                            if (throws.Count >= 2)
                            {
                                CalculateStrongholdLocation(throws[throws.Count - 2], throws[throws.Count - 1]);
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

        private static Coordinates CoordinatesFromCommand(string input)
        {
            string[] coordsStrings = input.Replace(".", ",").Split("@s ")[1].Split(" ");

            double x = Convert.ToDouble(coordsStrings[0]);
            double y = Convert.ToDouble(coordsStrings[1]);
            double z = Convert.ToDouble(coordsStrings[2]);
            double angle = Convert.ToDouble(coordsStrings[3]);

            return new Coordinates(x, y, z, angle);
        }

        private static void CalculateStrongholdLocation(Coordinates throw1, Coordinates throw2)
        {
            double z = (throw2.GetLine() - throw1.GetLine()) / (throw1.slope - throw2.slope);
            double x = throw1.slope * z + throw1.GetLine();

            Console.WriteLine($"Stronghold coordinates: X: {x} Z: {z}");
        }
    }
}
