using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Bone : MonoBehaviour
{
    [Range(0, 20)]
    public float speed = 0f;
    [Range(0, 5)]
    public float RotMultiplier = 1;
 
    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;
        var Bone = transform.GetChild(0);
        float Rot = 5;
        Bone = Bone.GetChild(0);
        do
        {
            Bone.rotation = Quaternion.Euler(0, 0, 90 + Rot * Mathf.Cos(speed) * RotMultiplier);
            Rot += Rot * 0.5f;

            if (Bone.childCount == 0) break;
            Bone = Bone.GetChild(0);

        } while (true);
        
    }
}
