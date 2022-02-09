using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    public float speed = 3.5f;
    private Rigidbody2D rb;

    float checkDist = 0.5f;

    public enum MoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public MoveDirection moveDirection = MoveDirection.DOWN;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        //Update Noah's direction based on velocity
        UpdateDirection();

        //Update Noah's animation
        UpdateAnimation();
    }

    private void UpdateDirection()
    {
        if (rb.velocity.x / speed > 0.5f)
        {
            //print("Walking RIGHT");
            moveDirection = MoveDirection.RIGHT;
        }
        else if (rb.velocity.x / speed < -0.5f)
        {
            //print("Walking LEFT");
            moveDirection = MoveDirection.LEFT;
        }
        else if (rb.velocity.y / speed > 0.5f)
        {
            //print("Walking UP");
            moveDirection = MoveDirection.UP;
        }
        else if (rb.velocity.y / speed < -0.5f)
        {
            //print("Walking DOWN");
            moveDirection = MoveDirection.DOWN;
        }

        //print(Mathf.Sqrt ((rb.velocity.x * rb.velocity.x) + (rb.velocity.y * rb.velocity.y)));
    }

    private void UpdateAnimation()
    {
        //Animation States
        //1 = Up idle
        //2 = Up walk
        //3 = Down idle
        //4 = Down walk
        //5 = Left idle
        //6 = Left walk
        //7 = Right idle
        //8 = Right walk
        switch (moveDirection)
        {
            case MoveDirection.UP:
                if (rb.velocity == Vector2.zero)
                {
                    anim.SetInteger("animState", 1);
                }
                else
                {
                    anim.SetInteger("animState", 2);
                }
                break;
            case MoveDirection.DOWN:
                if (rb.velocity == Vector2.zero)
                {
                    anim.SetInteger("animState", 3);
                }
                else
                {
                    anim.SetInteger("animState", 4);
                }
                break;
            case MoveDirection.LEFT:
                if (rb.velocity == Vector2.zero)
                {
                    anim.SetInteger("animState", 5);
                }
                else
                {
                    anim.SetInteger("animState", 6);
                }
                break;
            case MoveDirection.RIGHT:
                if (rb.velocity == Vector2.zero)
                {
                    anim.SetInteger("animState", 7);
                }
                else
                {
                    anim.SetInteger("animState", 8);
                }
                break;
            default:
                break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkDist);
    }

}
