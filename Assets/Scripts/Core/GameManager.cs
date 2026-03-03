using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool IsGameOver { get; private set; }

    [Header("Goal")]
    public int totalMushrooms = 3;
    public int collected = 0;

    [Header("Timer")]
    public float timeLimit = 60f;
    private float timeLeft;
    public TMP_Text timeText;

    [Header("UI")]
    public TMP_Text statusText;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Control Lock")]
    public MonoBehaviour playerMoveScript;   // PlayerMove
    public MonoBehaviour cameraScript;       // ThirdPersonCamera

    [Header("UI Extra")]
    public GameObject pauseButton; 

    public bool IsWin { get; private set; }
    public bool IsLose { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Time.timeScale = 1f;
        IsGameOver = false;
        IsWin = false;
        IsLose = false;
    }

    private void Start()
    {
        timeLeft = timeLimit;

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateStatusText();
        UpdateTimeText();

        LockCursor(false);
    }

    private void Update()
    {
        // The countdown will no longer be displayed once the game is over
        if (IsGameOver) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        UpdateTimeText();

        if (timeLeft <= 0f)
        {
            Lose("Time Up");
        }
    }

    public void AddMushroom()
    {
        if (IsGameOver) return;

        collected++;
        if (collected > totalMushrooms) collected = totalMushrooms;
        UpdateStatusText();
    }

    public void Win()
    {
        if (IsGameOver) return;

        IsWin = true;
        IsGameOver = true;

        if (winPanel != null) winPanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);
        EndControlAndPause();
    }

    public void Lose(string reason)
    {
        if (IsGameOver) return;

        IsLose = true;
        IsGameOver = true;

        Debug.Log("Lose: " + reason);

        if (losePanel != null) losePanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);
        EndControlAndPause();
    }

    private void EndControlAndPause()
    {
        if (playerMoveScript != null) playerMoveScript.enabled = false;
        if (cameraScript != null) cameraScript.enabled = false;

        Time.timeScale = 0f;

        LockCursor(false);
    }

    public bool HasEnoughMushrooms => collected >= totalMushrooms;

    public void Restart()
    {
        // Recovery time before reopening
        Time.timeScale = 1f;
        IsGameOver = false;
        IsWin = false;
        IsLose = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateStatusText()
    {
        if (statusText != null)
            statusText.text = $"Mushroom: {collected}/{totalMushrooms}";
    }

    private void UpdateTimeText()
    {
        if (timeText != null)
            timeText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}