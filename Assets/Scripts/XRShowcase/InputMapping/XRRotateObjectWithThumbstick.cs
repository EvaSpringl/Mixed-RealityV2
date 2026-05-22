using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRRotateObjectWithThumbstick : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference thumbstickAction;

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] [Range(0f, 1f)] private float deadzone = 0.25f;
    [SerializeField] private Space rotationSpace = Space.Self;

    private InputAction thumbstick;

    protected override void RegisterInputActions()
    {
        thumbstick = GetAction(thumbstickAction);
    }

    private void Update()
    {
        if (thumbstick == null || target == null)
        {
            return;
        }

        var input = thumbstick.ReadValue<Vector2>();
        if (Mathf.Abs(input.x) < deadzone)
        {
            return;
        }

        var rotationStep = input.x * rotationSpeed * Time.deltaTime;
        target.Rotate(rotationAxis.normalized, rotationStep, rotationSpace);
    }
}
