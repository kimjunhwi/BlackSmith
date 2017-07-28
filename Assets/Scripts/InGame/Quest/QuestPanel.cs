using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestPanel : MonoBehaviour
{

    public int nCost;

	public bool bIsQuest = false;
	public bool bIsBuy  = false;

	[HideInInspector]
	public CGameQuestInfo questData;

    public GameObject runningObject;
	public Button giveUpButton;
	public GameObject completeButton;

	//Complete
	public GameObject startButton;
	public Text completeText;
	public Button sButton;

	//
	public int nItemIndex =0;
	public int getGold =0;
	public int nCompareCondition;

    public Text textGrade;
	public Text textReward;
	public Text textProgressValue;
	public Text textQuestName;
	public Text textQuestContents;

	private GameObject getInfoGameObject;

    private float fTime;

	public QusetManager questManager;




    private void Start()
    {
		
        //runningObject = transform.FindChild("CheckImage").gameObject;
        //runningButton = transform.FindChild("RunningImage").gameObject;
		//startButton = transform.FindChild("StartButton").gameObject;
		giveUpButton.onClick.AddListener(GiveUpActive);

    }

	void Update()
	{
		if (getGold == nCompareCondition) 
		{
			QuestComplete ();
		}
	}

	public void GiveUpActive()
	{
		bIsQuest = false;
		questManager.GiveUpQuest ();
	}

	public void GetQuest(CGameQuestInfo _quest, QusetManager _questManager)
    {
        bIsQuest = true;
		_quest.bIsActive = true;
		nItemIndex = _quest.nIndex;				//index
        questData = _quest;						//퀘스트 정보
		questManager = _questManager;			//퀘스트 매니저  
       
		textQuestContents.text = questData.strExplain;

		textProgressValue.text = getGold.ToString () + "/" + questData.nCompleteCondition.ToString ();
		nCompareCondition = questData.nCompleteCondition;
		if (questData.nRewardGold != 0)
		{
			textReward.text = questData.nRewardGold.ToString ();
		}

		if (questData.nRewardHonor != 0) 
		{
			textReward.text = questData.nRewardHonor.ToString ();
		}

		if (questData.nRewardBossPotion != 0) 
		{
			textReward.text = questData.nRewardBossPotion.ToString ();
		}


		sButton = startButton.GetComponent<Button> ();
		sButton.onClick.RemoveListener (() =>  questManager.CompleteQuest(float.Parse(textReward.text)));
		sButton.onClick.AddListener (() =>  questManager.CompleteQuest(float.Parse(textReward.text)));

    }

	public void QuestComplete()
	{
		startButton.SetActive (true);
		//ScoreManager.ScoreInstance.goldText.text = GameManager.Instance.player
	}



}
