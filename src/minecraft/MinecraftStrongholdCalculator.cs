public static class MinecraftStrongHoldCalculator
{
    public static (double, double) Find(Point throw1, Point throw2)
    {
        double z = (throw2.GetLine() - throw1.GetLine()) / (throw1.slope - throw2.slope);
        double x = throw1.slope * z + throw1.GetLine();

        return (x, z);
    }
}