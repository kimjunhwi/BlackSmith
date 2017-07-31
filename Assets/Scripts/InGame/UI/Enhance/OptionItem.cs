using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour {

	private Button LockButton;

	public Image OptionImage;
	public Image LockImage;

	public Sprite OptionSprite;
	public Sprite UnOptionSprite;

	public Sprite LockSprite;
	public Sprite UnLockSprite;

	public Text OptionText;

	private CGameMainWeaponOption WeaponOption;

	void Awake()
	{
		LockButton = transform.FindChild("LockButton").GetComponent<Button>();

		LockButton.onClick.AddListener(HandleClick);
	}

	void OnEnable()
	{

	}

	public void SetInit()
	{
		WeaponOption = null;

		OptionText.text = null;

		OptionImage.sprite = UnOptionSprite;
		LockImage.sprite = UnLockSprite;
	}

	public void Setup(CGameMainWeaponOption _WeaponOption)
	{
		WeaponOption = _WeaponOption;

		OptionText.text = string.Format ("{0} : {1}", WeaponOption.strOptionName, WeaponOption.nValue);

		if (WeaponOption.bIsLock) 
		{
			OptionImage.sprite = OptionSprite;
			LockImage.sprite = LockSprite;
		}

	}

	//버튼 클릭시 아이템을 보여주기 위함
	public void HandleClick()
	{
	}
}
