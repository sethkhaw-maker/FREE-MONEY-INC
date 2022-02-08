using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyCursor : MonoBehaviour
{
    //public static CopyCursor instance;
    private Image rend;
    public Sprite highlightedCursor;
    public Sprite normalCursor;
    public AudioSource CursorAudio;
    public AudioClip[] CursorClips;
    public GameObject clickEffect;
    //public GameObject dontDestroy;
    //public GameObject trailEffect;
    public float timeBtwSpawn = 0.1f;

    IEnumerator StopParticleSystem(GameObject clickeffect, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(clickeffect);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        CursorAudio = GetComponent<AudioSource>();
        rend = GetComponent<Image>();
    }
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(instance);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //void Awake()
    //{
    //    dontDestroy = GameObject.Find("Cursor");
    //    //if (cursor > 1)
    //    //{
    //    //    Destroy(this.gameObject);
    //    //}

    //    DontDestroyOnLoad(dontDestroy);
    //}
    // Update is called once per frame
    void Update()
    {
        
        Vector2 cursorPos = Input.mousePosition;
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0))
        {
            rend.sprite = highlightedCursor;
            GameObject clone = Instantiate(clickEffect, Camera.main.ScreenToWorldPoint(transform.position), Quaternion.identity);
            StartCoroutine(StopParticleSystem(clone, 2f));
            PlayRandom();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            rend.sprite = normalCursor;
        //    //rend.sprite = handCursor;

        }

        if (timeBtwSpawn <= 0)
        {
            //Instantiate(trailEffect, cursorPos, Quaternion.identity);
            //timeBtwSpawn = 0.1f;

        }
        else
        {
            //timeBtwSpawn -= Time.deltaTime;
        }
    }
    void PlayRandom()
    {
        if (!CursorAudio.isPlaying)
        {
            CursorAudio.clip = CursorClips[Random.Range(0, CursorClips.Length)];
            CursorAudio.Play();
        }
        
    }
}

