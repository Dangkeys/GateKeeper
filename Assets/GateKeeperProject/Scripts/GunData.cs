using UnityEngine;
[CreateAssetMenu(fileName = "WeaponData", menuName = "VR/Weapon Data")]
public class GunData : ScriptableObject
{
    [Header("Type")]
    public bool isAutoGun = true;
    public WeaponType type = WeaponType.Pistol;
    [Header("Damage")]
    public float flatDamage = 20f;
    public float headshotMultiplier = 2f;
    public float range = 100f;
    public float penetration = 1f;
    public float damagePenetrationReduction = 0.7f;

    [Header("Fire Settings")]
    public float fireRate = 5f; 
    public float recoil = 2f;
    public float recoilReduction = 0.5f;
    public float maximumRecoil = 10f;
    public float recoilRecoveryTime = 0.5f;
    public float recoilRecoverySpeed = 8f;

    [Header("Ammo")]
    public int magazineSize = 30;
    public float reloadTime = 2f;
    [Range(0,100)]
    public float freeAmmoPercent = 0f;

    [Header("Bullet")]
    public float bulletSize = 0.1f;

    [Header("Pellet")]
    public float pelletCount = 1f;
    public float spreadAngle = 5f;

    [Header("Reward")]
    [SerializeField] private float flatDamageReward = 2f;
    [SerializeField] private float headshotMultiplierReward = 0.2f;
    [SerializeField] private float rangeReward = 10f;
    [SerializeField] private float penetrationReward = 1f;
    [SerializeField] private float damagePenetrationReductionReward = 0.01f;
    [SerializeField] private float fireRateReward = 1f; 
    [SerializeField] private float recoilReductionReward = 0.05f;
    [SerializeField] private float maximumRecoilReward = 1f;
    [SerializeField] private float recoilRecoveryTimeReward = 0.05f;
    [SerializeField] private float recoilRecoverySpeedReward = 0.8f;
    [SerializeField] private int magazineSizeReward = 3;
    [SerializeField] private float reloadTimeReward = 0.2f;
    [SerializeField] private float freeAmmoPercentReward = 1f;
    [SerializeField] private float bulletSizeReward = 0.01f;
    [SerializeField] private float pelletCountReward = 1f;
    [SerializeField] private float spreadAngleReward = 0.5f;
    [SerializeField] private int ammoReward = 120;
    [Header("Bullet Trail")]
    public GameObject bulletTrailPrefab;
    public float bulletTrailSpeed = 80f;

    public bool CanChangeStat(WeaponStatType stat)
    {
        if (type != WeaponType.Shotgun)
        {
            if (stat == WeaponStatType.PelletCount ||
                stat == WeaponStatType.SpreadAngle)
                return false;
        }

        switch (stat)
        {
            case WeaponStatType.DamagePenetrationReduction:
                if (damagePenetrationReduction - damagePenetrationReductionReward < 0f)
                    return false;
                break;

            case WeaponStatType.RecoilReduction:
                if (recoilReduction - recoilReductionReward > recoil)
                    return false;
                break;

            case WeaponStatType.MaximumRecoil:
                if (maximumRecoil - maximumRecoilReward < 0f)
                    return false;
                break;

            case WeaponStatType.RecoilRecoveryTime:
                if (recoilRecoveryTime - recoilRecoveryTimeReward < 0f)
                    return false;
                break;

            case WeaponStatType.ReloadTime:
                if (reloadTime - reloadTimeReward < 0f)
                    return false;
                break;

            case WeaponStatType.FreeAmmoPercent:
                if (freeAmmoPercent >= 100f)
                    return false;
                break;

            case WeaponStatType.SpreadAngle:
                if (spreadAngle - spreadAngleReward < 0f)
                    return false;
                break;
        }

        return true;
    }

    public float GetRewardValue(WeaponStatType stat)
    {
        switch (stat)
        {
            case WeaponStatType.FlatDamage: return flatDamageReward;
            case WeaponStatType.HeadshotMultiplier: return headshotMultiplierReward;
            case WeaponStatType.Range: return rangeReward;
            case WeaponStatType.Penetration: return penetrationReward;
            case WeaponStatType.DamagePenetrationReduction: return damagePenetrationReductionReward;
            case WeaponStatType.FireRate: return fireRateReward;
            case WeaponStatType.RecoilReduction: return recoilReductionReward;
            case WeaponStatType.MaximumRecoil: return maximumRecoilReward;
            case WeaponStatType.RecoilRecoveryTime: return recoilRecoveryTimeReward;
            case WeaponStatType.RecoilRecoverySpeed: return recoilRecoverySpeedReward;
            case WeaponStatType.MagazineSize: return magazineSizeReward;
            case WeaponStatType.ReloadTime: return reloadTimeReward;
            case WeaponStatType.FreeAmmoPercent: return freeAmmoPercentReward;
            case WeaponStatType.BulletSize: return bulletSizeReward;
            case WeaponStatType.PelletCount: return pelletCountReward;
            case WeaponStatType.SpreadAngle: return spreadAngleReward;
        }

        return 0f;
    }

    public void ApplyReward(WeaponStatType stat)
    {
        if (!CanChangeStat(stat))
            return;

        switch (stat)
        {
            case WeaponStatType.FlatDamage:
                flatDamage += flatDamageReward;
                break;

            case WeaponStatType.HeadshotMultiplier:
                headshotMultiplier += headshotMultiplierReward;
                break;

            case WeaponStatType.Range:
                range += rangeReward;
                break;

            case WeaponStatType.Penetration:
                penetration += penetrationReward;
                break;

            case WeaponStatType.DamagePenetrationReduction:
                damagePenetrationReduction -= damagePenetrationReductionReward;
                break;

            case WeaponStatType.FireRate:
                fireRate += fireRateReward;
                break;

            case WeaponStatType.RecoilReduction:
                recoilReduction += recoilReductionReward;
                break;

            case WeaponStatType.MaximumRecoil:
                maximumRecoil -= maximumRecoilReward;
                break;

            case WeaponStatType.RecoilRecoveryTime:
                recoilRecoveryTime -= recoilRecoveryTimeReward;
                break;

            case WeaponStatType.RecoilRecoverySpeed:
                recoilRecoverySpeed += recoilRecoverySpeedReward;
                break;

            case WeaponStatType.MagazineSize:
                magazineSize += magazineSizeReward;
                break;

            case WeaponStatType.ReloadTime:
                reloadTime -= reloadTimeReward;
                break;

            case WeaponStatType.FreeAmmoPercent:
                freeAmmoPercent += freeAmmoPercentReward;
                break;

            case WeaponStatType.BulletSize:
                bulletSize += bulletSizeReward;
                break;

            case WeaponStatType.PelletCount:
                pelletCount += pelletCountReward;
                break;

            case WeaponStatType.SpreadAngle:
                spreadAngle += spreadAngleReward;
                break;
        }
    }

    public int GetAmmoReward()
    {
        return ammoReward;
    }
}
