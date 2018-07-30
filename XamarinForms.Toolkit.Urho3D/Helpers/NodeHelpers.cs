using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D.Helpers
{
    /// <summary>
    /// Helpers to extend urho node functions
    /// </summary>
    public static class NodeHelpers
    {
        #region [Hacks]


        [DllImport("mono-urho", CallingConvention = CallingConvention.Cdecl)]
        private static extern string Node_GetVar11(IntPtr handle, int key);
        /// <summary>
        /// Hack for GetVar (Currently not found in urhosharp). this should be removed when exists in library.
        /// </summary>
        public static string GetVar(this Node node, StringHash key)
        {
            return Node_GetVar11(node.Handle, key.Code);
        }

        #endregion [Hacks]

        /// <summary>
        /// Function for get specified type components (including inherited types)
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="node">Node where search components</param>
        /// <param name="recursive">true if reursive, false otherwise</param>
        /// <returns>Enumerable components found</returns>
        public static IEnumerable<T> GetInheritedComponents<T>(this Urho.Node node, bool recursive = false)
        {
            List<T> components = node.Components.OfType<T>().ToList();

            // call recursive in childrens
            if (recursive) foreach (var child in node.Children) components.AddRange(child.GetInheritedComponents<T>(true));

            return components;
        }

        /// <summary>
        /// function to verify if a node leaves the screen, in which case, it will be positioned on the other side
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <param name="camera">Camera with screen limits</param>
        public static void MirrorIfExitScreen(this Urho.Node node, Camera camera)
        {
            if (null == node) return;

            Vector2 position = camera.WorldToScreenPoint(node.Position);
            Vector3 screenMin = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 screenMax = camera.ScreenToWorldPoint(new Vector3(1.0f, 1.0f, 0));

            if (position.X > 1.0f)
                node.SetTransform2D(new Vector2(screenMin.X, node.Position.Y), node.Rotation2D);

            if (position.X < 0)
                node.SetTransform2D(new Vector2(screenMax.X, node.Position.Y), node.Rotation2D);

            if (position.Y > 1.0f)
                node.SetTransform2D(new Vector2(node.Position.X, screenMin.Y), node.Rotation2D);

            if (position.Y < 0)
                node.SetTransform2D(new Vector2(node.Position.X, screenMax.Y), node.Rotation2D);
        }
    }
}
