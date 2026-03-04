using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private GunSystem gunSystem;
    [SerializeField] private AmmoSystem ammoSystem;
    public void GetReward()
    {
        // Stat, Weapon, Ammo
        RandomStat();
        RandomWeapon();
        RandomAmmo();
    }

    private void RandomStat()
    {
        int statIndex = Random.Range(0, 4);
        Debug.Log("Random Stat : " + statIndex.ToString());
    }

    private void RandomWeapon()
    {
        int weaponIndex = Random.Range(0, 5);
        GunData gunData = gunSystem.GetGun(weaponIndex).GetGunData();
        WeaponStatType weaponStatType;
        do
        {
            int statIndex = Random.Range(0, 16);
            weaponStatType = (WeaponStatType)statIndex;
        }
        while(!gunData.CanChangeStat(weaponStatType));
        
        float reward = gunData.GetRewardValue(weaponStatType);

        Debug.Log("Random Weapon : " + gunData.type + " : " + reward);
    }

    private void RandomAmmo()
    {
        int ammoIndex = Random.Range(0, 5);
        if(ammoIndex < 5)
        {
            GunData gunData = gunSystem.GetGun(ammoIndex).GetGunData();
            int ammo = gunData.GetAmmoReward();
            Debug.Log("Increase Ammo : " + gunData.type + " : " + ammo.ToString());
        }
        else
        {
            Debug.Log("Increase Ammo rate");
        }
    }

    public void GetStat(int statIndex)
    {
        // 0 Reduce Damage Taken, 1 Max Health, 2 Move Speed, 3 Increase stamina
        switch(statIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public void GetWeapon(int weaponIndex, int statIndex)
    {
        // 0 Flat Damage, 1 headshot multiply, 2 Range, 3 Penetration,
        // 4 damagePenetrationReduction, 5 Firerate, 6 RecoilReduction,
        // 7 maximumrecoil, 8 recoilrecoverytime, 9 recoilrecoveryspeed,
        // 10 magazineSize, 11 reloadTime, 12 freeAmmoPercent, 13 bulletSize, 
        // 14 pellet count, 15 spread angle  
        GunData gunData = gunSystem.GetGun(weaponIndex).GetGunData();
        WeaponStatType weaponStatType = (WeaponStatType)statIndex;
        gunData.ApplyReward(weaponStatType);
    }

    public void GetAmmo(int ammoIndex)
    {
        // 0 Increase pistol ammo, 1 Increase Assault Rifle ammo, 2 Increase SMG ammo,
        // 3 Increase Shotgun ammo, 4 Increase Sniper ammo, 5 Increase ammo rate drop,
        switch(ammoIndex)
        {
            case 0 - 4:
                ammoSystem.IncreaseAmmo((WeaponType)ammoIndex, 100);
                break;
            case 5:
                break;
            default:
                break;
        }
    }
}
