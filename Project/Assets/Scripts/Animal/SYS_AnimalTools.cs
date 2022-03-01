using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SYS_AnimalTools 
{
    public static Vector2 MoveTowards(Vector2 targetPos, Animal self, float speed)
    {
        Vector2 dir = targetPos - (Vector2)self.transform.position;
        dir = dir.normalized;
        return dir * speed;
    }
}
