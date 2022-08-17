using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WpfAppForZoomInAndZoomOut.Helpers;

public static class StringHelper
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly Random rd = new(DateTime.Now.Millisecond);

    public static string RandomString()
    {
        var strLength = rd.Next(10, 50);
        return new string(Enumerable.Repeat(chars, strLength)
            .Select(s => s[rd.Next(s.Length)]).ToArray());
    }


    public static T? FindChild<T>(this DependencyObject parent, string? childName = null)
        where T : DependencyObject
    {
        // Confirm parent and childName are valid. 
        if (parent == null) return null;

        T? foundChild = null;

        var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            // If the child is not of the request child type child
            if (child is not T childType)
            {
                // recursively drill down the tree
                foundChild = FindChild<T>(child, childName);
                // If the child is found, break so we do not overwrite the found child. 
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                // If the child's name is set for search
                if (child is IFrameworkInputElement frameworkInputElement &&
                    frameworkInputElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = (T)child;
                    break;
                }

                // recursively drill down the tree
                foundChild = FindChild<T>(child, childName);
                // If the child is found, break so we do not overwrite the found child. 
                if (foundChild != null) break;
            }
            else
            {
                // child element found.
                foundChild = (T)child;
                break;
            }
        }

        return foundChild;
    }
}