using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour {

	public GameObject optionPopUpWindow;
	public GameObject optionSoundSwitch_On;
	public GameObject optionSoundSwitch_Off;

	public GameObject optionBgmSwitch_On;
	public GameObject optionBgmSwitch_Off;




	public void Start()
	{
		optionPopUpWindow.SetActive (false);
	}

	public void OptionWindowActive()
	{
		if (optionPopUpWindow.activeSelf != true)
			optionPopUpWindow.SetActive (true);
		else
			optionPopUpWindow.SetActive (false);
	}


	//사운드 OnOff 스위치(중간 버튼) 
	public void SoundSwitch()
	{
		if (optionSoundSwitch_Off.activeSelf != true) 
		{
			optionSoundSwitch_Off.SetActive (true);
			optionSoundSwitch_On.SetActive (false);
			SoundManager.instance.MuteES ();
		}
		else
		{
			optionSoundSwitch_On.SetActive (true);
			optionSoundSwitch_Off.SetActive (false);
			SoundManager.instance.UnMuteES ();
		}
	}

	//사운드 On
	public void SoundSwitchOn()
	{
		optionSoundSwitch_On.SetActive (true);
		optionSoundSwitch_Off.SetActive (false);
		SoundManager.instance.UnMuteES ();
	}

	//사운드 Off
	public void SoundSwitchFalse()
	{
		optionSoundSwitch_On.SetActive (false);
		optionSoundSwitch_Off.SetActive (true);
		SoundManager.instance.MuteES ();

	}
	//BGM OnOff 스위치(중간 버튼) 
	public void BgmSwitch()
	{
		if (optionBgmSwitch_Off.activeSelf != true) 
		{
			optionBgmSwitch_Off.SetActive (true);
			optionBgmSwitch_On.SetActive (false);
			SoundManager.instance.MuteBGM ();
		}
		else
		{
			optionBgmSwitch_On.SetActive (true);
			optionBgmSwitch_Off.SetActive (false);
			SoundManager.instance.UnMuteBGM ();
		}
	}

	//BGM On
	public void BgmSwitchOn()
	{
		optionBgmSwitch_On.SetActive (true);
		optionBgmSwitch_Off.SetActive (false);
		SoundManager.instance.UnMuteBGM ();
	}

	//BGM Off
	public void BgmSwitchFalse()
	{
		optionBgmSwitch_Off.SetActive (true);
		optionBgmSwitch_On.SetActive (false);
		SoundManager.instance.MuteBGM ();
	}

}
