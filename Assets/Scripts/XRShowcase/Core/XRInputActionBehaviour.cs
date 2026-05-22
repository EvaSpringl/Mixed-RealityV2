using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public abstract class XRInputActionBehaviour : MonoBehaviour
{
    private readonly List<ActionRegistration> actionRegistrations = new();

    protected virtual void OnEnable()
    {
        RegisterInputActions();
    }

    protected virtual void OnDisable()
    {
        UnregisterInputActions();
    }

    protected abstract void RegisterInputActions();

    protected void RegisterButtonAction(
        InputActionReference actionReference,
        Action<InputAction.CallbackContext> performedHandler,
        Action<InputAction.CallbackContext> canceledHandler = null)
    {
        var action = actionReference != null ? actionReference.action : null;
        if (action == null)
        {
            return;
        }

        action.performed += performedHandler;

        if (canceledHandler != null)
        {
            action.canceled += canceledHandler;
        }

        action.Enable();
        actionRegistrations.Add(new ActionRegistration(action, performedHandler, canceledHandler));
    }

    protected InputAction GetAction(InputActionReference actionReference)
    {
        var action = actionReference != null ? actionReference.action : null;
        if (action != null)
        {
            action.Enable();
        }

        return action;
    }

    protected static string GetThumbstickDirection(Vector2 value, float deadzone = 0.35f)
    {
        if (value.magnitude < deadzone)
        {
            return "Center";
        }

        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            return value.x > 0f ? "Right" : "Left";
        }

        return value.y > 0f ? "Up" : "Down";
    }

    private void UnregisterInputActions()
    {
        foreach (var registration in actionRegistrations)
        {
            registration.Action.performed -= registration.PerformedHandler;

            if (registration.CanceledHandler != null)
            {
                registration.Action.canceled -= registration.CanceledHandler;
            }
        }

        actionRegistrations.Clear();
    }

    private readonly struct ActionRegistration
    {
        public ActionRegistration(
            InputAction action,
            Action<InputAction.CallbackContext> performedHandler,
            Action<InputAction.CallbackContext> canceledHandler)
        {
            Action = action;
            PerformedHandler = performedHandler;
            CanceledHandler = canceledHandler;
        }

        public InputAction Action { get; }

        public Action<InputAction.CallbackContext> PerformedHandler { get; }

        public Action<InputAction.CallbackContext> CanceledHandler { get; }
    }
}
