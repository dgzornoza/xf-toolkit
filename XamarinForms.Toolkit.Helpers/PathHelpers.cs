using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace XamarinForms.Toolkit.Helpers
{
    /// <summary>
    /// Helpers for manage paths
    /// </summary>
    public static class PathHelpers
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
		
        /// <summary>
        /// Ensure end slash in path
        /// </summary>
        /// <param name="path">Path for ensure end slash</param>
        /// <returns>Path with end slash</returns>
        public static string EnsureEndSlash(string path)
        {
            string separator1 = Path.DirectorySeparatorChar.ToString();
            string separator2 = Path.AltDirectorySeparatorChar.ToString();

            path = path.TrimEnd();

            if (path.EndsWith(separator1) || path.EndsWith(separator2)) return path;
            if (path.Contains(separator2)) return path + separator2;
            return path + separator1;
        }	
    }
}
