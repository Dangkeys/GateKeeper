using UnityEngine;

namespace GateKeeperProject.Scripts
{
    public class Player : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            
            Debug.Log(damage);
           Debug.Log("ouch"); 
        }
    }
}