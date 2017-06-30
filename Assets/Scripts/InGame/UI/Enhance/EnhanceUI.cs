using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class EnhanceUI : MonoBehaviour {

	public int nLevel = 0;

	public string strEnhanceName = null;

	public Text EnhanceText;
	public Image EnhanceImage;
	public Button EnhanceButton;

	public Player cPlayer;

	protected virtual void Awake()
	{
		cPlayer = GameManager.Instance.player;

		EnhanceButton.onClick.AddListener (EnhanceButtonClick);
	}

	protected virtual void EnhanceButtonClick() { }

}
