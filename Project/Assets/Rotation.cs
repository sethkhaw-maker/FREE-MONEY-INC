using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 0.1f;
    private float speedinDegrees;
    public bool antiSpin = false;
    // Start is called before the first frame update
    void Start()
    {
        speedinDegrees = speed * 360f;
        if (antiSpin == true)
        {
            speedinDegrees *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speedinDegrees * Time.deltaTime * 1/10);
    }
}
