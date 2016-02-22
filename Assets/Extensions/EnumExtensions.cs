using UnityEngine;
using System;
using System.ComponentModel;


public static class EnumExtensions
{

    public static bool TryParse<T>(Enum theEnum, string valueToParse, out T returnValue)
    {
        returnValue = default(T);
        if (Enum.IsDefined(typeof(T), valueToParse))
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            returnValue = (T)converter.ConvertFromString(valueToParse);
            return true;
        }
        return false;
    }


}