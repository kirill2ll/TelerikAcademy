﻿using System;

namespace CohesionAndCoupling
{
    static class MathUtils
    {
        public static double GetVectorMagnitude2D(double x, double y)
        {
            double distance = Math.Sqrt((x * x) + (y * y));
            return distance;
        } 

        public static double GetVectorMagnitude3D(double x, double y, double z)
        {
            double distance = Math.Sqrt((x * x) + (y * y) + (z * z));
            return distance;
        }

        public static double CalcDistance2D(double x1, double y1, double x2, double y2)
        {
            double distance = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            return distance;
        }

        public static double CalcDistance3D(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double distance = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));
            return distance;
        }

        public static double CalcVolume(double width, double height, double depth)
        {
            double volume = width * height * depth;
            return volume;
        }
    }
}
