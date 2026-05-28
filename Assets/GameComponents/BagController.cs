using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Wird auf das Taschen-GameObject gelegt, das am XR Origin (Gürtel des Spielers)
/// befestigt ist. Braucht einen Trigger-Collider, der die Öffnung der Tasche darstellt.
///
/// Setup in Unity:
///   1. Leeres GameObject als Kind von XR Origin erstellen, z.B. "Bag"
///   2. Position: ca. (0.3, -0.3, 0) → rechte Hüfte des Spielers
///   3. Collider (z.B. SphereCollider, Radius 0.15) hinzufügen, "Is Trigger" = true
///   4. Dieses Script hinzufügen
///   5. Optional: TextMeshPro-Text für den Punktestand reinziehen
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
    [Tooltip("Wird aufgerufen wenn ein Objekt eingesammelt wird")]
    public UnityEvent<int> onItemCollected;
    [Tooltip("Wird aufgerufen wenn alle Objekte eingesammelt wurden")]
    public UnityEvent onGameComplete;

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

        // Objekt loslassen falls noch gehalten, dann deaktivieren
        // (XRGrabInteractable hat keine einfache "ForceRelease"-API von außen,
        //  daher deaktivieren wir den Interactable kurz, was automatisch loslässt)
        var grabInteractable = item.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
            grabInteractable.enabled = false;

        // Objekt aus der Welt entfernen
        Destroy(item);

        UpdateUI();

        // Event feuern (z.B. für Sound, Partikel, Haptics)
        onItemCollected?.Invoke(collectedCount);

        // Gewonnen?
        if (collectedCount >= requiredItems)
        {
            onGameComplete?.Invoke();
            HandleGameComplete();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"{collectedCount} / {requiredItems}";
    }

    private void HandleGameComplete()
    {
        Debug.Log("Alle Objekte eingesammelt! Spiel beendet.");
        // Hier kann man z.B. eine Endscreen-UI einblenden oder die Scene wechseln:
        // SceneManager.LoadScene("EndScreen");
        // Oder einfach eine kurze Verzögerung:
        // StartCoroutine(LoadEndScreen());
    }

    // Öffentlicher Getter, falls andere Scripts den Score brauchen
    public int CollectedCount  => collectedCount;
    public int RequiredItems   => requiredItems;
    public bool IsComplete     => collectedCount >= requiredItems;

    // Gizmo: macht die Trigger-Zone im Editor sichtbar
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