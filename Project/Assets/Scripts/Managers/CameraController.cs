using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables
    public Camera scopeCam;
    private bool isScoped = false;
    private float scopeCooldown = 2f;
    private float scopeTimer = 0;

    public float mouseSensitivity = 70f;
    public GameObject customCursor;
    public GameObject gameplayHud;
    public Animator fadeAnim;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        scopeCam.depth = -2;
    }

    void Update()
    {
        if (isScoped)
        {
            float x = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            float y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            scopeCam.transform.position += new Vector3(x, y, 0);

            if (Input.GetMouseButtonDown(0) && Time.time >= scopeTimer)
            {
                SetCameraToGame();
            }
        }
        else
        {
            scopeCam.transform.position = transform.position;
        }
    }

    //Button Event
    public void SetCameraToScope()
    {
        if (Time.time >= scopeTimer)
        {
            GameplayManager.gameState = GameplayManager.GameState.SCOPING;
            Cursor.lockState = CursorLockMode.Locked;
            customCursor.SetActive(false);

            //Fade transition to scope
            StartCoroutine(SwapCamera(3f));
        }
    }

    //Enable the camera scope
    private IEnumerator SwapCamera(float fadeDuration)
    {
        fadeAnim.SetInteger("fadeState", 1);
        yield return new WaitForSeconds(fadeDuration / 2);
        fadeAnim.SetInteger("fadeState", 0);

        if (isScoped == false)
        {
            isScoped = true;
            scopeCam.depth = 1;
            gameplayHud.SetActive(false);
        }
        else
        {
            isScoped = false;
            scopeCam.depth = -2;
            Cursor.lockState = CursorLockMode.None;
            customCursor.SetActive(true);
            gameplayHud.SetActive(true);
            GameplayManager.gameState = GameplayManager.GameState.PLAYING;
        }
        scopeTimer = Time.time + scopeCooldown;

        yield return new WaitForSeconds(fadeDuration / 2);
        fadeAnim.SetInteger("fadeState", 0);
    }

    //Disable the cameera scope
    public void SetCameraToGame()
    {
        StartCoroutine(SwapCamera(3f));
    }
}
