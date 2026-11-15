using System;
using System.Collections.Generic;

public static class EnumFactory<TEnum, TBase>
    where TEnum : Enum
    where TBase : class
{
    private static readonly Dictionary<TEnum, Type> registry = new Dictionary<TEnum, Type>();

    public static void Register(TEnum key, Type type)
    {
        registry[key] = type;
    }

    public static TBase Create(TEnum key)
    {
        if (registry.TryGetValue(key, out var type))
            return (TBase)Activator.CreateInstance(type);
        
        throw new ArgumentException($"Behavior of type {key} not found in registry.");
    }
}
