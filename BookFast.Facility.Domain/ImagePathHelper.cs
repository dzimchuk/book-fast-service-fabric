using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookFast.Facility.Domain
{
    internal static class ImagePathHelper
    {
        private static readonly Regex ImagePath = new Regex(@"^(?<path>[^\?]+)\?*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string[] CleanUp(string[] paths)
        {
            if (paths == null)
                return null;

            var result = new List<string>();
            foreach (var path in paths)
            {
                var m = ImagePath.Match(path);
                result.Add(m.Success ? m.Groups["path"].Value : path);
            }

            return result.ToArray();
        }

        public static string[] Merge(string[] existingPaths, string[] newPaths)
        {
            if (newPaths == null)
                return existingPaths;

            newPaths = CleanUp(newPaths);

            if (existingPaths == null)
                return newPaths;

            var result = new List<string>(existingPaths);

            var imagesToRemove = existingPaths.Except(newPaths, new CaseInsensitiveEqualityComparer());
            foreach (var imageToRemove in imagesToRemove)
            {
                result.Remove(imageToRemove);
            }

            var imagesToAdd = newPaths.Except(existingPaths, new CaseInsensitiveEqualityComparer());
            foreach (var imageToAdd in imagesToAdd)
            {
                result.Add(imageToAdd);
            }

            return result.ToArray();
        }

        private class CaseInsensitiveEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => x.Equals(y, StringComparison.OrdinalIgnoreCase);

            public int GetHashCode(string obj) => obj.ToUpperInvariant().GetHashCode();
        }
    }
}
