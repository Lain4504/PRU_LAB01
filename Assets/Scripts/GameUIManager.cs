using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private TextMeshProUGUI gameOverScoreText; 
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    private bool isPaused = false;

    void Start()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        UpdateScoreDisplay();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameBGMusic();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverCanvas.activeSelf)
        {
            TogglePause();
        }
        UpdateScoreDisplay(); 
    }

    public void TogglePause()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ContinueGame()
    {
        TogglePause();
    }

    public void BackToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ShowGameOver()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGMusic(); 
        }
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);

        gameOverScoreText.text = "Score: " + GameManager.Instance.Score.ToString();

        float difficultyTimer = asteroidSpawner.GetDifficultyTimer();
        float minutes = Mathf.FloorToInt(difficultyTimer / 60f);
        float seconds = Mathf.FloorToInt(difficultyTimer % 60f);
        timeSurvivedText.text = "Time Survived: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        GameManager.Instance.SaveGameData(difficultyTimer);
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.Score.ToString();
        }
    }
}