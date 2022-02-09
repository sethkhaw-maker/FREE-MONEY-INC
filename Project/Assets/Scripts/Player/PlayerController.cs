using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    public float speed = 5f;
    private Rigidbody2D rb;

    float checkDist = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse is held down, Noah moves to mouse position
        if (Input.GetMouseButton(0))
        {
            //Get mouse position in world space
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Return if player is near the mouse
            if (Vector2.Distance(transform.position, mousePos) < checkDist)
            {
                //Noah has reached destination
                rb.velocity = Vector2.zero;
                return;
            }

            //Get the vector direction from mousepos to player
            Vector2 dir = mousePos - new Vector2(transform.position.x, transform.position.y);
            dir = dir.normalized;

            rb.velocity = dir * speed;
        }
        else
        {
            //Mouse is not held down, Noah stops moving
            rb.velocity = Vector2.zero;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkDist);
    }

}
