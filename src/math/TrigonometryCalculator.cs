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

    // Gets the angle from one point to the next
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

    // Gets the distance from one point to another
    public static double GetDistanceBetweenPoints(Point a, Point b)
    {
        double xDistance = a.x - b.x;
        double yDistance = a.y - b.y;
        double zDistance = a.z - b.z;

        double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2) + Math.Pow(zDistance, 2));

        return distance;
    }

    // Takes a point and angle and returns the coordinates of where it intersects with a circle of the given radius
    // This formula is thanks to Sharpieman20 (https://github.com/Sharpieman20/Sharpies-Speedrunning-Tools)
    public static (double, double) GetLineIntersectionOnCircle(Point point, double radius)
    {
        double x = point.x;
        double z = point.z;
        double angle = point.angle;

        if (angle < 0)
        {
            angle += 360;
        }

        angle -= 180;

        double d = 90 - angle;

        double x1 = x / 8;
        double z1 = z / 8;
        double r = d * (Math.PI / 180);

        double m1 = -1 * Math.Tan(r);
        double a = 1 + (m1 * m1);
        double b1 = -1 * m1 * x1 + z1;
        double b = 2 * m1 * b1;
        double co = b1 * b1 - radius * radius;

        double xp = ((-1 * b) + (Math.Sign(angle) * Math.Sqrt(b * b - 4 * a * co))) / (2 * a);
        double zp = m1 * xp + b1;

        return (xp, zp);
    }

    public static (double, double) FindClosestPointInCircle(Point point, double radius)
    {
        double magnitude = Math.Sqrt(point.x * point.x + point.z * point.z);
        double x = point.x / magnitude * radius;
        double z = point.z / magnitude * radius;

        return (x, z);
    }
}