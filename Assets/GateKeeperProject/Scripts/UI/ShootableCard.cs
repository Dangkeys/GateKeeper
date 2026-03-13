using UnityEngine;
using UnityEngine.Events;

public class ShootableCard : MonoBehaviour
{
    [Tooltip("What happens when the player shoots this card?")]
    public UnityEvent OnShot;

    public void TriggerCard()
    {
        Debug.Log("Trigger Card");
        OnShot?.Invoke();
    }
}