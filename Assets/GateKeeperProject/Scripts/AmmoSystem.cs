using System;
using System.Collections;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    [SerializeField] private int[] ammos;
    public event Action<WeaponType, int, int> OnAmmoChanged;

    void Start()
    {
        StartCoroutine(InvokeNextFrame());
    }

    private IEnumerator InvokeNextFrame()
    {
        yield return null;
        for (int i = 0; i <= 5; i++)
        {
            OnAmmoChanged?.Invoke((WeaponType)i, 0, ammos[i]);    
        }
    }

    public int GetAmmo(WeaponType type)
    {
        return ammos[(int)type];
    }

    public void IncreaseAmmo(WeaponType type, int amount)
    {
        ammos[(int)type] += amount;
        OnAmmoChanged?.Invoke(type, 0, ammos[(int)type]);
    }

    public void UseAmmo(WeaponType type, int amount)
    {
        int index = (int)type;
        int ammo = Mathf.Min(ammos[index], amount);
        ammos[index] = Mathf.Max(0, ammos[index] - amount);
        OnAmmoChanged?.Invoke(type, ammo, ammos[index]);
    }

}
