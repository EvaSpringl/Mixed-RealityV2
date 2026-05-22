using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class XRSpawnPrefabOnButton : XRInputActionBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference spawnAction;

    [Header("Spawn")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform parentForSpawnedObjects;
    [SerializeField] private bool useSpawnPointRotation = true;

    protected override void RegisterInputActions()
    {
        RegisterButtonAction(spawnAction, OnSpawnPerformed);
    }

    public void Spawn()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"{name}: No prefab assigned.");
            return;
        }

        var point = spawnPoint != null ? spawnPoint : transform;
        var rotation = useSpawnPointRotation ? point.rotation : Quaternion.identity;
        Instantiate(prefabToSpawn, point.position, rotation, parentForSpawnedObjects);
    }

    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        Spawn();
    }
}
