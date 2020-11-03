public static class MinecraftStrongHoldCalculator
{
    // Calculate the location of the stronghold using two points with angles
    public static (double, double) Find(Point throw1, Point throw2)
    {
        double z = (throw2.GetLine() - throw1.GetLine()) / (throw1.slope - throw2.slope);
        double x = throw1.slope * z + throw1.GetLine();

        return (x, z);
    }
}