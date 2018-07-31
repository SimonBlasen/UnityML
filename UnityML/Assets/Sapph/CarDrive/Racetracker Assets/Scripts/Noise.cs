using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellNoise
{
    private static double SQRT_2 = 1.4142135623730950488;
    private static double SQRT_3 = 1.7320508075688772935;

    private bool useDistance = false;

    private long seed;
    private short distanceMethod;

    public CellNoise(long seed, short distanceMethod)
    {
        this.seed = seed;
        this.distanceMethod = distanceMethod;
    }

    private double distance(double xDist, double zDist)
    {
        return Math.Sqrt(xDist * xDist + zDist * zDist);
    }

    private double getDistance2D(double xDist, double zDist)
    {
        switch (distanceMethod)
        {
            case 0:
                return Math.Sqrt(xDist * xDist + zDist * zDist) / SQRT_2;
            case 1:
                return xDist + zDist;
            default:
                return Double.NaN;
        }
    }

    private double getDistance(double xDist, double yDist, double zDist)
    {
        switch (distanceMethod)
        {
            case 0:
                return Math.Sqrt(xDist * xDist + yDist * yDist + zDist * zDist) / SQRT_3; //Approximation (for speed) of elucidean (regular) distance
            case 1:
                return xDist + yDist + zDist;
            default:
                return Double.NaN;
        }
    }

    public bool isUseDistance()
    {
        return useDistance;
    }

    public void setUseDistance(bool useDistance)
    {
        this.useDistance = useDistance;
    }

    public short getDistanceMethod()
    {
        return distanceMethod;
    }

    public long getSeed()
    {
        return seed;
    }

    public void setDistanceMethod(short distanceMethod)
    {
        this.distanceMethod = distanceMethod;
    }

    public void setSeed(long seed)
    {
        this.seed = seed;
    }

    public float noise(double x, double z, double frequency)
    {
        x *= frequency;
        z *= frequency;

        int xInt = (x > .0 ? (int)x : (int)x - 1);
        int zInt = (z > .0 ? (int)z : (int)z - 1);

        double minDist = 32000000.0;

        double xCandidate = 0;
        double zCandidate = 0;

        for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++)
        {
            for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
            {
                double xPos = xCur + valueNoise2D(xCur, zCur, seed);
                double zPos = zCur + valueNoise2D(xCur, zCur, seed - 50);
                double xDist = xPos - x;
                double zDist = zPos - z;
                double dist = xDist * xDist + zDist * zDist;

                if (dist < minDist)
                {
                    minDist = dist;
                    xCandidate = xPos;
                    zCandidate = zPos;
                }
            }
        }

        if (useDistance)
        {
            double xDist = xCandidate - x;
            double zDist = zCandidate - z;
            return (float)getDistance2D(xDist, zDist);
        }

        else
            return ((float)CellNoise.valueNoise2D(
              (int)(Math.Round(xCandidate - 0.5)),
              (int)(Math.Round(zCandidate - 0.5)), seed));
    }

    public float border2(double x, double z, double width, float depth)
    {
        x *= 1D;
        z *= 1D;

        int xInt = (x > .0 ? (int)x : (int)x - 1);
        int zInt = (z > .0 ? (int)z : (int)z - 1);

        double dCandidate = 32000000.0;
        double xCandidate = 0;
        double zCandidate = 0;

        double dNeighbour = 32000000.0;
        double xNeighbour = 0;
        double zNeighbour = 0;

        double xPos, zPos, xDist, zDist, dist;
        for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++)
        {
            for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
            {
                xPos = xCur + valueNoise2D(xCur, zCur, seed);
                zPos = zCur + valueNoise2D(xCur, zCur, seed - 55);
                xDist = xPos - x;
                zDist = zPos - z;
                dist = distance(xPos - x, zPos - z);

                if (dist < dCandidate)
                {
                    dNeighbour = dCandidate;
                    xNeighbour = xCandidate;
                    zNeighbour = zCandidate;

                    dCandidate = dist;
                    xCandidate = xPos;
                    zCandidate = zPos;
                }
                else if (dist > dCandidate && dist < dNeighbour)
                {
                    dNeighbour = dist;
                    xNeighbour = xPos;
                    zNeighbour = zPos;
                }
            }
        }

        double diff = distance(xCandidate - xNeighbour, zCandidate - zNeighbour);
        double total = (dCandidate + dNeighbour) / diff;

        dCandidate = dCandidate / total;
        dNeighbour = dNeighbour / total;

        double c = (diff / 2D) - dCandidate;
        if (c < width)
        {
            return (((float)(c / width)) - 1f) * depth;
        }
        else
        {
            return 0f;
        }
    }

    public float border(double x, double z, double width, float depth)
    {
        x *= 1D;
        z *= 1D;

        int xInt = (x > .0 ? (int)x : (int)x - 1);
        int zInt = (z > .0 ? (int)z : (int)z - 1);

        double dCandidate = 32000000.0;
        double xCandidate = 0;
        double zCandidate = 0;

        double dNeighbour = 32000000.0;
        double xNeighbour = 0;
        double zNeighbour = 0;

        for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++)
        {
            for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
            {

                double xPos = xCur + valueNoise2D(xCur, zCur, seed);
                double zPos = zCur + valueNoise2D(xCur, zCur, seed - 99);
                double xDist = xPos - x;
                double zDist = zPos - z;
                //double dist = xDist * xDist + zDist * zDist;
                double dist = getDistance2D(xPos - x, zPos - z);

                if (dist < dCandidate)
                {
                    dNeighbour = dCandidate;
                    dCandidate = dist;

                    /*dNeighbour = dCandidate;
                    xNeighbour = xCandidate;
                    zNeighbour = zCandidate;

                    dCandidate = dist;
                    xCandidate = xPos;
                    zCandidate = zPos;*/
                }
                else if (dist < dNeighbour)
                {
                    dNeighbour = dist;
                }
            }
        }

        //double c = getDistance2D(xNeighbour - x, zNeighbour - z) - getDistance2D(xCandidate - x, zCandidate - z);
        double c = dNeighbour - dCandidate;
        if (c < width)
        {
            return (((float)(c / width)) - 1f) * depth;
        }
        else
        {
            return 0f;
        }
    }

    public double noise(double x, double y, double z, double frequency)
    {
        // Inside each unit cube, there is a seed point at a random position.  Go
        // through each of the nearby cubes until we find a cube with a seed point
        // that is closest to the specified position.
        x *= frequency;
        y *= frequency;
        z *= frequency;

        int xInt = (x > .0 ? (int)x : (int)x - 1);
        int yInt = (y > .0 ? (int)y : (int)y - 1);
        int zInt = (z > .0 ? (int)z : (int)z - 1);

        double minDist = 32000000.0;

        double xCandidate = 0;
        double yCandidate = 0;
        double zCandidate = 0;

        for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++)
        {
            for (int yCur = yInt - 2; yCur <= yInt + 2; yCur++)
            {
                for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++)
                {
                    // Calculate the position and distance to the seed point inside of
                    // this unit cube.

                    double xPos = xCur + valueNoise3D(xCur, yCur, zCur, seed);
                    double yPos = yCur + valueNoise3D(xCur, yCur, zCur, seed);
                    double zPos = zCur + valueNoise3D(xCur, yCur, zCur, seed);
                    double xDist = xPos - x;
                    double yDist = yPos - y;
                    double zDist = zPos - z;
                    double dist = xDist * xDist + yDist * yDist + zDist * zDist;

                    if (dist < minDist)
                    {
                        // This seed point is closer to any others found so far, so record
                        // this seed point.
                        minDist = dist;
                        xCandidate = xPos;
                        yCandidate = yPos;
                        zCandidate = zPos;
                    }
                }
            }
        }

        if (useDistance)
        {
            double xDist = xCandidate - x;
            double yDist = yCandidate - y;
            double zDist = zCandidate - z;

            return getDistance(xDist, yDist, zDist);
        }

        else
            return ((double)CellNoise.valueNoise3D(
              (int)(Math.Round(xCandidate - 0.5)),
              (int)(Math.Round(yCandidate - 0.5)),
              (int)(Math.Round(zCandidate - 0.5)), seed));

    }

    /**
     * To avoid having to store the feature points, we use a hash function 
     * of the coordinates and the seed instead. Those big scary numbers are
     * arbitrary primes.
     */
    public static double valueNoise2D(int x, int z, long seed)
    {
        long n = (1619 * x + 6971 * z + 1013 * seed) & 0x7fffffff;
        n = (n >> 13) ^ n;
        return 1.0 - ((double)((n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff) / 1073741824.0);
    }

    public static double valueNoise3D(int x, int y, int z, long seed)
    {
        long n = (1619 * x + 31337 * y + 6971 * z + 1013 * seed) & 0x7fffffff;
        n = (n >> 13) ^ n;
        return 1.0 - ((double)((n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff) / 1073741824.0);
    }

}



public class PerlinNoise
{
    private static double STRETCH_2D = -0.211324865405187;    //(1/Math.sqrt(2+1)-1)/2;
    private static double STRETCH_3D = -1.0 / 6.0;            //(1/Math.sqrt(3+1)-1)/3;
    private static double SQUISH_2D = 0.366025403784439;      //(Math.sqrt(2+1)-1)/2;
    private static double SQUISH_3D = 1.0 / 3.0;              //(Math.sqrt(3+1)-1)/3;

    private static long DEFAULT_SEED = 0;

    private int[] perm;
    private int[] perm2D;
    private int[] perm3D;

    public PerlinNoise(long seed)
    {
        perm = new int[256];
        perm2D = new int[256];
        perm3D = new int[256];
        int[] source = new int[256];
        for (int i = 0; i < 256; i++)
        {
            source[i] = i;
        }
        for (int i = 255; i >= 0; i--)
        {
            seed = seed * 6364136223846793005L + 1442695040888963407L;
            int r = (int)((seed + 31) % (i + 1));
            if (r < 0)
            {
                r += (i + 1);
            }
            perm[i] = source[r];
            perm2D[i] = ((perm[i] % 12) * 2);
            perm3D[i] = ((perm[i] % 24) * 3);
            source[r] = source[i];
        }
    }


    //Alias for 1D
    //
    // Ranges in [-1, 1]
    //
    public float noise1(float x)
    {
        return (float)noise(x, 0.5);
    }

    //Alias for 2D
    public float noise2(float x, float y)
    {
        return (float)noise(x, y);
    }

    //Alias for 3D
    public float noise3(float x, float y, float z)
    {
        return (float)noise(x, y, z);
    }

    //Alias for 3D (again)
    public double improvedNoise(double x, double y, double z)
    {
        return noise(x, y, z);
    }

    //2D OpenSimplex Noise
    public double noise(double x, double y)
    {

        //Place input coordinates onto grid.
        double stretchOffset = (x + y) * STRETCH_2D;
        double xs = x + stretchOffset;
        double ys = y + stretchOffset;

        //Floor to get grid coordinates of rhombus (stretched square) super-cell origin.
        int xsb = fastFloor(xs);
        int ysb = fastFloor(ys);

        //Skew out to get actual coordinates of rhombus origin. We'll need these later.
        double squishOffset = (xsb + ysb) * SQUISH_2D;
        double xb = xsb + squishOffset;
        double yb = ysb + squishOffset;

        //Compute grid coordinates relative to rhombus origin.
        double xins = xs - xsb;
        double yins = ys - ysb;

        //Sum those together to get a value that determines which region we're in.
        double inSum = xins + yins;

        //Positions relative to origin point.
        double dx0 = x - xb;
        double dy0 = y - yb;

        //We'll be defining these inside the next block and using them afterwards.
        double dx_ext, dy_ext;
        int xsv_ext, ysv_ext;

        double value = 0;

        //Contribution (1,0)
        double dx1 = dx0 - 1 - SQUISH_2D;
        double dy1 = dy0 - 0 - SQUISH_2D;
        double attn1 = 2 - dx1 * dx1 - dy1 * dy1;
        if (attn1 > 0)
        {
            attn1 *= attn1;
            value += attn1 * attn1 * extrapolate2D(xsb + 1, ysb + 0, dx1, dy1);
        }

        //Contribution (0,1)
        double dx2 = dx0 - 0 - SQUISH_2D;
        double dy2 = dy0 - 1 - SQUISH_2D;
        double attn2 = 2 - dx2 * dx2 - dy2 * dy2;
        if (attn2 > 0)
        {
            attn2 *= attn2;
            value += attn2 * attn2 * extrapolate2D(xsb + 0, ysb + 1, dx2, dy2);
        }

        if (inSum <= 1)
        { //We're inside the triangle (2-Simplex) at (0,0)
            double zins = 1 - inSum;
            if (zins > xins || zins > yins)
            { //(0,0) is one of the closest two triangular vertices
                if (xins > yins)
                {
                    xsv_ext = xsb + 1;
                    ysv_ext = ysb - 1;
                    dx_ext = dx0 - 1;
                    dy_ext = dy0 + 1;
                }
                else
                {
                    xsv_ext = xsb - 1;
                    ysv_ext = ysb + 1;
                    dx_ext = dx0 + 1;
                    dy_ext = dy0 - 1;
                }
            }
            else
            { //(1,0) and (0,1) are the closest two vertices.
                xsv_ext = xsb + 1;
                ysv_ext = ysb + 1;
                dx_ext = dx0 - 1 - 2 * SQUISH_2D;
                dy_ext = dy0 - 1 - 2 * SQUISH_2D;
            }
        }
        else
        { //We're inside the triangle (2-Simplex) at (1,1)
            double zins = 2 - inSum;
            if (zins < xins || zins < yins)
            { //(0,0) is one of the closest two triangular vertices
                if (xins > yins)
                {
                    xsv_ext = xsb + 2;
                    ysv_ext = ysb + 0;
                    dx_ext = dx0 - 2 - 2 * SQUISH_2D;
                    dy_ext = dy0 + 0 - 2 * SQUISH_2D;
                }
                else
                {
                    xsv_ext = xsb + 0;
                    ysv_ext = ysb + 2;
                    dx_ext = dx0 + 0 - 2 * SQUISH_2D;
                    dy_ext = dy0 - 2 - 2 * SQUISH_2D;
                }
            }
            else
            { //(1,0) and (0,1) are the closest two vertices.
                dx_ext = dx0;
                dy_ext = dy0;
                xsv_ext = xsb;
                ysv_ext = ysb;
            }
            xsb += 1;
            ysb += 1;
            dx0 = dx0 - 1 - 2 * SQUISH_2D;
            dy0 = dy0 - 1 - 2 * SQUISH_2D;
        }

        //Contribution (0,0) or (1,1)
        double attn0 = 2 - dx0 * dx0 - dy0 * dy0;
        if (attn0 > 0)
        {
            attn0 *= attn0;
            value += attn0 * attn0 * extrapolate2D(xsb, ysb, dx0, dy0);
        }

        //Extra Vertex
        double attn_ext = 2 - dx_ext * dx_ext - dy_ext * dy_ext;
        if (attn_ext > 0)
        {
            attn_ext *= attn_ext;
            value += attn_ext * attn_ext * extrapolate2D(xsv_ext, ysv_ext, dx_ext, dy_ext);
        }

        return value;
    }

    //3D OpenSimplex Noise
    public double noise(double x, double y, double z)
    {
        double stretchOffset = (x + y + z) * STRETCH_3D;
        double xs = x + stretchOffset;
        double ys = y + stretchOffset;
        double zs = z + stretchOffset;

        int xsb = fastFloor(xs);
        int ysb = fastFloor(ys);
        int zsb = fastFloor(zs);

        double squishOffset = (xsb + ysb + zsb) * SQUISH_3D;
        double dx0 = x - (xsb + squishOffset);
        double dy0 = y - (ysb + squishOffset);
        double dz0 = z - (zsb + squishOffset);

        double xins = xs - xsb;
        double yins = ys - ysb;
        double zins = zs - zsb;

        double inSum = xins + yins + zins;

        int hash =
           (int)(yins - zins + 1) |
           (int)(xins - yins + 1) << 1 |
           (int)(xins - zins + 1) << 2 |
           (int)inSum << 3 |
           (int)(inSum + zins) << 5 |
           (int)(inSum + yins) << 7 |
           (int)(inSum + xins) << 9;

        Contribution3 c = lookup3D[hash];

        double value = 0.0;
        while (c != null)
        {
            double dx = dx0 + c.dx;
            double dy = dy0 + c.dy;
            double dz = dz0 + c.dz;
            double attn = 2 - dx * dx - dy * dy - dz * dz;
            if (attn > 0)
            {
                int px = xsb + c.xsb;
                int py = ysb + c.ysb;
                int pz = zsb + c.zsb;

                int i = perm3D[(perm[(perm[px & 0xFF] + py) & 0xFF] + pz) & 0xFF];
                double valuePart = gradients3D[i] * dx + gradients3D[i + 1] * dy + gradients3D[i + 2] * dz;

                attn *= attn;
                value += attn * attn * valuePart;
            }

            c = c.next;
        }
        return value;
    }

    private double extrapolate2D(int xsb, int ysb, double dx, double dy)
    {
        int index = perm2D[(perm[xsb & 0xFF] + ysb) & 0xFF];
        return gradients2D[index] * dx + gradients2D[index + 1] * dy;
    }

    private static int fastFloor(double x)
    {
        int xi = (int)x;
        return x < xi ? xi - 1 : xi;
    }

    private static Contribution3[] lookup3D;

    private static void doShit()
    {
        int[][] base3D = new int[][] {
            new int[] { 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1 },
            new int[] { 2, 1, 1, 0, 2, 1, 0, 1, 2, 0, 1, 1, 3, 1, 1, 1 },
            new int[] { 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 2, 1, 1, 0, 2, 1, 0, 1, 2, 0, 1, 1 }
        };
        int[] p3D = new int[] { 0, 0, 1, -1, 0, 0, 1, 0, -1, 0, 0, -1, 1, 0, 0, 0, 1, -1, 0, 0, -1, 0, 1, 0, 0, -1, 1, 0, 2, 1, 1, 0, 1, 1, 1, -1, 0, 2, 1, 0, 1, 1, 1, -1, 1, 0, 2, 0, 1, 1, 1, -1, 1, 1, 1, 3, 2, 1, 0, 3, 1, 2, 0, 1, 3, 2, 0, 1, 3, 1, 0, 2, 1, 3, 0, 2, 1, 3, 0, 1, 2, 1, 1, 1, 0, 0, 2, 2, 0, 0, 1, 1, 0, 1, 0, 2, 0, 2, 0, 1, 1, 0, 0, 1, 2, 0, 0, 2, 2, 0, 0, 0, 0, 1, 1, -1, 1, 2, 0, 0, 0, 0, 1, -1, 1, 1, 2, 0, 0, 0, 0, 1, 1, 1, -1, 2, 3, 1, 1, 1, 2, 0, 0, 2, 2, 3, 1, 1, 1, 2, 2, 0, 0, 2, 3, 1, 1, 1, 2, 0, 2, 0, 2, 1, 1, -1, 1, 2, 0, 0, 2, 2, 1, 1, -1, 1, 2, 2, 0, 0, 2, 1, -1, 1, 1, 2, 0, 0, 2, 2, 1, -1, 1, 1, 2, 0, 2, 0, 2, 1, 1, 1, -1, 2, 2, 0, 0, 2, 1, 1, 1, -1, 2, 0, 2, 0 };
        int[] lookupPairs3D = new int[] { 0, 2, 1, 1, 2, 2, 5, 1, 6, 0, 7, 0, 32, 2, 34, 2, 129, 1, 133, 1, 160, 5, 161, 5, 518, 0, 519, 0, 546, 4, 550, 4, 645, 3, 647, 3, 672, 5, 673, 5, 674, 4, 677, 3, 678, 4, 679, 3, 680, 13, 681, 13, 682, 12, 685, 14, 686, 12, 687, 14, 712, 20, 714, 18, 809, 21, 813, 23, 840, 20, 841, 21, 1198, 19, 1199, 22, 1226, 18, 1230, 19, 1325, 23, 1327, 22, 1352, 15, 1353, 17, 1354, 15, 1357, 17, 1358, 16, 1359, 16, 1360, 11, 1361, 10, 1362, 11, 1365, 10, 1366, 9, 1367, 9, 1392, 11, 1394, 11, 1489, 10, 1493, 10, 1520, 8, 1521, 8, 1878, 9, 1879, 9, 1906, 7, 1910, 7, 2005, 6, 2007, 6, 2032, 8, 2033, 8, 2034, 7, 2037, 6, 2038, 7, 2039, 6 };

        Contribution3[] contributions3D = new Contribution3[p3D.Length / 9];
        for (int i = 0; i < p3D.Length; i += 9)
        {
            int[] baseSet = base3D[p3D[i]];
            Contribution3 previous = null, current = null;
            for (int k = 0; k < baseSet.Length; k += 4)
            {
                current = new Contribution3(baseSet[k], baseSet[k + 1], baseSet[k + 2], baseSet[k + 3]);
                if (previous == null)
                {
                    contributions3D[i / 9] = current;
                }
                else
                {
                    previous.next = current;
                }
                previous = current;
            }
            current.next = new Contribution3(p3D[i + 1], p3D[i + 2], p3D[i + 3], p3D[i + 4]);
            current.next.next = new Contribution3(p3D[i + 5], p3D[i + 6], p3D[i + 7], p3D[i + 8]);
        }

        lookup3D = new Contribution3[2048];
        for (int i = 0; i < lookupPairs3D.Length; i += 2)
        {
            lookup3D[lookupPairs3D[i]] = contributions3D[lookupPairs3D[i + 1]];
        }
    }

    //2D Gradients -- new scheme (Dodecagon)
    private static double[] gradients2D = new double[] {
       0.114251372530929,   0.065963060686016,
       0.131926121372032,   0.000000000000000,
       0.114251372530929,  -0.065963060686016,
       0.065963060686016,  -0.114251372530929,
       0.000000000000000,  -0.131926121372032,
      -0.065963060686016,  -0.114251372530929,
      -0.114251372530929,  -0.065963060686016,
      -0.131926121372032,  -0.000000000000000,
      -0.114251372530929,   0.065963060686016,
      -0.065963060686016,   0.114251372530929,
      -0.000000000000000,   0.131926121372032,
       0.065963060686016,   0.114251372530929,
    };

    //3D Gradients (Stretched Rhombicuboctahedron)
    private static double[] gradients3D = new double[] {
        -0.106796116504854,     0.0388349514563107,     0.0388349514563107,
        -0.0388349514563107,    0.106796116504854,      0.0388349514563107,
        -0.0388349514563107,    0.0388349514563107,     0.106796116504854,
        0.106796116504854,      0.0388349514563107,     0.0388349514563107,
        0.0388349514563107,     0.106796116504854,      0.0388349514563107,
        0.0388349514563107,     0.0388349514563107,     0.106796116504854,
        -0.106796116504854,     -0.0388349514563107,    0.0388349514563107,
        -0.0388349514563107,    -0.106796116504854,     0.0388349514563107,
        -0.0388349514563107,    -0.0388349514563107,    0.106796116504854,
        0.106796116504854,      -0.0388349514563107,    0.0388349514563107,
        0.0388349514563107,     -0.106796116504854,     0.0388349514563107,
        0.0388349514563107,     -0.0388349514563107,    0.106796116504854,
        -0.106796116504854,     0.0388349514563107,     -0.0388349514563107,
        -0.0388349514563107,    0.106796116504854,      -0.0388349514563107,
        -0.0388349514563107,    0.0388349514563107,     -0.106796116504854,
        0.106796116504854,      0.0388349514563107,     -0.0388349514563107,
        0.0388349514563107,     0.106796116504854,      -0.0388349514563107,
        0.0388349514563107,     0.0388349514563107,     -0.106796116504854,
        -0.106796116504854,     -0.0388349514563107,    -0.0388349514563107,
        -0.0388349514563107,    -0.106796116504854,     -0.0388349514563107,
        -0.0388349514563107,    -0.0388349514563107,    -0.106796116504854,
        0.106796116504854,      -0.0388349514563107,    -0.0388349514563107,
        0.0388349514563107,     -0.106796116504854,     -0.0388349514563107,
        0.0388349514563107,     -0.0388349514563107,    -0.106796116504854,
    };
}

public class Contribution3
{
    public double dx, dy, dz;
    public int xsb, ysb, zsb;
    public Contribution3 next;

    public Contribution3(double multiplier, int xsb, int ysb, int zsb)
    {
        dx = -xsb - multiplier * (1.0 / 3.0);
        dy = -ysb - multiplier * (1.0 / 3.0);
        dz = -zsb - multiplier * (1.0 / 3.0);
        this.xsb = xsb;
        this.ysb = ysb;
        this.zsb = zsb;
    }
}

public class NoiseGen
{

    public double XScale = 0.02;
    public double YScale = 0.02;
    public double ZScale = 1;
    public byte Octaves = 1;

    public double Scale
    {
        set
        {
            XScale = value;
            YScale = value;
        }
    }

    public NoiseGen()
    {

    }

    public NoiseGen(double pScale, byte pOctaves)
    {
        XScale = pScale;
        YScale = pScale;
        Octaves = pOctaves;
    }

    public NoiseGen(double pXScale, double pYScale, byte pOctaves)
    {
        XScale = pXScale;
        YScale = pYScale;
        Octaves = pOctaves;
    }

    public float GetNoise(double x, double y, double z)
    {
        if (Octaves > 1)
            return Noise.GetOctaveNoise(x * XScale, y * YScale, z * ZScale, Octaves);
        else
            return Noise.GetNoise(x * XScale, y * YScale, z * ZScale);
    }
}

// Simplex noise in 3D
public static class Noise
{
    // Inner class to speed up gradient computations
    // (array access is a lot slower than member access)
    private struct Grad
    {
        public double x, y, z, w;

        public Grad(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
        }
    }

    private static Grad[] grad3 = new Grad[] {
            new Grad(1,1,0), new Grad(-1,1,0), new Grad(1,-1,0), new Grad(-1,-1,0),
            new Grad(1,0,1), new Grad(-1,0,1), new Grad(1,0,-1), new Grad(-1,0,-1),
            new Grad(0,1,1), new Grad(0,-1,1), new Grad(0,1,-1), new Grad(0,-1,-1)
        };

    private static short[] p = new short[] {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,190,6,148,
            247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,
            74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,102,143,54,
            65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,
            52,217,226,250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,223,183,170,213,
            119,248,152,2,44,154,163,70,221,153,101,155,167,43,172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,
            218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,239,107,49,192,214,31,181,199,106,157,
            184,84,204,176,115,121,50,45,127,4,150,254,138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

    // To remove the need for index wrapping, double the permutation table length
    private static short[] perm = new short[512];
    private static short[] permMod12 = new short[512];

    static Noise()
    {
        for (int i = 0; i < 512; i++)
        {
            perm[i] = p[i & 255];
            permMod12[i] = (short)(perm[i] % 12);
        }
    }

    // Skewing and unskewing factors for 2, 3, and 4 dimensions
    private static double F3 = 1.0 / 3.0;
    private static double G3 = 1.0 / 6.0;

    // This method is a *lot* faster than using (int)Math.floor(x)
    private static int fastfloor(double x)
    {
        int xi = (int)x;
        return x < xi ? xi - 1 : xi;
    }

    private static double dot(Grad g, double x, double y, double z)
    {
        return g.x * x + g.y * y + g.z * z;
    }

    // 3D simplex noise
    public static float GetNoise(double xin, double yin, double zin)
    {
        double n0, n1, n2, n3; // Noise contributions from the four corners
                               // Skew the input space to determine which simplex cell we're in
        double s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
        int i = fastfloor(xin + s);
        int j = fastfloor(yin + s);
        int k = fastfloor(zin + s);
        double t = (i + j + k) * G3;
        double X0 = i - t; // Unskew the cell origin back to (x,y,z) space
        double Y0 = j - t;
        double Z0 = k - t;
        double x0 = xin - X0; // The x,y,z distances from the cell origin
        double y0 = yin - Y0;
        double z0 = zin - Z0;
        // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
        // Determine which simplex we are in.
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
        if (x0 >= y0)
        {
            if (y0 >= z0)
            { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
        }
        else
        { // x0<y0
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
        }
        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
        // c = 1/6.
        double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
        double y1 = y0 - j1 + G3;
        double z1 = z0 - k1 + G3;
        double x2 = x0 - i2 + 2.0 * G3; // Offsets for third corner in (x,y,z) coords
        double y2 = y0 - j2 + 2.0 * G3;
        double z2 = z0 - k2 + 2.0 * G3;
        double x3 = x0 - 1.0 + 3.0 * G3; // Offsets for last corner in (x,y,z) coords
        double y3 = y0 - 1.0 + 3.0 * G3;
        double z3 = z0 - 1.0 + 3.0 * G3;
        // Work out the hashed gradient indices of the four simplex corners
        int ii = i & 255;
        int jj = j & 255;
        int kk = k & 255;
        int gi0 = permMod12[ii + perm[jj + perm[kk]]];
        int gi1 = permMod12[ii + i1 + perm[jj + j1 + perm[kk + k1]]];
        int gi2 = permMod12[ii + i2 + perm[jj + j2 + perm[kk + k2]]];
        int gi3 = permMod12[ii + 1 + perm[jj + 1 + perm[kk + 1]]];
        // Calculate the contribution from the four corners
        double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0; // change to 0.5 if you want
        if (t0 < 0) n0 = 0.0;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
        }
        double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1; // change to 0.5 if you want
        if (t1 < 0) n1 = 0.0;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
        }
        double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2; // change to 0.5 if you want
        if (t2 < 0) n2 = 0.0;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
        }
        double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3; // change to 0.5 if you want
        if (t3 < 0) n3 = 0.0;
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
        }
        // Add contributions from each corner to get the final noise value.
        // The result is scaled to stay just inside [-1,1] (now [0, 1])
        return (float)(32.0 * (n0 + n1 + n2 + n3) + 1) * 0.5f; // change to 76.0 if you want
    }

    // get multiple octaves of noise at once
    public static float GetOctaveNoise(double pX, double pY, double pZ, int pOctaves)
    {
        float value = 0;
        float divisor = 0;
        float currentHalf = 0;
        float currentDouble = 0;

        for (int i = 0; i < pOctaves; i++)
        {
            currentHalf = (float)Math.Pow(0.5f, i);
            currentDouble = (float)Math.Pow(2, i);
            value += GetNoise(pX * currentDouble, pY * currentDouble, pZ) * currentHalf;
            divisor += currentHalf;
        }

        return value / divisor;
    }
}