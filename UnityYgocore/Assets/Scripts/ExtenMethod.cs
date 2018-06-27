using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtenMethod 
{

    public static T GetChild<T>(this Transform tran,string name)
    {
        return tran.FindChild(name).GetComponent<T>();
    }

    public static T GetRemoveFirstList<T>(this List<T> list)
    {
        T val = list[0];
        list.RemoveAt(0);
        return val;
    }

    public static T GetRemoveList<T>(this List<T> list,int index)
    {
        T val = list[index];
        list.RemoveAt(index);
        return val;
    }

    public static bool IsBind(this int val, int val1)
    {
        return (val & val1) == 0 ? false : true;
    }


    public static bool IsBind(this Int64 val, Int64 val1)
    {
        return (val & val1) == 0 ? false : true;
    }

    public static List<string> GetCardIDList(this List<Card> list)
    {
        List<string> result = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            result.Add(list[i].cardID);
        }
        return result;
    }

    public static int ToInt(this string val)
    {
        int result;
        int.TryParse(val,out result);
        return result;
    }

    public static bool ToBool(this string val)
    {
        bool result;
        bool.TryParse(val, out result);
        return result;
    }
}
