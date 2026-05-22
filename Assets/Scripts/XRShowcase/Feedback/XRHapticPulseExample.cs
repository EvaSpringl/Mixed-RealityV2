using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

[DisallowMultipleComponent]
public class XRHapticPulseExample : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference pulseAction;

    [Header("Controller")]
    [SerializeField] private XRNode controllerNode = XRNode.RightHand;

    [Header("Pulse Settings")]
    [SerializeField] [Range(0f, 1f)] private float amplitude = 0.5f;
    [SerializeField] [Min(0.01f)] private float duration = 0.1f;

    protected override void RegisterInputActions()
    {
        RegisterButtonAction(pulseAction, OnPulsePerformed);
    }

    public void Pulse()
    {
        var device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid)
        {
            Debug.LogWarning($"{name}: No XR device found for {controllerNode}.");
            return;
        }

        if (!device.TryGetHapticCapabilities(out var capabilities) || !capabilities.supportsImpulse)
        {
            Debug.LogWarning($"{name}: {controllerNode} does not support haptic impulse.");
            return;
        }

        device.SendHapticImpulse(0u, amplitude, duration);
    }

    private void OnPulsePerformed(InputAction.CallbackContext context)
    {
        Pulse();
    }
}
