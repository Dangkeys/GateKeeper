using UnityEngine;

namespace GateKeeperProject.Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (mainCamera != null)
            {
                transform.LookAt(mainCamera.transform);
                transform.Rotate(0, 180, 0);
            }
        }
    }
}