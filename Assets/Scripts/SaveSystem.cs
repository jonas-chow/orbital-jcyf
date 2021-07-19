using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Replays/"; 

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void SaveReplay(Replay saveReplay)
    {
        Init();
        File.WriteAllText(SAVE_FOLDER + saveReplay.friendlyName + "_vs_" + 
            saveReplay.opponentName + "_" + saveReplay.datetime + ".json", 
            JsonUtility.ToJson(saveReplay));
    }

    public static List<string> GetReplays()
    {
        Init();
        return new List<string>(Directory.EnumerateFiles(SAVE_FOLDER, "*.json"));
    }

    public static Replay LoadReplay(string replayPath)
    {
        Init();
        if (File.Exists(replayPath))
        {
            string saveJson = File.ReadAllText(replayPath);
            return JsonUtility.FromJson<Replay>(saveJson);
        } else {
            throw new FileNotFoundException();
        }
    }

    public static void DeleteReplay(string replayPath)
    {
        File.Delete(replayPath);
    }

    public static void DeleteAllReplays()
    {
        GetReplays().ForEach(path => DeleteReplay(path));
    }
}
