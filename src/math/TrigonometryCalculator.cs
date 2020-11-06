using System;

public static class TrigonometryCalculator
{
    // Calculate the location of the stronghold using two points with angles
    public static (double, double) GetPointIntersection(Point a, Point b)
    {
        double z = (b.GetLine() - a.GetLine()) / (a.slope - b.slope);
        double x = a.slope * z + a.GetLine();

        return (x, z);
    }

    public static double GetAngleAToB(Point a, Point b)
    {
        double angle = (Math.Atan2(a.x - b.x, a.z - b.z));
        angle = (-(angle / Math.PI) * 360.0d) / 2.0 + 180.0;

        if (angle > 180)
        {
            angle = -180 + (angle - 180);
        }

        return angle;
    }

    public static double GetDistanceBetweenPoints(Point a, Point b)
    {
        double xDistance = a.x - b.x;
        double yDistance = a.y - b.y;
        double zDistance = a.z - b.z;

        double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2) + Math.Pow(zDistance, 2));

        return distance;
    }
}