using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RestartGame()
    {
        RunManager.Instance.StartNewRun();

        SceneManager.LoadScene("SampleScene");
    }

    
}
