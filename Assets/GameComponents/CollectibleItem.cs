using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Dieses Script kommt auf jedes sammelbare Objekt (Münze, Muschel, …)
/// zusammen mit XRGrabInteractable.
/// 
/// Sobald das Objekt in die Tasche gelegt wird (= Trigger-Zone der Tasche),
/// meldet es sich beim BagController und verschwindet.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class CollectibleItem : MonoBehaviour
{
    // Verhindert Doppel-Einsammeln (z.B. wenn Physik das Objekt mehrfach
    // durch den Trigger bewegt)
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Prüfen ob der Collider zur Tasche gehört
        BagController bag = other.GetComponent<BagController>();
        if (bag == null) return;

        collected = true;
        bag.CollectItem(gameObject);
    }
}