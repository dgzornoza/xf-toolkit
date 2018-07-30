using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D.Helpers
{
    /// <summary>
    /// Helpers to extend common mathematical functions and constants.
    /// </summary>
    public static class MathHelpers
    {
        public static Vector2 RadianToVector2(float radian, bool ccw = true)
        {
            float y = (float)Math.Sin(radian);
            return new Vector2((float)Math.Cos(radian), ccw ? y : -y);
        }

        public static Vector2 DegreeToVector2(float degree, bool ccw = true)
        {
            return RadianToVector2(Urho.MathHelper.DegreesToRadians(degree));
        }

        public static Vector2 Flip(this Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }
    }
}
