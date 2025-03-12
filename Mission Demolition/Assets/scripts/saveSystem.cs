using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(string path, T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static void Load<T>(string path, T target)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, target);
        }
    }
}
