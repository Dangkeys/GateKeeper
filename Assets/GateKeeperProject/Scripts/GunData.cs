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
    public float damageReduction = 0.7f;


    [Header("Fire Settings")]
    public float fireRate = 5f; 
    public float recoil = 2f;
    public float recoilReduction = 0.5f;
    public float maximumRecoil = 10f;
    public float RecoilRecoveryTime = 0.5f;

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
}
