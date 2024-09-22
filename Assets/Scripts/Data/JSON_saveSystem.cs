using System.IO;
using UnityEngine;
namespace Game.Data
{
    public static class JSON_saveSystem
    {
        private static string GetPath()
        {
            return Path.Combine(Application.persistentDataPath, ConstSystem.DATA_PATH);
        }

        public static void Save<T>(T data)
        {
            string json = JsonUtility.ToJson(data);
            string path = GetPath();
            File.WriteAllText(path, json);
        }

        public static T Load<T>()
        {
            string path = GetPath();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogWarning("File not found: " + path);
                return default(T);
            }
        }

        public static bool SaveExists()
        {
            return File.Exists(GetPath());
        }

        public static void DeleteSave()
        {
            string path = GetPath();

            if (File.Exists(path))
                File.Delete(path);

            else
                Debug.LogWarning("File not found: " + path);
        }
    }
}


