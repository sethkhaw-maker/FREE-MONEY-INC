using UnityEngine;

public class ActiveOnAwake : MonoBehaviour
{
    void Awake()
    {
        GetComponent<UnityEngine.UI.Image>().color = Color.black;
    }
}
