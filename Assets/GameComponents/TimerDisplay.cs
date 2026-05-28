using UnityEngine;
using TMPro;

/// <summary>
/// Countdown-Timer, der in der oberen linken Ecke des Sichtfelds angezeigt wird.
/// Läuft der Timer ab, wird GameManager.OnTimeUp() aufgerufen.
///
/// Setup:
///   1. Leeres GameObject "TimerAnchor" als Kind der XR-Kamera (Main Camera) erstellen
///   2. Position: (-0.18, 0.12, 0.5) → obere linke Ecke, 50cm vor der Kamera
///   3. Rotation: (0, 0, 0) relativ zur Kamera
///   4. World Space Canvas als Kind von TimerAnchor erstellen:
///        - Width: 200, Height: 80
///        - Scale: 0.001 / 0.001 / 0.001
///   5. TextMeshPro-Text in den Canvas, dieses Script auf TimerAnchor
///   6. timerText-Feld mit dem TMP-Text verbinden
/// </summary>
public class TimerDisplay : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Spielzeit in Sekunden (300 = 5 Minuten)")]
    [SerializeField] private float totalTime = 300f;

    [Header("Referenzen")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Farben")]
    [SerializeField] private Color normalColor  = Color.white;
    [Tooltip("Farbe wenn weniger als 60 Sekunden übrig")]
    [SerializeField] private Color warningColor = Color.yellow;
    [Tooltip("Farbe wenn weniger als 30 Sekunden übrig")]
    [SerializeField] private Color dangerColor  = Color.red;

    private float timeRemaining;
    private bool  running = true;

    private void Start()
    {
        timeRemaining = totalTime;
        UpdateDisplay();
    }

    private void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            running = false;
            UpdateDisplay();
            GameManager.Instance?.OnTimeUp();
            return;
        }

        UpdateDisplay();
    }

    public void StopTimer() => running = false;

    private void UpdateDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";

        // Farbe je nach verbleibender Zeit
        if (timeRemaining <= 30f)
            timerText.color = dangerColor;
        else if (timeRemaining <= 60f)
            timerText.color = warningColor;
        else
            timerText.color = normalColor;
    }
}