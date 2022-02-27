using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeimdallAudio : MonoBehaviour
{
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (this.gameObject.GetComponent<AudioSource>().isPlaying) { return; }
            else
            {
                //GameController.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(GameController.instance.SoundClips[6]);
                this.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
        }
    }
}
