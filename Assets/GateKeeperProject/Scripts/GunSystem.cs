using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Header("Gun References")]
    [SerializeField] private Gun[] guns;
    [Header("Hand Transforms")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform keepWeapon;
    private int rightWeapon = -1;
    private int leftWeapon = -1;

    public void EquipRight(int index)
    {
        GetGunOut(index, true);
    }

    public void EquipLeft(int index)
    {
        GetGunOut(index, false);
    }

    public void UnequipRight()
    {
        if (rightWeapon == -1) return;

        KeepGun(rightWeapon);
        rightWeapon = -1;
    }

    public void UnequipLeft()
    {
        if (leftWeapon == -1) return;

        KeepGun(leftWeapon);
        leftWeapon = -1;
    }

    private void GetGunOut(int index, bool isRight)
    {
        if (index < 0 || index >= guns.Length)
        {
            Debug.LogWarning("Invalid gun index");
            return;
        }

        if (leftWeapon == index)
        {
            KeepGun(leftWeapon);
            leftWeapon = -1;
        }

        if (rightWeapon == index)
        {
            KeepGun(rightWeapon);
            rightWeapon = -1;
        }

        if (isRight)
        {
            if (rightWeapon != -1)
                KeepGun(rightWeapon);

            SetGunToHand(index, rightHand, HandType.Right);
            rightWeapon = index;
        }
        else
        {
            if (leftWeapon != -1)
                KeepGun(leftWeapon);

            SetGunToHand(index, leftHand, HandType.Left);
            leftWeapon = index;
        }
    }

    private void KeepGun(int index)
    {
        guns[index].transform.SetParent(keepWeapon);
        guns[index].transform.localPosition = Vector3.zero;
        guns[index].transform.localRotation = Quaternion.identity;
        guns[index].SetHand(HandType.None);
    }

    private void SetGunToHand(int index, Transform hand, HandType handType)
    {
        guns[index].transform.SetParent(hand);
        guns[index].transform.localPosition = Vector3.zero;
        guns[index].transform.localRotation = Quaternion.identity;
        guns[index].SetHand(handType);
    }
}