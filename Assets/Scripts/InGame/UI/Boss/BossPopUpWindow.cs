﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum PANELINDEX
{
	PANEL_BOSS = 10000,
	PANEL_ARBAIT,
	PANEL_INVENTORY,
	PANEL_SHOP,
	PANEL_WEAR,
	PANEL_QUEST,
	PANEL_INHANCE,
}


public class BossPopUpWindow : MonoBehaviour
{
	public GameObject PopUpWindow_Yes;
	public GameObject PopUpWindow_YesNo;
	public GameObject PopUpWindow_Reward;
	public GameObject PopUpWindow_RewardPanel;

	private Text PopUpWindow_Yes_Text;
	private Text PopUpWindow_YesNo_Text;

	public Button PopUpWindow_Yes_Button;
	public Button PopUpWindow_YesNo_YesButton;
	public Button PopUpWindow_YesNo_NoButton;
	public Button PopUpWindow_Reward_YesButton;

	public BossCreator bossCreator;
	public int nBossIndex;


	public BossCharacter bossInfo;

	public Image BossRewardBackGround;

	public SimpleObjectPool backLightPool;
	public RectTransform backLightPosition;
	public RectTransform canvasRect;

	public BossIce bossIce;
	public BossFire bossFire;
	public BossMusic bossMusic;
	public BossSasin bossSasin;

	public bool isRewardPanelOn_Fail = false;
	public bool isRewardPanelOn_Success = false;
	public bool isRewardPanelOn_Finish = false;

	public SimpleObjectPool rewardObjPool;

	Vector3 ViewportPosition;

	public BossEffect bossEffect;

	Camera cam;

	//Image strPath
	private const string strGoldImagePath = "DungeonUI/dungeon_reward_gold";
	private const string strHonorImagePath = "DungeonUI/dungeon_reward_honor";
	private const string strRubyImagePath = "DungeonUI/dungeon_reward_roobie";

	void Start()
	{

		bossEffect = GameObject.Find ("BossEffect").GetComponent<BossEffect> ();
		 
		PopUpWindow_Yes_Text = PopUpWindow_Yes.GetComponentInChildren<Text> ();
		PopUpWindow_Yes_Button = PopUpWindow_Yes.GetComponentInChildren<Button> ();

		PopUpWindow_YesNo_Text = PopUpWindow_YesNo.GetComponentInChildren<Text> ();
		PopUpWindow_YesNo_YesButton = PopUpWindow_YesNo.transform.GetChild (1).GetComponent<Button> ();
		PopUpWindow_YesNo_NoButton = PopUpWindow_YesNo.transform.GetChild(2).GetComponent<Button>();

		PopUpWindow_Reward_YesButton = PopUpWindow_Reward.transform.GetChild (1).GetComponent<Button> ();



	

		cam = Camera.main;
		PopUpWindow_Yes.SetActive (false);
		PopUpWindow_YesNo.SetActive (false);
		PopUpWindow_Reward.SetActive (false);

	}


	public void PopUpWindowYes_Switch()
	{
		if (PopUpWindow_Yes.activeSelf != true)
			PopUpWindow_Yes.SetActive (true);
		else
			PopUpWindow_Yes.SetActive (false);	
	}

	public void PopUpWindowYesNo_Switch()
	{
		Debug.Log ("PopUpWindowYesNo_Switch Call!");
		if (PopUpWindow_YesNo.activeSelf != true) 
		{
			PopUpWindow_YesNo_YesButton.onClick.AddListener ( bossCreator.BossCreateInit);

			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_SASIN) {
				PopUpWindow_YesNo_Text.text = "보스(사신)을 소환 하시겠습니까?";

			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_ICE) {
				PopUpWindow_YesNo_Text.text = "보스(얼음)을 소환 하시겠습니까?";
			

			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_FIRE) {
				PopUpWindow_YesNo_Text.text = "보스(불)을 소환 하시겠습니까?";

			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) {
				PopUpWindow_YesNo_Text.text = "보스(음악)을 소환 하시겠습니까?";
		
			} else
				PopUpWindow_YesNo_Text.text = "보스 소환 실패";
			
			PopUpWindow_YesNo.SetActive (true);
		} 
		else 
		{
			Debug.Log ("Boss PopUpWindow.SetActive(false)");
			PopUpWindow_YesNo_YesButton.onClick.RemoveListener (bossCreator.BossCreateInit);
			PopUpWindow_YesNo.SetActive (false);
		}
	}

	public void PopUpWindowReward_Switch_isFail()
	{
		if (PopUpWindow_Reward.activeSelf != true && isRewardPanelOn_Fail == false) 
		{
			Debug.Log ("Active IsFail");
			//bossInfo.ItemInfo.....
			PopUpWindow_Reward.SetActive (true);
			isRewardPanelOn_Fail = true;
		} 
		else
		{
			Debug.Log ("DeActive IsFail");
			if (bossIce.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossIce.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossFire.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossFire.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossSasin.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossSasin.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossMusic.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossMusic.eCureentBossState = Character.EBOSS_STATE.FINISH;
			
			PopUpWindow_Reward_YesButton.onClick.RemoveListener (PopUpWindowReward_Switch_isFail);
			isRewardPanelOn_Fail = false;
			isRewardPanelOn_Finish = true;
			PopUpWindow_Reward.SetActive (false);
		}
	}

	public void PopUpWindowReward_Switch_isSuccess()
	{

		if (PopUpWindow_Reward.activeSelf != true) 
		{
			Debug.Log ("Active IsSuccess");
			//bossInfo.ItemInfo.....

			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_ICE)
			{
				RewardShowAndAdd (0, 1);
			}

			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_SASIN)
			{
				RewardShowAndAdd (2, 3);
			}

			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_FIRE)
			{
				RewardShowAndAdd (4, 5);
			}


			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC)
			{
				RewardShowAndAdd (6, 7);
			}


			if (bossIce.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossIce.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossFire.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossFire.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossSasin.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossSasin.eCureentBossState = Character.EBOSS_STATE.FINISH;
			if (bossMusic.eCureentBossState == Character.EBOSS_STATE.RESULT)
				bossMusic.eCureentBossState = Character.EBOSS_STATE.FINISH;


			PopUpWindow_Reward.SetActive (true);
			isRewardPanelOn_Success = true;
		} 
		else
		{
			Debug.Log ("DeActive IsSuccess");
			int rewardCount = PopUpWindow_RewardPanel.transform.childCount;

			while (rewardCount != 0) {
				GameObject reward = PopUpWindow_RewardPanel.transform.GetChild (0).gameObject;
				rewardObjPool.ReturnObject (reward);
				rewardCount--;
			}

			PopUpWindow_Reward_YesButton.onClick.RemoveListener (PopUpWindowReward_Switch_isSuccess);
			isRewardPanelOn_Success = false;
			isRewardPanelOn_Finish = true;
			PopUpWindow_Reward.SetActive (false);
		}
	}

	public void GetBossIndex(int _index)
	{
		nBossIndex = _index;
	}
	public void GetBossInfo(BossCharacter _bossCharacter)
	{
		bossInfo = _bossCharacter;
	}

	public void SetBossRewardBackGroundImage(bool _isFailed)
	{
		if (_isFailed == false) {
			PopUpWindow_RewardPanel.SetActive (true);
			BossRewardBackGround.sprite = Resources.Load ("DungeonUI/dungeon_reward_clear", typeof(Sprite)) as Sprite;
		}
		else
		{
			BossRewardBackGround.sprite = Resources.Load ("DungeonUI/dungeon_reward_fail", typeof(Sprite)) as Sprite;
			PopUpWindow_RewardPanel.SetActive (false);
		}
	}


	public void RewardShowAndAdd(int _bossWeaponIndex01, int _bossWeaponIndex02)
	{
		BossCharacter bossCharacter = null;
		if (_bossWeaponIndex01 == 0 && _bossWeaponIndex02 == 1)
			bossCharacter = bossIce;
		if (_bossWeaponIndex01 == 2 && _bossWeaponIndex02 == 3)
			bossCharacter = bossSasin;
		if (_bossWeaponIndex01 == 4 && _bossWeaponIndex02 == 5)
			bossCharacter = bossFire;
		if (_bossWeaponIndex01 == 6 && _bossWeaponIndex02 == 7)
			bossCharacter = bossMusic;


		float nRandom_Weapon01 = Random.Range (0f, 1f);	
		if (nRandom_Weapon01 <= bossCharacter.bossInfo.fDropPercent) 
		{
			GameObject Reward = rewardObjPool.GetObject ();
			Reward.transform.SetParent (PopUpWindow_RewardPanel.transform, false);
			Reward.transform.localScale = Vector3.one;
			Text RewardText = Reward.GetComponentInChildren<Text> ();
			RewardText.text = "장비";
			Image RewardImage = Reward.transform.GetChild(1).GetComponent<Image> ();
			RewardImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache (GameManager.Instance.bossWeaponInfo [_bossWeaponIndex01].strResource);
			GameManager.Instance.player.inventory.inventorySlots [GameManager.Instance.bossWeaponInfo [_bossWeaponIndex01].nSlotIndex].AddItem
			(GameManager.Instance.bossWeaponInfo [_bossWeaponIndex01]);
		}

		float nRandom_Weapon02 = Random.Range (0f, 1f);	
		if (nRandom_Weapon02 <= bossCharacter.bossInfo.fDropPercent)
		{
			GameObject Reward = rewardObjPool.GetObject ();
			Reward.transform.SetParent (PopUpWindow_RewardPanel.transform, false);
			Reward.transform.localScale = Vector3.one;
			Text RewardText = Reward.GetComponentInChildren<Text> ();
			RewardText.text = "장비";
			Image RewardImage = Reward.transform.GetChild(1).GetComponent<Image> ();
			RewardImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache (GameManager.Instance.bossWeaponInfo [_bossWeaponIndex02].strResource);
			GameManager.Instance.player.inventory.inventorySlots [GameManager.Instance.bossWeaponInfo [_bossWeaponIndex02].nSlotIndex].AddItem
			(GameManager.Instance.bossWeaponInfo [_bossWeaponIndex02]);
		}
		//Gold
		GameObject Gold = rewardObjPool.GetObject ();
		Gold.transform.SetParent (PopUpWindow_RewardPanel.transform, false);
		Gold.transform.localScale = Vector3.one;
		Text GoldText = Gold.GetComponentInChildren<Text> ();
		GoldText.text = "골드";
		Image GoldRewardImage = Gold.transform.GetChild(1).GetComponent<Image> ();
		GoldRewardImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache (strGoldImagePath);
		ScoreManager.ScoreInstance.GoldPlus (bossCharacter.bossInfo.nGold);
		//Honor
		GameObject Honor = rewardObjPool.GetObject ();
		Honor.transform.SetParent (PopUpWindow_RewardPanel.transform, false);
		Honor.transform.localScale = Vector3.one;
		Text HonorText = Honor.GetComponentInChildren<Text> ();
		HonorText.text = "명예";
		Image HonorRewardImage = Honor.transform.GetChild(1).GetComponent<Image> ();
		HonorRewardImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache (strHonorImagePath);
		ScoreManager.ScoreInstance.HonorPlus (bossCharacter.bossInfo.nHonor);
		//Dia
		GameObject Dia = rewardObjPool.GetObject ();
		Dia.transform.SetParent (PopUpWindow_RewardPanel.transform, false);
		Dia.transform.localScale = Vector3.one;
		Text DiaText = Dia.GetComponentInChildren<Text> ();
		DiaText.text = "보석";
		Image DiaRewardImage = Dia.transform.GetChild(1).GetComponent<Image> ();
		DiaRewardImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache (strRubyImagePath);
		//ScoreManager.ScoreInstance.HonorPlus (bossCharacter.bossInfo.nDia);

	}
}
