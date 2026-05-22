using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRButtonLogger : XRInputActionBehaviour
{
    [Header("Controller Labels")]
    [SerializeField] private string controllerLabel = "Right Controller";
    [SerializeField] private string primaryButtonLabel = "A / X";
    [SerializeField] private string secondaryButtonLabel = "B / Y";

    [Header("Assign Input Actions In Inspector")]
    [SerializeField] private InputActionReference triggerAction;
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference primaryButtonAction;
    [SerializeField] private InputActionReference secondaryButtonAction;
    [SerializeField] private InputActionReference thumbstickAction;
    [SerializeField] private InputActionReference thumbstickClickAction;

    [Header("Logging")]
    [SerializeField] private bool logThumbstickContinuously = true;
    [SerializeField] [Range(0f, 1f)] private float thumbstickDeadzone = 0.35f;

    [Header("Optional Inspector Events")]
    [SerializeField] private UnityEvent onTriggerPressed;
    [SerializeField] private UnityEvent onTriggerReleased;
    [SerializeField] private UnityEvent onGripPressed;
    [SerializeField] private UnityEvent onGripReleased;
    [SerializeField] private UnityEvent onPrimaryPressed;
    [SerializeField] private UnityEvent onPrimaryReleased;
    [SerializeField] private UnityEvent onSecondaryPressed;
    [SerializeField] private UnityEvent onSecondaryReleased;
    [SerializeField] private UnityEvent onThumbstickClicked;
    [SerializeField] private UnityEvent onThumbstickClickReleased;
    [SerializeField] private Vector2Event onThumbstickMoved;
    [SerializeField] private UnityEvent onThumbstickReleased;

    [Serializable]
    public class Vector2Event : UnityEvent<Vector2>
    {
    }

    private string lastThumbstickDirection = "Center";

    protected override void RegisterInputActions()
    {
        RegisterButtonAction(triggerAction, OnTriggerPressed, OnTriggerReleased);
        RegisterButtonAction(gripAction, OnGripPressed, OnGripReleased);
        RegisterButtonAction(primaryButtonAction, OnPrimaryPressed, OnPrimaryReleased);
        RegisterButtonAction(secondaryButtonAction, OnSecondaryPressed, OnSecondaryReleased);
        RegisterButtonAction(thumbstickAction, OnThumbstickMoved, OnThumbstickReleased);
        RegisterButtonAction(thumbstickClickAction, OnThumbstickClicked, OnThumbstickClickReleased);
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Trigger Pressed ({ReadFloat(context):0.00})");
        onTriggerPressed?.Invoke();
    }

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Trigger Released");
        onTriggerReleased?.Invoke();
    }

    private void OnGripPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Grip Pressed ({ReadFloat(context):0.00})");
        onGripPressed?.Invoke();
    }

    private void OnGripReleased(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Grip Released");
        onGripReleased?.Invoke();
    }

    private void OnPrimaryPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} {primaryButtonLabel} Pressed");
        onPrimaryPressed?.Invoke();
    }

    private void OnPrimaryReleased(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} {primaryButtonLabel} Released");
        onPrimaryReleased?.Invoke();
    }

    private void OnSecondaryPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} {secondaryButtonLabel} Pressed");
        onSecondaryPressed?.Invoke();
    }

    private void OnSecondaryReleased(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} {secondaryButtonLabel} Released");
        onSecondaryReleased?.Invoke();
    }

    private void OnThumbstickClicked(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Thumbstick Clicked");
        onThumbstickClicked?.Invoke();
    }

    private void OnThumbstickClickReleased(InputAction.CallbackContext context)
    {
        Debug.Log($"{controllerLabel} Thumbstick Click Released");
        onThumbstickClickReleased?.Invoke();
    }

    private void OnThumbstickMoved(InputAction.CallbackContext context)
    {
        var thumbstickValue = context.ReadValue<Vector2>();
        var direction = GetThumbstickDirection(thumbstickValue, thumbstickDeadzone);

        if (logThumbstickContinuously || direction != lastThumbstickDirection)
        {
            Debug.Log($"{controllerLabel} Thumbstick: {thumbstickValue} ({direction})");
        }

        lastThumbstickDirection = direction;
        onThumbstickMoved?.Invoke(thumbstickValue);
    }

    private void OnThumbstickReleased(InputAction.CallbackContext context)
    {
        lastThumbstickDirection = "Center";
        Debug.Log($"{controllerLabel} Thumbstick Released");
        onThumbstickReleased?.Invoke();
    }

    private static float ReadFloat(InputAction.CallbackContext context)
    {
        return context.control.valueType == typeof(float) ? context.ReadValue<float>() : 1f;
    }
}
