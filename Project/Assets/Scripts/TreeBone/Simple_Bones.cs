using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Bones : MonoBehaviour
{
    float m_R = 0;
    Vector3 m_OriginalScale;
    Quaternion m_OriginalRotation;
    // Start is called before the first frame update
    void Start()
    {
        m_OriginalScale = transform.localScale;
        m_OriginalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        m_R += Time.deltaTime * 0.6f;

        var S = m_OriginalScale;
        S.x += (Mathf.Cos(90 + m_R * 5) + Mathf.Cos(m_R * 10)) * 0.005f;
        S.y += (Mathf.Cos(90 + m_R * 10) + Mathf.Cos(m_R * 10)) * 0.001f;

        transform.rotation = m_OriginalRotation * Quaternion.Euler(0, 0, Mathf.Cos(m_R * 4) * 0.8f);
        transform.localScale = S;
    }
}
