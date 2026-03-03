using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;   // Pause Panel
    public GameObject pauseButton;  // Pause button

    public bool IsPaused { get; private set; }

    void Start()
    {
        // Initial state: Panel is off, Pause button is on
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
        IsPaused = false;
    }

    public void OnClickPause()
    {
        // Victory or defeat, there is no room for pause
        if (GameManager.Instance != null && (GameManager.Instance.IsWin || GameManager.Instance.IsLose))
            return;

        Pause();
    }

    public void Pause()
    {
        IsPaused = true;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false); // Hide the button during the pause

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);  // Restore button displayed upon activation
    }
}