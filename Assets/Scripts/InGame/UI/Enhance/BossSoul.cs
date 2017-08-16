using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSoul : MonoBehaviour {

	public bool bIsCheck = false;

	public Image BossSoulPanel;
	public Image SoulCheckSlot;

	public Sprite unActiveBossSoulPanel;
	public Sprite ActiveBossSoulPanel;

	public Sprite unActiveBossSoulCheckSlot;
	public Sprite ActiveBossSoulCheckSlot;

	public Button insertButton;

	protected MakingUI MakingUI;

	protected Player playerData;

	private int nIndex = 0;

	void Start()
	{
		insertButton.onClick.AddListener (InsertButton);
	}

	public void SetUp (MakingUI _MakingUI, Player _player,int _nIndex)
	{
		MakingUI = _MakingUI;
		playerData = _player;

		nIndex = _nIndex;
	}

	protected virtual void InsertButton()
	{
	}

	public void ReSetting()
	{
		bIsCheck = false;

		BossSoulPanel.sprite = unActiveBossSoulPanel;
		SoulCheckSlot.sprite = unActiveBossSoulCheckSlot;
	}

}
