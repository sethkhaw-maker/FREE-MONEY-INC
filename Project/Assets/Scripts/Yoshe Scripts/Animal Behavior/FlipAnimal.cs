using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnimal : MonoBehaviour
{
    private float speed = 10;

    private Vector2 originalSize;

    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        float randomStartSize = Random.Range(1f, 1.3f);
        transform.localScale *= randomStartSize;
        originalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float rnd = Input.GetAxisRaw("Horizontal");

        if (rnd == 1)
        {
            facingRight = true;
        }
        else if (rnd == -1)
        {
            facingRight = false;
        }

        if (facingRight)
        {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, originalSize.x, Time.deltaTime * speed), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, -originalSize.x, Time.deltaTime * speed), transform.localScale.y, transform.localScale.z);
        }

        transform.localScale = new Vector3(transform.localScale.x, originalSize.y + (Mathf.Sin(Time.time)/10*originalSize.y), transform.localScale.z);
    }
}
