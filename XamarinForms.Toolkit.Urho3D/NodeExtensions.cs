using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D
{
    public static class NodeExtensions
    {
        public static IEnumerable<T> GetRecursiveComponents<T>(this Urho.Node node)
        {
            List<T> components = node.Components.OfType<T>().ToList();

            // call recursive in childrens
            foreach (var child in node.Children) components.AddRange(child.GetRecursiveComponents<T>());
            
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
