using System.Collections;
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

	Vector3 ViewportPosition;

	public BossEffect bossEffect;
	Camera cam;

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

}
