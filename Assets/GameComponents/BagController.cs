using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Verwaltet die Sammeltasche des Spielers.
/// Bei Spielende wird GameManager.OnGameComplete() aufgerufen.
/// </summary>
public class BagController : MonoBehaviour
{
    [Header("Spielziel")]
    [Tooltip("Wie viele Objekte müssen eingesammelt werden?")]
    [SerializeField] private int requiredItems = 5;

    [Header("UI (optional)")]
    [Tooltip("TextMeshPro-Text der den Punktestand anzeigt, z.B. '2 / 5'")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Events")]
    [Tooltip("Wird aufgerufen wenn ein Objekt eingesammelt wird (int = aktueller Stand)")]
    public UnityEvent<int> onItemCollected;

    private int collectedCount = 0;

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Wird von CollectibleItem aufgerufen, wenn das Objekt in die Tasche kommt.
    /// </summary>
    public void CollectItem(GameObject item)
    {
        collectedCount++;

        // XRGrabInteractable deaktivieren damit das Objekt losgelassen wird
        var grab = item.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null) grab.enabled = false;

        Destroy(item);
        UpdateUI();

        onItemCollected?.Invoke(collectedCount);

        // Gewonnen?
        if (collectedCount >= requiredItems)
            GameManager.Instance?.OnGameComplete();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"{collectedCount} / {requiredItems}";
    }

    public int CollectedCount => collectedCount;
    public int RequiredItems  => requiredItems;
    public bool IsComplete    => collectedCount >= requiredItems;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        var col = GetComponent<Collider>();
        if (col is SphereCollider sc)
            Gizmos.DrawSphere(transform.position, sc.radius * transform.lossyScale.x);
        else if (col is BoxCollider bc)
            Gizmos.DrawCube(transform.position, Vector3.Scale(bc.size, transform.lossyScale));
    }
}