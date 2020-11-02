using System;

public static class MinecraftCommandParser
{
    public static Point PointFromCommand(string input)
    {
        string[] coordsStrings = input.Replace(".", ",").Split("@s ")[1].Split(" ");

        double x = Convert.ToDouble(coordsStrings[0]);
        double y = Convert.ToDouble(coordsStrings[1]);
        double z = Convert.ToDouble(coordsStrings[2]);
        double angle = Convert.ToDouble(coordsStrings[3]);

        return new Point(x, y, z, angle);
    }
}