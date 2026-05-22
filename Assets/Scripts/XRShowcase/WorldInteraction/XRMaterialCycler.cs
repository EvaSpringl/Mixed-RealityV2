using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRMaterialCycler : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference cycleAction;

    [Header("Targets")]
    [SerializeField] private Renderer[] targetRenderers;
    [SerializeField] [Min(0)] private int materialSlotIndex;

    [Header("Materials")]
    [SerializeField] private Material[] materials;
    [SerializeField] private bool useSharedMaterials;

    private int currentIndex;

    protected override void RegisterInputActions()
    {
        ApplyCurrentMaterial();
        RegisterButtonAction(cycleAction, OnCyclePerformed);
    }

    public void CycleMaterial()
    {
        if (materials == null || materials.Length == 0)
        {
            return;
        }

        currentIndex = (currentIndex + 1) % materials.Length;
        ApplyCurrentMaterial();
    }

    private void OnCyclePerformed(InputAction.CallbackContext context)
    {
        CycleMaterial();
    }

    private void ApplyCurrentMaterial()
    {
        if (materials == null || materials.Length == 0)
        {
            return;
        }

        var materialToApply = materials[currentIndex];

        foreach (var targetRenderer in targetRenderers)
        {
            if (targetRenderer == null)
            {
                continue;
            }

            var rendererMaterials = useSharedMaterials ? targetRenderer.sharedMaterials : targetRenderer.materials;
            if (materialSlotIndex >= rendererMaterials.Length)
            {
                Debug.LogWarning($"{name}: Renderer {targetRenderer.name} has no material slot {materialSlotIndex}.");
                continue;
            }

            rendererMaterials[materialSlotIndex] = materialToApply;

            if (useSharedMaterials)
            {
                targetRenderer.sharedMaterials = rendererMaterials;
            }
            else
            {
                targetRenderer.materials = rendererMaterials;
            }
        }
    }
}
