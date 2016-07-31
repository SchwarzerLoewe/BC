using System;
using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public static class Extensions
    {
        public static T Pop<T>(this Stack s)
        {
            return (T)s.Pop();
        }

        public static void Replace<T>(this List<T> src, Predicate<T> selector, T newItem)
        {
            for (int i = 0; i < src.Count; i++)
            {
                var item = src[i];
                if (selector(item))
                {
                    src[i] = newItem;
                }
            }
        }
        public static Primitive GetPrimitiveFor(this Primitive prim, object val)
        {
            if (val is int) return Primitive.Integer;
            if (val is float) return Primitive.Float;
            if (val is string) return Primitive.String;

            return Primitive.Void;
        }
        public static void Remove<T>(this List<T> src, Predicate<T> selector)
        {
            for (int i = 0; i < src.Count; i++)
            {
                var item = src[i];

                if(selector(item))
                {
                    src.RemoveAt(i);

                    return;
                }
            }
        }
    }
}