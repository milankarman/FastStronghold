using System;

// Class to keep track of points and angles, with helper methods to do math with them.
public class Point
{
    public double x;
    public double y;
    public double z;

    public double angle;
    public double slope;

    public Point(double x, double y, double z, double angle)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        // Normalize minecraft angle (it keeps going after 360 by default)
        this.angle = angle % 360;

        this.slope = Math.Tan(-angle * Math.PI / 180);
    }

    public double GetLine()
    {
        return x - slope * z;
    }

    // Get the angle as it is displayed in game (for navigation purposes)
    private double GetInGameAngle()
    {
        if (angle > 180)
        {
            return -180 + (angle - 180);
        }

        return angle;
    }

    public override string ToString()
    {
        return $"X:{Math.Round(x)} Y:{Math.Round(y)} Z:{Math.Round(z)} | Angle:{Math.Round(GetInGameAngle(), 1)}";
    }
}