using UnityEngine;

public class BoundaryPush : MonoBehaviour
{
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private Transform worldCenter;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (rb == null) return;

        // Richtung zurück zur Mitte
        Vector3 directionToCenter = (worldCenter.position - other.transform.position).normalized;
        rb.AddForce(directionToCenter * pushForce, ForceMode.Acceleration);
    }
}