using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGeneratorScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] clouds;
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    GameObject endPoint;

    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        Invoke("AttemptSpawn", spawnInterval);
    }

    void SpawnCloud()
    {
        int randomIndex = Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex]);

        float startY = Random.Range(startPos.y - 2f, startPos.y + 1f);
        cloud.transform.position = new Vector3(startPos.x, Random.Range(-40,45), startPos.z);

        float scale = Random.Range(0.8f, 1.2f);
        cloud.transform.localScale = new Vector2(scale, scale);

        float speed = Random.Range(0.5f, 1.5f);
        cloud.GetComponent<CloudScript>().StartFloating(speed, endPoint.transform.position.x);
    }

    void AttemptSpawn()
    {
        // Allows us to do checks here
        SpawnCloud();

        Invoke("AttemptSpawn", spawnInterval);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
