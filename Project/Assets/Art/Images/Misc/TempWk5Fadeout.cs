using UnityEngine;

public class TempWk5Fadeout : MonoBehaviour
{
    public GameObject overlay;
    public GameObject audioSource;
    Animator m_anim;

    void Awake()
    {
        m_anim = overlay.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (!overlay.activeSelf) { overlay.SetActive(true); audioSource.SetActive(false); }
            else
            {
                if (PauseMenu.GameIsPaused) { PauseMenu.instance.Resume(); }
                m_anim.SetTrigger("FadeOutTrigger");
                audioSource.SetActive(true);
                Invoke("DeleteSelf", 1.0f);
            }
        }
    }

    void DeleteSelf()
    {
        overlay.SetActive(false);
    }
}
