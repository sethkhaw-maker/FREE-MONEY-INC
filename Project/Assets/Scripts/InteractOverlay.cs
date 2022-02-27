using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOverlay : MonoBehaviour
{
    Material material;
    Sprite sprite;
    public Color ogColor;
    public Color overlayColor;
    
    void Start()
    {
        ogColor = new Color(1, 1, 1, 0);
        material = gameObject.GetComponent<SpriteRenderer>().material;
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        //setting the material
        this.material.SetTexture("_MainTexture", sprite.texture);
        this.material.SetColor("_OverlayColor", ogColor);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetOverlay();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisableOverlay();
        }
    }

    public void SetOverlay()
    {
        this.material.SetColor("_OverlayColor", overlayColor);
    }

    public void DisableOverlay()
    {
        this.material.SetColor("_OverlayColor", ogColor);
    }
}
