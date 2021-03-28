using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static List<Level> levelData;
    public static GameSettings settings;

    // Called when a level is updated and needs to be saved
    public static void SaveLevels(List<Level> lvls)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.bin";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        bf.Serialize(stream, lvls);
        stream.Close();
    }
    // Call on start of game
    public static List<Level> LoadLevels() {
        string path = Application.persistentDataPath + "/levels.bin";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<Level> loadedLevels = bf.Deserialize(stream) as List<Level>;
            stream.Close();
            levelData = loadedLevels;
            return loadedLevels;
        }
        else {
            Debug.LogError("Cannot find level data file at: " + path);
        }
        return null;
    }

    public static void SaveGameSettings(GameSettings settings) {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.bin";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        bf.Serialize(stream, settings);
        stream.Close();
    }

    public static GameSettings LoadGameSettings() {
        string path = Application.persistentDataPath + "/settings.bin";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameSettings gs = bf.Deserialize(stream) as GameSettings;
            stream.Close();
            settings = gs;
            return gs;
        }
        else {
            Debug.LogError("Cannot find game settings data file at: " + path);
        }
        return null;
    }
}
