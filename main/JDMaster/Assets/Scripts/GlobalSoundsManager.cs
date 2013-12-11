using UnityEngine;
using System.Collections;

public class GlobalSoundsManager : MonoBehaviour
{

    public AudioClip defaultClip;
    public AudioClip happyClip;
    public AudioClip happyClip2;
    public AudioClip scaredClip;
    public AudioClip scaredClip2;
    public AudioClip warriedClip;
    //public AudioClip myPlaylist[];

    private GameObject globalManager;
    private float fear;

    void Start()
    {
        globalManager = GameObject.Find("Global");

        // AudioSource.PlayClipAtPoint(defaultClip, Camera.main.transform.position);

        if (audio.isPlaying)                    // if there is audio playing
            audio.Stop();                       // stop it

        audio.clip = happyClip;               // set the audio clip 
        audio.Play();

        audio.loop = true;
        audio.playOnAwake = true;
    }

    void Update()
    {

        fear = globalManager.GetComponent<GlobalManager>().globalFear;

        if (fear >= 0f && fear <= 30f && audio.clip != happyClip)
        {
            // Debug.Log("Entro in HAPPY CLIP");
            if (audio.isPlaying)                    // if there is audio playing
                audio.Stop();                       // stop it

            audio.clip = happyClip;                 // set the audio clip 
            audio.Play();
            audio.loop = true;
        }

        if (fear > 30f && fear <= 60f && audio.clip != warriedClip)
        {
            // Debug.Log("Entro in WORRIED CLIP");
            if (audio.isPlaying)                    // if there is audio playing
                audio.Stop();                       // stop it

            audio.clip = warriedClip;                 // set the audio clip 
            audio.Play();
            audio.loop = true;
        }

        if (fear > 60f && audio.clip != scaredClip)
        {
            // Debug.Log("Entro in SCARED CLIP");
            if (audio.isPlaying)                    // if there is audio playing
                audio.Stop();                       // stop it

            audio.clip = scaredClip;                 // set the audio clip 
            audio.Play();
            audio.loop = true;
        }
    }
}
