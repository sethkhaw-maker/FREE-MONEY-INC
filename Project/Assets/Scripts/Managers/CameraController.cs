using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject scopeLens;
    public Animator fadeAnim;

    private PlayerController player;

    public GameObject arkIndicator;

    private float castDist = 300f;
    public LayerMask animalLayer;
    public GameObject infoBox;
    public Text animalInfo;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        scopeCam.depth = -2;
        infoBox.SetActive(false);
    }

    void Update()
    {
        if (isScoped)
        {
            CheckAnimal();

            float x = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            float y = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            scopeCam.transform.position += new Vector3(x, y, 0);

            //Lock position of binoculars camera to boundaries
            if (scopeCam.transform.position.x < -38)
            {
                scopeCam.transform.position = new Vector3(-38f, scopeCam.transform.position.y, scopeCam.transform.position.z);
            }
            else if (scopeCam.transform.position.x > 38)
            {
                scopeCam.transform.position = new Vector3(38f, scopeCam.transform.position.y, scopeCam.transform.position.z);
            }

            if (scopeCam.transform.position.y < -36)
            {
                scopeCam.transform.position = new Vector3(scopeCam.transform.position.x, -36, scopeCam.transform.position.z);
            }
            else if (scopeCam.transform.position.y > 37)
            {
                scopeCam.transform.position = new Vector3(scopeCam.transform.position.x, 37, scopeCam.transform.position.z);
            }

            //print(scopeCam.transform.position.x);

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
        fadeAnim.SetInteger("fadeState", 2);
        yield return new WaitForSeconds(fadeDuration / 2);
        fadeAnim.SetInteger("fadeState", 1);

        if (isScoped == false)
        {
            isScoped = true;
            scopeCam.depth = 1;
            gameplayHud.SetActive(false);
            arkIndicator.SetActive(false);
            scopeLens.SetActive(true);

        }
        else
        {
            isScoped = false;
            scopeCam.depth = -2;
            Cursor.lockState = CursorLockMode.None;
            customCursor.SetActive(true);
            gameplayHud.SetActive(true);
            arkIndicator.SetActive(true);
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
        PlaySFX();
        scopeLens.SetActive(false);
    }

    public void PlaySFX()
    {
        FindObjectOfType<AudioManager>()?.Play("Binocular SFX");
    }

    public void CheckAnimal()
    {
        RaycastHit2D hit = Physics2D.Raycast(scopeCam.transform.position, scopeCam.transform.forward, castDist, animalLayer);
        if (hit.collider != null)
        {
            //print(hit.transform.name);

            string hitName = hit.transform.GetComponent<Animal>().animalName;
            string hitType = hit.transform.GetComponent<Animal>().animalType.ToString();

            infoBox.SetActive(true);
            animalInfo.text = "Animal : " + hitName + "\n" + "Type : " + hitType;
        }
        else
        {
            infoBox.SetActive(false);
        }
    }

}
