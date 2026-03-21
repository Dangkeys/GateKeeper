using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Header("Gun References")]
    [SerializeField] private Gun[] guns;
    [Header("Hand Transforms")]
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform keepWeapon;
    private WeaponType currentRightWeapon = WeaponType.None;
    private WeaponType currentLeftWeapon = WeaponType.None;

    public void EquipRight(int weaponIndex)
    {
        WeaponType weapon = (WeaponType)weaponIndex;
        GetGunOut(weapon, HandType.Right);
    }

    public void EquipLeft(int weaponIndex)
    {
        WeaponType weapon = (WeaponType)weaponIndex;
        GetGunOut(weapon, HandType.Left);
    }

    private void GetGunOut(WeaponType weapon, HandType currentHandType)
    {
        if (currentHandType == HandType.Right && currentLeftWeapon == weapon)
        {
            SwapWeapons();
        }
        else if (currentHandType == HandType.Left && currentRightWeapon == weapon)
        {
            SwapWeapons();
        }
        else if (currentHandType == HandType.Right)
        {
            if (currentRightWeapon != WeaponType.None)
            {
                KeepGunAway(currentRightWeapon);
            }
            SetGunToHand(weapon, rightHandTransform, HandType.Right);
            currentRightWeapon = weapon;
        }
        else if (currentHandType == HandType.Left)
        {
            if (currentLeftWeapon != WeaponType.None)
            {
                KeepGunAway(currentLeftWeapon);
            }
            SetGunToHand(weapon, leftHandTransform, HandType.Left);
            currentLeftWeapon = weapon;
        }
    }

    private void KeepGunAway(WeaponType weapon)
    {
        SetGunToHand(weapon, keepWeapon, HandType.None);
    }

    private void SetGunToHand(WeaponType weapon, Transform hand, HandType handType)
    {
        GetGun((int)weapon).transform.SetParent(hand);
        GetGun((int)weapon).transform.localPosition = Vector3.zero;
        GetGun((int)weapon).transform.localRotation = Quaternion.identity;
        GetGun((int)weapon).SetCurrentHandType(handType);
    }

    private void SwapWeapons()
    {
        WeaponType temp = currentLeftWeapon;
        currentLeftWeapon = currentRightWeapon;
        currentRightWeapon = temp;
        SetGunToHand(currentLeftWeapon, leftHandTransform, HandType.Left);
        SetGunToHand(currentRightWeapon, rightHandTransform, HandType.Right);
    }

    public Gun GetGun(int index)
    {
        return guns[index];
    }
}