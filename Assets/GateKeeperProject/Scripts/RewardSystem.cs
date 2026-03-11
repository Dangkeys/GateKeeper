using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private GunSystem gunSystem;
    [SerializeField] private AmmoSystem ammoSystem;
    [SerializeField] private Sprint sprint;
    public void GetReward()
    {
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
                sprint.IncreaseSpeed();
                break;
            case 3:
                sprint.IncreaseStamina();
                break;
            default:
                break;
        }
    }

    public void GetWeapon(int weaponIndex, int statIndex)
    {
        GunData gunData = gunSystem.GetGun(weaponIndex).GetGunData();
        WeaponStatType weaponStatType = (WeaponStatType)statIndex;
        gunData.ApplyReward(weaponStatType);
    }

    public void GetAmmo(int ammoIndex)
    {
        // 0 Increase pistol ammo, 1 Increase Assault Rifle ammo, 2 Increase SMG ammo,
        // 3 Increase Shotgun ammo, 4 Increase Sniper ammo, 5 Increase ammo rate drop,
        if(ammoIndex >= 0 && ammoIndex <= 4)
        {
            ammoSystem.IncreaseAmmo((WeaponType)ammoIndex, 100);
        }
        else
        {
            
        }
    }
}
