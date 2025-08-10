using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject GameOverPanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Вернуть время перед загрузкой
        pauseMenuUI.SetActive(false);
        StartCoroutine(LoadMainMenuAsync());
    }

    public void ShowGameOverPanel()
    {
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }
    private System.Collections.IEnumerator LoadMainMenuAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");

        while (!operation.isDone)
        {
            // Можно показать экран загрузки, прогресс и т.д.
            yield return null;
        }
    }
}
