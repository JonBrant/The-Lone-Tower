using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DictionaryExtensions
{
    public static bool HasAtLeast(this Dictionary<CurrencyTypes, float> lhs, Dictionary<CurrencyTypes, float> rhs)
    {
        if (!rhs.Keys.All(key => lhs.ContainsKey(key)))
        {
            return false;
        }

        foreach (KeyValuePair<CurrencyTypes, float> kvp in rhs)
        {
            if (lhs[kvp.Key] < rhs[kvp.Key])
            {
                return false;
            }
        }

        return true;
    }

    public static void SubtractValues(this Dictionary<CurrencyTypes, float> lhs, Dictionary<CurrencyTypes, float> rhs)
    {
        foreach (KeyValuePair<CurrencyTypes, float> kvp in rhs)
        {
            lhs[kvp.Key] -= rhs[kvp.Key];
        }
    }
    public static void AddValues(this Dictionary<CurrencyTypes, float> lhs, Dictionary<CurrencyTypes, float> rhs)
    {
        foreach (KeyValuePair<CurrencyTypes, float> kvp in rhs)
        {
            lhs[kvp.Key] += rhs[kvp.Key];
        }
    }

    public static string ToCommaString(this Dictionary<CurrencyTypes, float> dictionary)
    {
        string returnValue = "";
        var keys = dictionary.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            returnValue += $"{dictionary[keys[i]]} {keys[i]}";
            if (i + 1 < keys.Count)
            {
                returnValue += ", ";
            }
        }

        return returnValue;
    }
}