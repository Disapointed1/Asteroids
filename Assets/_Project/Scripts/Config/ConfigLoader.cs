using System;
using Newtonsoft.Json;
using UnityEngine;

public static class ConfigLoader
{
    private const string ConfigFolderPath = "Config";

    public static T Load<T>(string fileName)
    {
        var jsonFile = Resources.Load<TextAsset>($"{ConfigFolderPath}/{fileName}");

        if (jsonFile == null)
            throw new Exception($"Config file not found: {ConfigFolderPath}/{fileName}");

        return JsonConvert.DeserializeObject<T>(jsonFile.text);
    }
}