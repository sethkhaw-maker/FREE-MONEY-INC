using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject[] bones;

    private void Awake() { if (instance == null) instance = this; }

    public void SpawnBones(Transform animalPos) { Instantiate(bones[Random.Range(0, bones.Length)], animalPos.position, Quaternion.identity); }
}
