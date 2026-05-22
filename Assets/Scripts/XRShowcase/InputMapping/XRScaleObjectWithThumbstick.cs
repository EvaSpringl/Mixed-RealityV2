using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRScaleObjectWithThumbstick : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference thumbstickAction;

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Scaling")]
    [SerializeField] private bool scaleUniformly = true;
    [SerializeField] private Vector3 scaleAxis = Vector3.one;
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float minScale = 0.25f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] [Range(0f, 1f)] private float deadzone = 0.25f;

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
        if (Mathf.Abs(input.y) < deadzone)
        {
            return;
        }

        var scaleDelta = input.y * scaleSpeed * Time.deltaTime;

        if (scaleUniformly)
        {
            var newUniformScale = Mathf.Clamp(target.localScale.x + scaleDelta, minScale, maxScale);
            target.localScale = Vector3.one * newUniformScale;
            return;
        }

        var desiredScale = target.localScale + scaleAxis.normalized * scaleDelta;
        target.localScale = new Vector3(
            Mathf.Clamp(desiredScale.x, minScale, maxScale),
            Mathf.Clamp(desiredScale.y, minScale, maxScale),
            Mathf.Clamp(desiredScale.z, minScale, maxScale));
    }
}
