using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonSRML.Utils.Extensions;

public static class ListExtensions
{ 
    public static void AddAndRemoveRangeWhere<T>(this ListAsset<T> list, IEnumerable<T> range, Func<T, T, bool> cond)
    {
        List<T> listToAdd = range.ToList();
        bool Predicate(T x)
        {
            return listToAdd.Any(y => cond(x, y));
        }

        List<T> normalList = new List<T>();

        foreach (var VARIABLE in list.items)
        {
            if (Predicate(VARIABLE))
            {
                normalList.Add(VARIABLE);
            }
        }
        
        foreach (var a in normalList)
        {
            list.items.Remove(a);
        }

        foreach (var VARIABLE in listToAdd)
        {
            list.items.Add(VARIABLE);

        }
    }
    public static T GetObjectFromIndexInList<T>(this Il2CppSystem.Collections.Generic.List<T> @this, int index)
    {
        return @this.Cast<Il2CppSystem.Collections.Generic.IReadOnlyList<T>>()[index];
    }

    public static T FindObject<T>(this Il2CppSystem.Collections.Generic.List<T> @this, Predicate<T> predicate)
    {
        foreach (var VARIABLE in @this)
        {
            if (predicate.Invoke(VARIABLE))
            {
                return VARIABLE;
            }

        }

        return default;
    }
}