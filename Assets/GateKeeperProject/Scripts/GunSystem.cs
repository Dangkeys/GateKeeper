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

    public void UnequipRight()
    {
        if (currentRightWeapon == WeaponType.None) return;

        KeepGunAway(currentRightWeapon);
        currentRightWeapon = WeaponType.None;
    }

    public void UnequipLeft()
    {
        if (currentLeftWeapon == WeaponType.None) return;

        KeepGunAway(currentLeftWeapon);
        currentLeftWeapon = WeaponType.None;
    }

    private void GetGunOut(WeaponType weapon, HandType curretnHandType)
    {
        if (currentLeftWeapon == weapon)
        {
            KeepGunAway(currentLeftWeapon);
            currentLeftWeapon = WeaponType.None;
        }

        if (currentRightWeapon == weapon)
        {
            KeepGunAway(currentRightWeapon);
            currentRightWeapon = WeaponType.None;
        }

        if (curretnHandType == HandType.Right)
        {
            if (currentRightWeapon != WeaponType.None)
                KeepGunAway(currentRightWeapon);

            SetGunToHand(weapon, rightHandTransform, HandType.Right);
            currentRightWeapon = weapon;
        }
        else if (curretnHandType == HandType.Left)
        {
            if (currentLeftWeapon != WeaponType.None)
                KeepGunAway(currentLeftWeapon);

            SetGunToHand(weapon, leftHandTransform, HandType.Left);
            currentLeftWeapon = weapon;
        }
    }

    private void KeepGunAway(WeaponType weapon)
    {
        guns[(int)weapon].transform.SetParent(keepWeapon);
        guns[(int)weapon].transform.localPosition = Vector3.zero;
        guns[(int)weapon].transform.localRotation = Quaternion.identity;
        guns[(int)weapon].SetCurrentHandType(HandType.None);
    }

    private void SetGunToHand(WeaponType weapon, Transform hand, HandType handType)
    {
        guns[(int)weapon].transform.SetParent(hand);
        guns[(int)weapon].transform.localPosition = Vector3.zero;
        guns[(int)weapon].transform.localRotation = Quaternion.identity;
        guns[(int)weapon].SetCurrentHandType(handType);
    }

    public Gun GetGun(int index)
    {
        return guns[index];
    }
}