using System.ComponentModel;
using UnityEngine;

public class AmmoDropSystem : MonoBehaviour
{
    [SerializeField] private AmmoDrop[] ammoDropPrefabs;
    [SerializeField] private int ammoSpawnAmount = 100;
    private AmmoDrop[] ammoDrops;
    [SerializeField] private Transform ammoTransform;
    private int maximumIndex;

    void Start()
    {
        maximumIndex = ammoDropPrefabs.Length * ammoSpawnAmount;
        ammoDrops = new AmmoDrop[maximumIndex];
        int index = 0;
        foreach(AmmoDrop ammo in ammoDropPrefabs)
        {
            for(int i = 0; i < ammoSpawnAmount; i++)
            {
                ammoDrops[index] = Instantiate(ammo, ammoTransform);
                index++;
            }
        }
    }

    public void SpawnAmmo(Vector3 position)
    {
        int index;
        int counter = 0;
        do
        {
            
            index = Random.Range(0, maximumIndex);
            counter++;
        }
        while(ammoDrops[index].GetActive() && counter < maximumIndex);
        ammoDrops[index].ChangePosition(position);
    }
}
