using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTalkPanel : MonoBehaviour {

	public Text bossTalk_Text;
	public GameObject bossTalkPanel;

	void Start()
	{
		bossTalk_Text.text = "";
		bossTalkPanel.SetActive (false);
	}

	public void StartShowBossTalkWindow(float _fTime, string _string)
	{
		StartCoroutine (ShowBossTalkWindow (_fTime, _string));
	}

	public IEnumerator ShowBossTalkWindow(float _fTime, string _string)
	{
		bossTalkPanel.SetActive (true);
		bossTalk_Text.text = _string;

		while (_fTime >= 0) {
			_fTime -= Time.deltaTime;
			yield return null;
		}
		bossTalkPanel.SetActive (false);
		bossTalk_Text.text = "";

		yield break;
	}

}
