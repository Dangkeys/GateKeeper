using UnityEngine;
using VContainer;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] private int recieveAmmo = 10;
    [SerializeField] AmmoSystem ammoSystem;
    [SerializeField] private WeaponType weaponType;

    
    
    public void ChangePosition(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Random.rotation;
        gameObject.SetActive(true);
    }
    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ammoSystem.IncreaseAmmo(weaponType, recieveAmmo);
            gameObject.SetActive(false);
        }
    }
}