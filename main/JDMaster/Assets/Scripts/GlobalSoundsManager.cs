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
    //public AudioClip myPlaylist[];

    private GlobalManager globalManager;
    private float fear;
	private float time;

	bool fadeInOver = true;
	bool fadeOutOver = true;
	int population;
	float audioVolume = 0f;
	float maxVolume;
	
	void Start()
    {
		maxVolume = audio.volume;
        globalManager = GameObject.Find("Global").GetComponent<GlobalManager>();
		population = GlobalManager.globalManager.population;

		time = 0f;

        // AudioSource.PlayClipAtPoint(defaultClip, Camera.main.transform.position);

        if (audio.isPlaying)                    // if there is audio playing
            audio.Stop();                       // stop it


        audio.loop = true;
        audio.playOnAwake = true;

		if(Application.loadedLevelName == "TowerDefense")
			fadeIn(happyClip2,0.1f);
    }

    void Update()
    {
		if(Application.loadedLevelName == "TowerDefense")
		{
			int currentPopulation = GlobalManager.globalManager.population;

			if(currentPopulation <= population/2 && currentPopulation > population/3)
			{
				Debug.Log(population);

				if (audio.isPlaying && audio.clip != worriedClip)                    // if there is audio playing
					fadeOut(0.5f);  
				else
					fadeIn(worriedClip,10f);
			}

			if(currentPopulation <= population/3 && currentPopulation > 0)
			{
				Debug.Log(population);

				if (audio.isPlaying && audio.clip != scaredClip)                    // if there is audio playing
					fadeOut(0.5f);  
				else
					fadeIn(scaredClip,10f);
			}

			if(currentPopulation == 0)
			{
				if (audio.isPlaying && audio.clip != happyClip)                    // if there is audio playing
					fadeOut(10f);  
				else
					fadeIn(happyClip,0.1f);
			}


			/*
			 time = globalManager.time;

			if (time > 60f && audio.clip==happyClip)
			{
				if (audio.isPlaying)                    // if there is audio playing
					audio.Stop();    
				audio.clip = worriedClip;                 // set the audio clip 
				audio.Play();
			}

			if (time > 120f && audio.clip == worriedClip)
			{
				if (audio.isPlaying)                    // if there is audio playing
					audio.Stop();    
				audio.clip = scaredClip;                 // set the audio clip 
				audio.Play();
			}
			*/
		}
		else
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
					fadeIn(worriedClip,10.0f);
			}
			
			if (fear > 60f)
			{
				if (audio.isPlaying && audio.clip != scaredClip)                    // if there is audio playing
					fadeOut(10f);  
				else
					fadeIn(scaredClip,10.0f);
			}
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
		
		if(audio.volume < maxVolume)
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
