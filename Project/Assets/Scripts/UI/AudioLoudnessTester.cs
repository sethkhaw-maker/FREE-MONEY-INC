
using UnityEngine;

public class AudioLoudnessTester : MonoBehaviour
{
/*    public AudioSource BeatClick;
    public AudioSource BeatClick2;*/
    public float initsize = 1f;
    public AudioSource Bgmaudiosource;
    public float updateStep = 0.05f;
    public int sampledataLength = 1024;

    private float currentupdateTime = 0f;

    public float clipLoudness;

    private float[] clipSampleData;

    //Array if u want many objs
    public GameObject[] sprite;
    public float sizeFactor = 1;

    public float minSize = 0;
    public float maxSize = 100;

    //private void Awake()
    //{
    //    clipSampleData = new float[sampledataLength];

    //}

    //private void Update()
    //{
    //    currentupdateTime += Time.deltaTime;
    //    if (currentupdateTime >= updateStep)
    //    {
    //        currentupdateTime = 0f;
    //        Bgmaudiosource.clip.GetData(clipSampleData, Bgmaudiosource.timeSamples);
    //        clipLoudness = 0f;

    //        foreach (var sample in clipSampleData)
    //        {
    //            clipLoudness += Mathf.Abs(sample);
    //        }

    //        clipLoudness /= sampledataLength;

    //        clipLoudness *= sizeFactor;

    //        clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);

    //        foreach (var spritesample in sprite)
    //        {
    //            spritesample.transform.localScale = new Vector3(initsize + clipLoudness / 5, initsize + clipLoudness / 5, initsize + clipLoudness / 5);
    //        }



    //    }
    //}
  
}
