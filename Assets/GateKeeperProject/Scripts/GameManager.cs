using System.Collections;
using GateKeeperProject.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WaveHandler waveHandler;
    [SerializeField] private RewardSystem rewardSystem;
    [SerializeField] private BlessingUI blessingUI;
    [SerializeField] private Player _player;

    void Start()
    {
        waveHandler.OnWaveComplete += EndWaveEvent;

        rewardSystem.OnRewardSelected += StartNextWave;

        StartNextWave();
    }

    private void EndWaveEvent()
    {
        blessingUI.gameObject.SetActive(true);

        rewardSystem.GetReward();
    }

    public void StartNextWave()
    {
        blessingUI.gameObject.SetActive(false);

        _player.PlayerHealth.SetCurrentToMaxHealth();
        waveHandler.StartNextWave();
    }

    private void OnDestroy()
    {
        // Good practice to unsubscribe from events when the object is destroyed
        if (waveHandler != null) waveHandler.OnWaveComplete -= EndWaveEvent;
        if (rewardSystem != null) rewardSystem.OnRewardSelected -= StartNextWave;
    }
}