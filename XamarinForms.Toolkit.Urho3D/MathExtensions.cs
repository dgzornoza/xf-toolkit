using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D
{
    /// <summary>
    /// Help extensions whith common mathematical functions and constants.
    /// </summary>
    public static class MathExtensions
    {
        public static Vector2 RadianToVector2(float radian, bool ccw = true)
        {
            float x = (float)System.Math.Sin(radian);
            return new Vector2(ccw ? -x : x, (float)System.Math.Cos(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(Urho.MathHelper.DegreesToRadians(degree));
        }

        public static Vector2 Flip(this Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }
    }
}
