using UnityEngine;
using System.Collections;

public class GlobalSoundsManager : MonoBehaviour
{
	
	public AudioClip defaultClip;
	public AudioClip happyClip;
	public AudioClip happyClip2;
	public AudioClip scaredClip;
	public AudioClip scaredClip2;
	public AudioClip worriedClip;
	float delay = 2.0f;
	//public AudioClip myPlaylist[];
	
	private GameObject globalManager;
	private float fear;
	float audioVolume = 0f;
	bool fadeInOver = true;
	bool fadeOutOver = true;
	
	void Start()
	{
		globalManager = GameObject.Find("Global");
		
		// AudioSource.PlayClipAtPoint(defaultClip, Camera.main.transform.position);
		
		if (audio.isPlaying)                    // if there is audio playing
			audio.Stop(); 

		audio.volume = 0;
		audio.playOnAwake = true;
	}
	
	void Update()
	{
		
		fear = globalManager.GetComponent<GlobalManager>().globalFear;

		if (fear == 0)
		{
			if (audio.isPlaying && audio.clip != happyClip2)                    // if there is audio playing
				fadeOut(0.5f);  
			else
				fadeIn(happyClip2,0.1f);
		}
		
		if (fear > 0f && fear <= 60f)
		{
			if (audio.isPlaying && audio.clip != worriedClip)                    // if there is audio playing
				fadeOut(10f);  
			else
				fadeIn(worriedClip,10f);
		}
		
		if (fear > 60f)
		{
			if (audio.isPlaying && audio.clip != scaredClip)                    // if there is audio playing
				fadeOut(10f);  
			else
				fadeIn(scaredClip,10f);
		}
	}

	void fadeIn(AudioClip audioClip, float fadeInSpeed = 0.1f)
	{
		if(!audio.isPlaying || (audio.isPlaying && audio.clip != audioClip))
		{
			audio.clip = audioClip; 
			audioVolume = 0;
			audio.Play();
		}

		if(audio.volume < 1f)
		{
			audioVolume = audioVolume + (fadeInSpeed * Time.deltaTime);
			audio.volume = audioVolume;
		}
	}

	void fadeOut(float fadeOutSpeed = 0.1f)
	{
		if(!audio.isPlaying)
		{
			throw new UnityException("No audio is playing, FadeOut is NOT possible");
		}

		if(audio.volume > 0)
		{
			audioVolume = audioVolume - (fadeOutSpeed * Time.deltaTime);
			audio.volume = audioVolume;
		} else if(audio.volume <= 0)
		{
			audio.Stop();
		}
	}
}
