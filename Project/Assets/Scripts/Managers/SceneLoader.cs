using UnityEngine;
using UnityEngine.SceneManagement;

//Should exist in every scene but not as a singleton, cause of buttons hook up
public class SceneLoader : MonoBehaviour
{
    //These functions can be called independently, or also hooked together to button on clicks

    //Change scene based on index
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    //Reload the current scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Change bgm through Audio Manager, transitions inside
    public void ChangeBGM(int bgmIndex)
    {
        FindObjectOfType<AudioManager>()?.ChangeBGM(bgmIndex);
    }

    //Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }


}
