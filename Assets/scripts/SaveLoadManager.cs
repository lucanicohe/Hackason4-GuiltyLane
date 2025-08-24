using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public void SaveDataToJson<T>(string path, T data)
    {
        var s = JsonUtility.ToJson(data, true);
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        using StreamWriter writer = new StreamWriter(path);
        writer.Write(s);
        writer.Close();
        Debug.Log($"save success");
    }

    public T LoadDataFromJson<T>(string path)
    {
        if (File.Exists(path))
        {
            using StreamReader reader = new StreamReader(path);
            var s = reader.ReadToEnd();
            var data = JsonUtility.FromJson<T>(s);
            reader.Close();
            return data;
        }
        return default;
    }


    public void SavePlayerData()
    {
        string dir = Application.persistentDataPath + "/SaveData";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string path = dir + "/SAVEDATA.data";

        //セーブデータClassにデータをいれる。

        // SaveDataToJson(path,//セーブデータ);
    }
}
