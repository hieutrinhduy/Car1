using System;
using UnityEngine;

namespace Extensions
{
    public static class MathfExtension
    {
        public static float Distance(float a, float b)
        {
            return Math.Abs(a - b);
        }
    }
}