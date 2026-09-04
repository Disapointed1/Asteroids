using UnityEngine;
using Newtonsoft.Json;

public static class ConfigLoader
{
    public static T Load<T>(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Config/{fileName}");
        return JsonConvert.DeserializeObject<T>(jsonFile.text);
    }
}
