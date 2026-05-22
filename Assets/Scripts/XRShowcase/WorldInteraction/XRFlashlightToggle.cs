using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRFlashlightToggle : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction;

    [Header("Targets")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Light targetLight;

    [Header("State")]
    [SerializeField] private bool startEnabled;

    private bool isEnabled;

    protected override void RegisterInputActions()
    {
        isEnabled = startEnabled;
        ApplyState();
        RegisterButtonAction(toggleAction, OnTogglePerformed);
    }

    public void Toggle()
    {
        isEnabled = !isEnabled;
        ApplyState();
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        Toggle();
    }

    private void ApplyState()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isEnabled);
        }

        if (targetLight != null)
        {
            targetLight.enabled = isEnabled;
        }
    }
}
