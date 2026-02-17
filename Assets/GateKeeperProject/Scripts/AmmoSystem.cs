using System;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    [SerializeField] private int[] ammos;
    public event Action<WeaponType, int> OnAmmoChanged;

    void Awake()
    {
        ammos = new int[Enum.GetNames(typeof(WeaponType)).Length];
    }

    public int GetAmmo(WeaponType type)
    {
        return ammos[(int)type];
    }

    public void IncreaseAmmo(WeaponType type, int amount)
    {
        ammos[(int)type] += amount;
        OnAmmoChanged?.Invoke(type, ammos[(int)type]);
    }

    public int UseAmmo(WeaponType type, int amount)
    {
        int index = (int)type;
        int ammo = Mathf.Min(ammos[index], amount);
        ammos[index] = Mathf.Max(0, ammos[index] - amount);
        OnAmmoChanged?.Invoke(type, ammos[index]);
        return ammo;
    }

}
