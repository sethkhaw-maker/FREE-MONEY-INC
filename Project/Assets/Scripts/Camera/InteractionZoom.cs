using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InteractionZoom : MonoBehaviour
{
    //Variables
    CinemachineVirtualCamera cmCam;
    private float minigameZoomSize = 3.5f;
    private float overworldZoomSize = 7f;
    private float zoomSpeed = 2f;
    private float scrollSpeed = 5f;

    void Start()
    {
        cmCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        overworldZoomSize += -Input.mouseScrollDelta.y;
        overworldZoomSize = Mathf.Clamp(overworldZoomSize, minigameZoomSize, 10f);

        float step = zoomSpeed * Time.deltaTime;
        switch (GameplayManager.gameState)
        {
            case GameplayManager.GameState.PLAYING:
                cmCam.m_Lens.OrthographicSize = Mathf.Lerp(cmCam.m_Lens.OrthographicSize, overworldZoomSize, step);
                break;
            case GameplayManager.GameState.MINIGAME:
                cmCam.m_Lens.OrthographicSize = Mathf.Lerp(cmCam.m_Lens.OrthographicSize, minigameZoomSize, step);
                break;
            case GameplayManager.GameState.PAUSED:
                break;
            default:
                break;
        }
    }
}
