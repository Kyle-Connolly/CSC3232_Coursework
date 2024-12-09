using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    public Button newSceneButton;

    void Start()
    {
        // Add listener for button click
        newSceneButton.onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Level 1");
    }
}

