using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData<Key, Value>
{
    Dictionary<Key, Value> ToDict();
}

public class DataManager
{
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        StatDict = LoadAndParseJson<StatData, int, Stat>("StatData").ToDict();
    }

    Data LoadAndParseJson<Data, Key, Value>(string path) where Data : IData<Key, Value>
    {
        TextAsset textAsset = Manager.Resource.Load<TextAsset>($"Data/{path}");
        Data data = JsonUtility.FromJson<Data>(textAsset.text); // Deserialize
        return data;
    }
}
