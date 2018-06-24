using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XamarinForms.Toolkit.Helpers
{
    /// <summary>
    /// Helpers for manage paths
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// Simplifies path, by removed upper folder dots references "folder\..\folder2" will be converted in "folder2"
        /// </summary>
        /// <param name="path">Path to simplify</param>
        /// <returns>path simplified</returns>
        public static string SimplifyPath(string path)
        {
            while (true)
            {
                string newPath = new Regex(@"[^\\/]+(?<!\.\.)[\\/]\.\.[\\/]").Replace(path, "");
                if (newPath == path) break;
                path = newPath;
            }
            return path;
        }
    }
}
