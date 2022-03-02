using UnityEngine;
using UnityEngine.SceneManagement;

//Obsolete, use SceneLoader

public class MainMenu : MonoBehaviour
{
    public AudioClip[] SFXClips;

    public void PlayGame()
    {
        Invoke("Level_1", 2.0f);
        GameObject.Find("BG Music Main Menu").GetComponent<MusicTransition>().PlayButtonSceneChange();
    }
    void Level_1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene(3);
    }
}
