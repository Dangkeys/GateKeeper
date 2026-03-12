using System.Collections;
using GateKeeperProject.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WaveHandler waveHandler;
    [SerializeField] private RewardSystem rewardSystem;
    [SerializeField] private BlessingUI _blessingUI;
    [SerializeField] private Player _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveHandler.OnWaveComplete += EndWaveEvent;
        StartNextWave();
    }

    private void EndWaveEvent()
    {
        _blessingUI.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartNextWave()
    {
        _blessingUI.gameObject.SetActive(false);
        _player.PlayerHealth.SetCurrentToMaxHealth();
        waveHandler.StartNextWave();
    }

}
