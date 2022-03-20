using UnityEngine.Audio;
using System;
using UnityEngine;

//The Audio Manager handles generating the audio source and controls the individual settings
//Singleton, modular, after setting up, access a clip by its "name"
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    //Singleton

    //Volume Settings, only adjust through Settings menu
    [Range(0, 1)] public float masterVolume = 1;
    [Range(0, 1)] public float bgmVolume = 1;
    [Range(0, 1)] public float sfxVolume = 1;

    //List of bgms
    public AudioClip[] bgms;                        //The list of BGMs, add in inspector
    [HideInInspector] public AudioSource bgmSource; //DNT
    private bool isTransitioning = false;           //DNT
    private int bgmIndex = 0;                       //The element in BGM list to swap to, call through function
    private float transitionSpeed = 0.5f;           //Speed of the transition, adjust internally once
    private float bgmInternalVolume = 1;            //DNT, Artificial volume for transition scale

    //List of sfxs
    public Sound[] sounds;


    void Awake()
    {
        //Set this copy to static instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Dont destroy this instance on scene change
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>(); //Create and assign a single audio source for bgm
        bgmSource.clip = bgms[0];       //Assign 1st clip to bgm audio source
        bgmSource.loop = true;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Play the BGM on loop
        bgmSource.Play();
    }

    private void Update()
    {
        TransitionBGM();

        //Use this to test a sound, try adjusting the volume sliders and press F!
        if (Input.GetKeyDown(KeyCode.F))
        {
            Play("Twinkle");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeBGM(1);
        }

        //Debugging
        //print(bgmSource.isPlaying);
    }

    //Call this function anywhere through singleton, plays a sound
    public void Play(string name)
    {
        //Loop through all sounds to find matching string name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //Scale sfx and master volume accordingly
        s.source.PlayOneShot(s.source.clip, s.source.volume * masterVolume * sfxVolume);

        //s.source.Play();  //Obsolete
    }

    //Transition between BGMs
    public void ChangeBGM(int clipIndex)
    {
        if (clipIndex < bgms.Length)
        {
            isTransitioning = true;
            bgmIndex = clipIndex;
        }
        else
        {
            Debug.LogWarning("The BGM clip does not exist!");
        }
    }

    public void TransitionBGM()
    {
        if (isTransitioning)
        {
            bgmInternalVolume -= Time.deltaTime * transitionSpeed;
            if (bgmInternalVolume <= 0)
            {
                //Stop the audio
                bgmSource.Stop();

                //Set volume to 0
                bgmInternalVolume = 0;
                isTransitioning = false;

                //Swap in the new BGM
                bgmSource.clip = bgms[bgmIndex];
                bgmSource.Play();
            }
        }
        else
        {
            if (bgmInternalVolume < 1)
            {
                bgmInternalVolume += Time.deltaTime * transitionSpeed;
            }
            else
            {
                bgmInternalVolume = 1;
            }
        }
        
        bgmSource.volume = bgmInternalVolume * masterVolume * bgmVolume;

    }
}
