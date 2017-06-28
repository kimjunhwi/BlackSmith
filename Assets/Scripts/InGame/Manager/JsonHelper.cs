using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static List<T> ListFromJson<T>(string json)
    {
        LWrapper<T> wrapper = JsonUtility.FromJson<LWrapper<T>>(json);
        return wrapper.list;
    }

    public static string ListToJson<T>(List<T> array)
    {
        LWrapper<T> wrapper = new LWrapper<T>();
        wrapper.list = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    [System.Serializable]
    private class LWrapper<T>
    {
        public List<T> list;
    }
}