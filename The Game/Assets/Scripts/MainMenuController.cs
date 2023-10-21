using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Canvas _scoreCanvas;
    [SerializeField] private int _sceneIndex = 1;
    
    public void StartGame()
    {
        _scoreCanvas.enabled = true;
        SceneManager.LoadScene(_sceneIndex);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_sceneIndex - 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
