using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInfo : MonoBehaviour 
{

	public int iID = 0;
	public int _index = 0;
	public int nType = 0;
	public bool bLoop = false;
	public bool bRemove = false;
	public float DeathTime = 0.0f;
	//public AudioSource kAudioSource = null;
	public float audiolength = 0.0f;

	void Awake()
	{
		//DontDestroyOnLoad(this);
	}
	/*
	void Update()
	{
		if (bRemove)
		{
			DeathTime -= Time.deltaTime;
			if (DeathTime <= 0.0f)
			{
				SoundManager.instance.RemoveClone(gameObject);
				//Destroy( gameObject );
			}
		}

		//if(audio == null)	return;		
		//playOneShot
		//if( !bLoop )
		//	if(!audio.isPlaying)
		//	{
		//		Destroy( gameObject );
		//	}
	}
	*/

}
