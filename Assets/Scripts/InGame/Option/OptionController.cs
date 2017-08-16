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



	public void SoundSwitch()
	{
		if (optionSoundSwitch_Off.activeSelf != true) 
		{
			optionSoundSwitch_Off.SetActive (true);
			optionSoundSwitch_On.SetActive (false);
		}
		else
		{
			optionSoundSwitch_On.SetActive (true);
			optionSoundSwitch_Off.SetActive (false);
		}
	}

	public void SoundSwitchOn()
	{
		optionSoundSwitch_On.SetActive (true);
		optionSoundSwitch_Off.SetActive (false);
	}


	public void SoundSwitchFalse()
	{
		optionSoundSwitch_On.SetActive (false);
		optionSoundSwitch_Off.SetActive (true);

	}

	public void BgmSwitch()
	{
		if (optionBgmSwitch_Off.activeSelf != true) 
		{
			optionBgmSwitch_Off.SetActive (true);
			optionBgmSwitch_On.SetActive (false);
		}
		else
		{
			optionBgmSwitch_On.SetActive (true);
			optionBgmSwitch_Off.SetActive (false);
		}
	}


	public void BgmSwitchOn()
	{
		optionBgmSwitch_On.SetActive (true);
		optionBgmSwitch_Off.SetActive (false);
	}


	public void BgmSwitchFalse()
	{
		optionBgmSwitch_Off.SetActive (true);
		optionBgmSwitch_On.SetActive (false);

	}

}
