using System.Collections.Generic;

public static class Extentions
{
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }
}
