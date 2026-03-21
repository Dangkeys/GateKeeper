using UnityEngine;
using System.IO;
using System;
using VContainer;
using GateKeeperProject.Scripts;

[Serializable]
public class GameData 
{
    public int highScore;
    public int latestScore;
    public int highestWave;
    public int latestWaveCleared;
}

public class SaveManager : MonoBehaviour 
{
    private string savePath;
    private GameData currentData = new GameData(); 

    private ScoreManager _scoreManager;
    private WaveHandler _waveHandler;

    [Inject]
    public void Construct(ScoreManager scoreManager, WaveHandler waveHandler) 
    {
        _scoreManager = scoreManager;
        _waveHandler = waveHandler;
    }

    void Start() 
    {
        savePath = GetSavePath();
        LoadGame();
    }

    public void SaveGame() 
    {
        int currentScore = _scoreManager != null ? _scoreManager.CurrentScore : 0;
        int currentWave = _waveHandler != null ? _waveHandler.WaveNumber : 0;

        currentData.latestScore = currentScore;
        currentData.latestWaveCleared = currentWave;

        if (currentScore > currentData.highScore) 
        {
            currentData.highScore = currentScore;
        }

        if (currentWave > currentData.highestWave) 
        {
            currentData.highestWave = currentWave;
        }

        string jsonData = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(savePath, jsonData);

        Debug.Log($"Game saved! Latest Score: {currentData.latestScore} | High Score: {currentData.highScore}");
    }

    public void LoadGame() 
    {
        if (File.Exists(savePath)) 
        {
            string jsonData = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<GameData>(jsonData);
        }
    }

    public GameData GetSaveData() 
    {
        return currentData;
    }

    // ==========================================
    // STATIC UTILITY METHODS FOR OTHER SCENES
    // ==========================================

    private static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public static GameData LoadDataStatically() 
    {
        string path = GetSavePath();
        
        if (File.Exists(path)) 
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(jsonData);
        }

        return new GameData(); 
    }
}