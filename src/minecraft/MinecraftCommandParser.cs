using System;
using System.Globalization;

public static class MinecraftCommandParser
{
    // Parses a F3+C command and turns it into a Point class instance
    public static Point PointFromF3C(string input)
    {
        string[] coordsStrings = input.Split("@s ")[1].Split(" ");

        double x = Convert.ToDouble(coordsStrings[0], CultureInfo.InvariantCulture);
        double y = Convert.ToDouble(coordsStrings[1], CultureInfo.InvariantCulture);
        double z = Convert.ToDouble(coordsStrings[2], CultureInfo.InvariantCulture);
        double angle = Convert.ToDouble(coordsStrings[3], CultureInfo.InvariantCulture);

        return new Point(x, y, z, angle);
    }
}