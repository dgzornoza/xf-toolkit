using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D
{
    /// <summary>
    /// Help extensions whith common mathematical functions and constants.
    /// </summary>
    public class MathHelpers
    {
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(MathHelper.DegreesToRadians(degree));
        }
    }
}
