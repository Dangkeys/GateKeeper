using System.Collections.Generic;
using UnityEngine;

namespace GateKeeperProject.Scripts.Enemies
{
    public class EnemyAnimationHandler : MonoBehaviour
    {
        private List<string> LeftHandNameList = new List<string> { "IndexFinger_01", "middle_01_l" };
        private List<string> RightHandNameList = new List<string> { "IndexFinger_01 1", "middle_01_r" };

        // --- NEW METHODS FOR ANIMATION EVENTS ---
        public void EnableLeftHand() => ActivateHand(true, EnemyHandType.Left);
        public void DisableLeftHand() => ActivateHand(false, EnemyHandType.Left);

        public void EnableRightHand() => ActivateHand(true, EnemyHandType.Right);
        public void DisableRightHand() => ActivateHand(false, EnemyHandType.Right);
        // ----------------------------------------

        private void ActivateHand(bool active, EnemyHandType handType)
        {
            GameObject hand = null;
            switch (handType)
            {
                case EnemyHandType.Left:
                    hand = Enemy.RecursiveFindChild(transform, LeftHandNameList);
                    break;
                case EnemyHandType.Right:
                    hand = Enemy.RecursiveFindChild(transform, RightHandNameList);
                    break;
                default:
                    return;
            }

            if (hand != null)
            {
                var collider = hand.GetComponentInChildren<CapsuleCollider>(true);

                if (collider != null)
                {
                    Debug.Log("Found Collider: " + collider.name + " | Setting active: " + active);
                    collider.gameObject.SetActive(active);
                }
                else
                {
                    Debug.LogWarning($"CapsuleCollider not found on {hand.name} or its children!");
                }
            }
        }
    }
}