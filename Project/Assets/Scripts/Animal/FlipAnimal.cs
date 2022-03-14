using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnimal : MonoBehaviour
{
    Animal self;
    private float speed = 10;
    private Vector2 originalSize;
    public int facingRight = 1;
    public bool dontFlip = false;

    public void Init(Animal a)
    {
        self = a;
        float randomStartSize = Random.Range(1f, 1.3f);
        transform.localScale *= randomStartSize;
        originalSize = transform.localScale;
    }

    void Update()
    {
        DecideFlip();
        if (!dontFlip) AnimateFlip();
        AnimateSquashStretch();
    }

    void DecideFlip()
    {
        if (self.rb.velocity == Vector2.zero) return;
        if (self.rb.velocity.x > 0) facingRight = 1;
        if (self.rb.velocity.x < 0) facingRight = -1;
    }

    public void Flip() => facingRight = -facingRight;
    public bool IsFacingRight() => facingRight == 1 ? true : false;
    void AnimateSquashStretch() => transform.localScale = new Vector3(transform.localScale.x, originalSize.y + (Mathf.Sin(Time.time) / 10 * originalSize.y), transform.localScale.z);
    public void AnimateFlip() => transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, originalSize.x * facingRight, Time.deltaTime * speed), transform.localScale.y, transform.localScale.z);
}
