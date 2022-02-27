using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public static List<Animal> party = new List<Animal>();

    //Variables
    public float speed = 3.5f;
    private Rigidbody2D rb;

    float checkDist = 0.5f;

    private LayerMask animalLayer;
    private LayerMask arkLayer;

    public Animal targetAnimal;

    public Vector2 targetMove;

    bool isClearingAnimals = false;

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
        instance = this;
        animalLayer = LayerMask.GetMask("Animal");
        arkLayer = LayerMask.GetMask("Ark");
        targetMove = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Update Noah's direction based on velocity
        UpdateDirection();

        //Update Noah's animation
        UpdateAnimation();

        if (party.Count == 0)
        {
            isClearingAnimals = false;
        }
        if (GameplayManager.gameState == GameplayManager.GameState.MINIGAME || isClearingAnimals == true)
        {
            targetMove = transform.position;
            rb.velocity = Vector2.zero;
            return;
        }

        //Input mouse clicked, check for animal or move there
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }

        //Chase target if exist
        if (targetAnimal != null)
        {
            //Enter the minigame if player is near the animal
            if (Vector2.Distance(transform.position, targetAnimal.transform.position) < checkDist)
            {
                //Noah has reached destination
                rb.velocity = Vector2.zero;

                //Activate the minigame
                if (GameplayManager.instance != null)
                    GameplayManager.instance.InitMinigame();

                //Reset the target animal to null
                targetMove = transform.position;
                return;
            }

            //Get normalized target direction
            Vector2 dir = targetAnimal.transform.position - transform.position;
            dir = dir.normalized;

            //Move towards target direction
            rb.velocity = dir * speed;
        }
        else
        {
            if (Vector2.Distance(transform.position, targetMove) < checkDist)
            {
                //Noah has reached destination
                rb.velocity = Vector2.zero;
            }
            else
            {
                //Get the vector direction from mousepos to player
                Vector2 dir = targetMove - new Vector2(transform.position.x, transform.position.y);
                dir = dir.normalized;

                rb.velocity = dir * speed;
            }
        }

        //Mouse is held down, Noah moves to mouse position
        if (Input.GetMouseButton(0))
        {
            if (targetAnimal != null) return;

            //Get mouse position in world space
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetMove = mousePos;

            //Return if player is near the mouse
            if (Vector2.Distance(transform.position, mousePos) < checkDist)
            {
                //Noah has reached destination
                rb.velocity = Vector2.zero;
                return;
            }

            //Get the vector direction from mousepos to player
            Vector2 dir = targetMove - new Vector2(transform.position.x, transform.position.y);
            dir = dir.normalized;

            rb.velocity = dir * speed;
        }

        //Testing section
        if (Input.GetKeyDown(KeyCode.F))
        {
            Animal[] animalsInScene = FindObjectsOfType<Animal>();
            foreach (var newAnimal in animalsInScene)
            {
                if (!party.Contains(newAnimal))
                newAnimal.RegisterAnimalToParty();
            }
        }
    }

    //Check for animal clicked
    private void CheckClick()
    {
        //Get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hitArk = Physics2D.Raycast(mousePos, transform.forward, 1f, arkLayer);
        if (hitArk.collider != null)
        {
            GameplayManager.instance.SendAnimalsIntoArk(hitArk.collider.gameObject);
            targetMove = transform.position;
            isClearingAnimals = true;
            return;
        }

        //Raycast to check if hit an animal
        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 1f, animalLayer);

        //Raycast hit an animal
        if (hit.collider != null)
        {
            //Get the animal component from hit
            Animal hitAnimal = hit.collider.GetComponent<Animal>();

            //Check if party contains the animal
            if (party.Contains(hitAnimal)) return;

            //Assign animal to target for chase and minigame
            targetAnimal = hitAnimal;
            targetMove = mousePos;
        }
        else
        {
            //Clicked outside of animal, stopped chasing
            targetAnimal = null;

            targetMove = mousePos;
        }
    }

    //Update direction of moveDirection for animation
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