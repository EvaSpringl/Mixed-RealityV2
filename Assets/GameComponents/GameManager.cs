using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Zentrale Spielsteuerung: verwaltet Win/Lose-Zustand und die Overlay-Texte.
/// 
/// Setup:
///   1. Leeres GameObject "GameManager" in der Szene erstellen
///   2. Dieses Script hinzufügen
///   3. Win- und GameOver-Canvas (World Space, vor der Kamera) zuweisen
///   4. BagController und TimerDisplay per Inspector verbinden
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referenzen")]
    [SerializeField] private BagController bagController;
    [SerializeField] private TimerDisplay timerDisplay;

    [Header("Overlay-Texte (World Space Canvas vor der Kamera)")]
    [SerializeField] private GameObject winOverlay;       // Canvas mit "Everything collected!"
    [SerializeField] private GameObject gameOverOverlay;  // Canvas mit "Game Over"

    [Header("Einstellungen")]
    [Tooltip("Sekunden bis zum Neustart nach Win oder Game Over")]
    [SerializeField] private float restartDelay = 4f;

    private bool gameEnded = false;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Overlays zu Beginn verstecken
        if (winOverlay)      winOverlay.SetActive(false);
        if (gameOverOverlay) gameOverOverlay.SetActive(false);
    }

    // ── Wird vom BagController via UnityEvent aufgerufen ──────────────────
    public void OnGameComplete()
    {
        if (gameEnded) return;
        gameEnded = true;

        timerDisplay?.StopTimer();

        if (winOverlay) winOverlay.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    // ── Wird vom TimerDisplay aufgerufen wenn die Zeit abläuft ────────────
    public void OnTimeUp()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (gameOverOverlay) gameOverOverlay.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}