using System;
using GateKeeperProject.Scripts;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public event Action OnRewardSelected;

    [Header("Core Systems")]
    [SerializeField] private BlessingUI blessingUI; 
    [SerializeField] private GunSystem gunSystem;
    [SerializeField] private AmmoSystem ammoSystem;
    [SerializeField] private Sprint sprint;
    [SerializeField] private Player _player;
    [SerializeField] private WaveHandler waveHandler;

    private int _currentStatIndex;
    private int _currentWeaponIndex;
    private WeaponStatType _currentWeaponStatType;
    private int _currentAmmoIndex;

    public void GetReward()
    {
        var (sTitle, sDesc) = RandomStat();
        var (wTitle, wDesc) = RandomWeapon();
        var (aTitle, aDesc) = RandomAmmo();
        blessingUI.UpdateCardUI(sTitle, sDesc, wTitle, wDesc, aTitle, aDesc);
    }

    private (string title, string description) RandomStat()
    {
        _currentStatIndex = UnityEngine.Random.Range(1, 4);
        
        switch (_currentStatIndex)
        {
            case 1: return ("Vitality", "Increase Max Health 10%");
            case 2: return ("Agility", $"Increase Move Speed {sprint.GetPercentSpeedReward()}%");
            case 3: return ("Endurance", $"Increase Stamina {sprint.GetPercentStaminaReward()}%");
            default: return ("Stat", "Unknown Upgrade");
        }
    }

    private (string title, string description) RandomWeapon()
    {
        _currentWeaponIndex = UnityEngine.Random.Range(0, 5);
        GunData gunData = gunSystem.GetGun(_currentWeaponIndex).GetGunData();
        
        do
        {
            int statIndex = UnityEngine.Random.Range(0, 16);
            _currentWeaponStatType = (WeaponStatType)statIndex;
        } while (!gunData.CanChangeStat(_currentWeaponStatType));

        float reward = gunData.GetRewardValue(_currentWeaponStatType);
        string descText = GetStatUpgradeText(gunData, _currentWeaponStatType, reward);
        string titleText = $"{gunData.type} Upgrade";
        
        return (titleText, descText);
    }

    private (string title, string description) RandomAmmo()
    {
        _currentAmmoIndex = UnityEngine.Random.Range(0, 5); 

        GunData gunData = gunSystem.GetGun(_currentAmmoIndex).GetGunData();
        int ammo = gunData.GetAmmoReward();
        
        string titleText = $"{gunData.type} Munitions";
        string descText = $"Gain {ammo} Ammo";

        return (titleText, descText);
    }

    private string GetStatUpgradeText(GunData data, WeaponStatType statType, float reward)
    {
        switch (statType)
        {
            case WeaponStatType.FlatDamage: return $"Damage: {data.flatDamage:0.##} -> {data.flatDamage + reward:0.##}";
            case WeaponStatType.HeadshotMultiplier: return $"Headshot: {data.headshotMultiplier:0.##}x -> {data.headshotMultiplier + reward:0.##}x";
            case WeaponStatType.Range: return $"Range: {data.range:0.##} -> {data.range + reward:0.##}";
            case WeaponStatType.Penetration: return $"Penetration: {data.penetration:0.##} -> {data.penetration + reward:0.##}";
            case WeaponStatType.DamagePenetrationReduction: return $"Penetration Falloff: {data.damagePenetrationReduction:0.##} -> {data.damagePenetrationReduction - reward:0.##}";
            case WeaponStatType.FireRate: return $"Fire Rate: {data.fireRate:0.##} -> {data.fireRate + reward:0.##}";
            case WeaponStatType.RecoilReduction: return $"Recoil Reduction: {data.recoilReduction:0.##} -> {data.recoilReduction + reward:0.##}";
            case WeaponStatType.MaximumRecoil: return $"Max Recoil: {data.maximumRecoil:0.##} -> {data.maximumRecoil - reward:0.##}";
            case WeaponStatType.RecoilRecoveryTime: return $"Recovery Time: {data.recoilRecoveryTime:0.##}s -> {data.recoilRecoveryTime - reward:0.##}s";
            case WeaponStatType.RecoilRecoverySpeed: return $"Recovery Speed: {data.recoilRecoverySpeed:0.##} -> {data.recoilRecoverySpeed + reward:0.##}";
            case WeaponStatType.MagazineSize: return $"Mag Size: {data.magazineSize} -> {data.magazineSize + reward}";
            case WeaponStatType.ReloadTime: return $"Reload Time: {data.reloadTime:0.##}s -> {data.reloadTime - reward:0.##}s";
            case WeaponStatType.FreeAmmoPercent: return $"Free Ammo: {data.freeAmmoPercent:0.##}% -> {data.freeAmmoPercent + reward:0.##}%";
            case WeaponStatType.BulletSize: return $"Bullet Size: {data.bulletSize:0.##} -> {data.bulletSize + reward:0.##}";
            case WeaponStatType.PelletCount: return $"Pellets: {data.pelletCount:0.##} -> {data.pelletCount + reward:0.##}";
            case WeaponStatType.SpreadAngle: return $"Spread: {data.spreadAngle:0.##}° -> {data.spreadAngle + reward:0.##}°";
            default: return $"{statType} Improved";
        }
    }

    public void SelectStatReward()
    {
        GetStat(_currentStatIndex);
        OnRewardSelected?.Invoke(); 
    }

    public void SelectWeaponReward()
    {
        GetWeapon(_currentWeaponIndex, (int)_currentWeaponStatType);
        OnRewardSelected?.Invoke(); 
    }

    public void SelectAmmoReward()
    {
        GetAmmo(_currentAmmoIndex);
        OnRewardSelected?.Invoke(); 
    }

    private void GetStat(int statIndex)
    {
        switch (statIndex)
        {
            case 1: _player.IncreaseMaxHealth(); break;
            case 2: sprint.IncreaseSpeed(); break;
            case 3: sprint.IncreaseStamina(); break;
        }
    }

    private void GetWeapon(int weaponIndex, int statIndex)
    {
        GunData gunData = gunSystem.GetGun(weaponIndex).GetGunData();
        WeaponStatType weaponStatType = (WeaponStatType)statIndex;
        gunData.ApplyReward(weaponStatType);
    }

    private void GetAmmo(int ammoIndex)
    {
        ammoSystem.IncreaseAmmo((WeaponType)ammoIndex, 100);
    }
}