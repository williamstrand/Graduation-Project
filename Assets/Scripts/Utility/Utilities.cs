﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Utility
{
    public static class Utilities
    {
        public static T[] GetAllOfType<T>() where T : class
        {
            var type = typeof(T);
            var assembly = Assembly.GetAssembly(type);
            var items = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && type.IsAssignableFrom(t))
                .Select(t => (T)Activator.CreateInstance(t))
                .ToArray();

            return items;
        }

        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}