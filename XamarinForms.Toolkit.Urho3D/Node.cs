using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urho;

namespace XamarinForms.Toolkit.Urho3D
{
    public static class Node
    {
        public static IEnumerable<T> GetRecursiveComponents<T>(this Urho.Node node)
        {
            List<T> components = node.Components.OfType<T>().ToList();

            // si se procesa en cascada, se hace lo mismo en los nodos hijos recursivamente
            foreach (var child in node.Children) components.AddRange(child.GetRecursiveComponents<T>());

            return components;
        }
    }
}
