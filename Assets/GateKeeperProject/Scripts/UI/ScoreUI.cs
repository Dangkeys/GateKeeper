using UnityEngine;
using TMPro; // Required for TextMeshPro

public class ScoreUI : MonoBehaviour
{
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI latestScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Wave Display")]
    [SerializeField] private TextMeshProUGUI latestWaveText;
    [SerializeField] private TextMeshProUGUI highestWaveText;

    void Start()
    {
        GameData savedData = SaveManager.LoadDataStatically();

        if (latestScoreText != null) 
        {
            latestScoreText.text = $"Score: {savedData.latestScore}";
        }
        
        if (highScoreText != null) 
        {
            highScoreText.text = $"High Score: {savedData.highScore}";
        }

        if (latestWaveText != null) 
        {
            latestWaveText.text = $"Wave Reached: {savedData.latestWaveCleared}";
        }

        if (highestWaveText != null) 
        {
            highestWaveText.text = $"Highest Wave: {savedData.highestWave}";
        }
    }
}