using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gemmeleg
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x: x.HasValue ? x.Value : v.x, y: y.HasValue ? y.Value : v.y, z: z.HasValue ? z.Value : v.z);
        }
        public static Vector3 Add(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x: x.HasValue ? v.x + x.Value : v.x, y: y.HasValue ? v.y + y.Value : v.y, z: z.HasValue ? v.z + z.Value : v.z);
        }
    }

    public static class ComponentExtensions
    {
        public static bool TryGetComponent<T>(this Component component, out T result)
        {
            result = component.GetComponent<T>();
            return result != null;
        }
    }
}
