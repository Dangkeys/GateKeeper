using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Sprint : MonoBehaviour
{
    [SerializeField] private DynamicMoveProvider dynamicMoveProvider;
    [SerializeField] private float initSpeed = 2.5f;
    [SerializeField] private float multiplySpeed = 2f;
    [SerializeField] private float sprintDuration = 10f;
    [SerializeField] private InputActionReference sprintInput;
    [SerializeField] private Scrollbar sprintScrollBar;
    private float currentSprintDuration;
    private bool isSprint;

    void Start()
    {
        dynamicMoveProvider.moveSpeed = initSpeed;
        currentSprintDuration = sprintDuration;
        UpdateSprintScrollBar();
    }

    void OnEnable()
    {
        sprintInput.action.started += TrySprint;
        sprintInput.action.canceled += CancelSprint;
    }

    void OnDisable()
    {
        sprintInput.action.started -= TrySprint;
        sprintInput.action.canceled -= CancelSprint;
    }

    void Update()
    {
        if(isSprint)
        {
            if(currentSprintDuration > 0f)
            {
                currentSprintDuration -= Time.deltaTime;
                UpdateSprintScrollBar();
            }
            else
            {
                isSprint = false;
                currentSprintDuration = 0f;
                dynamicMoveProvider.moveSpeed = initSpeed;
                UpdateSprintScrollBar();
            }
        }
        else
        {
            if(currentSprintDuration < sprintDuration)
            {
                currentSprintDuration += Time.deltaTime;
                UpdateSprintScrollBar();
            }
        }
    }

    private void TrySprint(InputAction.CallbackContext context)
    {
        if(currentSprintDuration > 0f)
        {
            isSprint = true;
            dynamicMoveProvider.moveSpeed = initSpeed * multiplySpeed;
        }
    }

    private void CancelSprint(InputAction.CallbackContext context)
    {
        isSprint = false;
        dynamicMoveProvider.moveSpeed = initSpeed;
    }

    private void UpdateSprintScrollBar()
    {
        sprintScrollBar.size = currentSprintDuration / sprintDuration;
    }

}
