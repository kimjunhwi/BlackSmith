using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QusetManager : MonoBehaviour, IPointerClickHandler
{
    private int nQuestCount = 0;
    private int nQuestMaxCount = 0;
	private int nQuestMaxHaveCount = 3;
	private int nQuestMileCount = 0;
	private int nQeustMaxMileCount = 0;

	public GameObject questPopUpWindow_YesNo;		//Yes or No
	public Button questPopUpWindow_YesButton;
	public Button questPopUpWindow_NoButton;
	public Text questPopUpWindowYesNo_Text;
	private bool isInitConfirm;

	public GameObject questPopUpWindow_Yes;			//Yes

	public Text rewardText;
	public Button completeButton;

	public GameObject questDay;
	private GameObject getInfoGameObject;

    public List<QuestPanel> questObjects = new List<QuestPanel>();
	public GameObject questContents;
    public CGameQuestInfo[] questDatas;

	private float sliderSpeed = 0.5f;
	public Slider silder;
	public Image rewardCheckImage01;
	public Image rewardCheckImage02;
	public Image rewardCheckImage03;

	private int nFirstReward = 3;
	private int nSecondReward = 5;
	private int nThirdReward = 7;

	Color CheckColor;

	public 	Text timerText;
	private float nTimeMinute = 1;
	private float nTimeSecond = 10;

	public bool isTimeOn = false;

	public bool isPanelOn = false;

	public void SetUp()
	{
		gameObject.SetActive (true);
		questDatas = GameManager.Instance.cQusetInfo;
		nQuestMaxCount = questDatas.Length;
		nQeustMaxMileCount = 7;

		CheckColor = new Color (255.0f, 0, 0, 255.0f);

		isTimeOn = true;
		isInitConfirm = true;
		QuestInitStart ();

		completeButton.onClick.AddListener (() => CompleteQuest(0f));
		nTimeMinute = 1;
		nTimeSecond = 10;
	}


	void Update()
	{
		if(silder.value <= ((float)nQuestMileCount / (float)nQeustMaxMileCount) )
			silder.value += ((float)nQuestMileCount / (float)nQeustMaxMileCount) * sliderSpeed * Time.deltaTime;


	}

	public void OnPointerClick (PointerEventData eventData)
	{
		
		getInfoGameObject = eventData.pointerEnter;
		GameManager.Instance.cQuestSaveIndex.Clear ();
		if (getInfoGameObject.gameObject.name == "QuestPanel")
		{
			int curItemCount = questContents.transform.childCount;
			for (int i = 0; i < curItemCount; i++) 
			{
				Transform child = questContents.transform.GetChild (i);
				QuestPanel childQuestPanel = child.GetComponent<QuestPanel> ();
				GameManager.Instance.cQuestSaveIndex.Add (childQuestPanel.nItemIndex);
			}
			GameManager.Instance.curLeftQuestTime_Minute = nTimeMinute;
			GameManager.Instance.curLeftQuestTime_Second = nTimeSecond;
			getInfoGameObject.SetActive (false);

		}

		//Test
		if (getInfoGameObject.gameObject.name == "CompleteButton") 
		{
			QuestPanel questPanel = getInfoGameObject.GetComponentInParent<QuestPanel> ();
			questPanel.getGold = questPanel.questData.nCompleteCondition;
			questPanel.textProgressValue.text = questPanel.getGold.ToString () + "/" + questPanel.questData.nCompleteCondition.ToString ();
			questPanel.QuestComplete ();
			questPanel.bIsQuest = false;
			nQuestMileCount++;

			if (nQuestMileCount == nFirstReward) 
			{
				//rewardCheckImage01.color = CheckColor;
				rewardCheckImage01.enabled = true;
			}

			if (nQuestMileCount == nSecondReward) 
			{
				//rewardCheckImage02.color = CheckColor;
				rewardCheckImage02.enabled = true;
			}

			if (nQuestMileCount == nThirdReward) 
			{
				//rewardCheckImage03.color = CheckColor;
				rewardCheckImage03.enabled = true;
			}
		}
	}
	public void GiveUpQuest()
	{
		if (!questPopUpWindow_YesNo.activeSelf) 
		{
			questPopUpWindow_YesNo.SetActive (true);
			questPopUpWindowYesNo_Text.text = "퀘스트를 포기 하시겠습니까?";
			questPopUpWindow_YesButton.onClick.AddListener (() => GameObjectSetActive(questPopUpWindow_YesNo, false));
			questPopUpWindow_YesButton.onClick.AddListener (CheckQuestDestroy);
		}
		else 
		{
			CheckQuestDestroy ();
			questPopUpWindow_YesNo.SetActive (false);
		}
	}

	public void GameObjectSetActive(GameObject _gameObject,  bool _bool)
	{
		if (_bool == true)
		{
			_gameObject.SetActive (true);
		} else
			_gameObject.SetActive (false);
	}



	public void CompleteQuest(float _gold)
	{
		//Complete
		if (questPopUpWindow_Yes.activeSelf) 
		{
			questPopUpWindow_Yes.SetActive (false);
			CheckQuestDestroy ();
		} 
		else 
		{
			rewardText.text = _gold.ToString () + " 획득!!";
			ScoreManager.ScoreInstance.GoldPlus (_gold);
			questPopUpWindow_Yes.SetActive (true);
		}
	}

	public void CheckQuestDestroy()
	{
		QuestPanel deleteQuestPanel = null;
		foreach (QuestPanel quest in questObjects)
		{
			if(quest.bIsQuest == false)
			{
				Destroy (quest.gameObject);
				deleteQuestPanel = quest;
			}
		}
		questObjects.Remove (deleteQuestPanel);
	}

	public void AllDestroyQuest()
	{
		foreach (QuestPanel quest in questObjects)
		{
			
			Destroy (quest.gameObject);

		}
	}

	public void QuestInitConfirm()
	{
		if (isTimeOn == true) {
			isInitConfirm = true;
			QuestInitStart ();
			questPopUpWindow_YesNo.SetActive (false);
		} else 
		{
			questPopUpWindow_YesNo.SetActive (false);
			isInitConfirm = false;
		}
			
	}



	public void QuestInit()
	{
		if (!questPopUpWindow_YesNo.activeSelf)
		{
			if (isTimeOn == true)
			{
				questPopUpWindow_YesNo.SetActive (true);
				questPopUpWindowYesNo_Text.text = "퀘스트를 초기화 하시겠습니까?";
				questPopUpWindow_YesButton.onClick.AddListener (QuestInitConfirm);
			}
		}
		else
		{
			questPopUpWindow_YesNo.SetActive (false);
		}
	}

	public void QuestInitStart()
	{

		if (isTimeOn == true && isInitConfirm == true && GameManager.Instance.cQuestSaveIndex.Count == 0) {
			nQuestCount = 0;
			questObjects.Clear ();
			//All QuestDelete
			int childs = questDay.transform.childCount;

			for (int i = childs - 1; i >= 0; i--) {
				Destroy (questDay.transform.GetChild (i).gameObject);
			}
			//Add
			for (int i = 0; i < nQuestMaxHaveCount; i++)
			{
				nQuestCount++;
				GameObject quest = (GameObject)Instantiate (Resources.Load ("Prefabs/Quest"));
				quest.transform.SetParent (questDay.transform,false);
				quest.transform.localScale = Vector3.one;

				QuestPanel questPanel = quest.gameObject.GetComponent<QuestPanel> ();
				questObjects.Add (questPanel);
			}
			Quest (true);	//Data Dispatch

			TimerWarpper (nTimeMinute, nTimeSecond);
			isTimeOn = false;
			isInitConfirm = false;
		}
		if (isTimeOn == true && isInitConfirm == true && GameManager.Instance.cQuestSaveIndex.Count != 0) 
		{
			nQuestCount = 0;
			AllDestroyQuest ();
			questObjects.Clear ();

			for (int i = 0; i < GameManager.Instance.cQuestSaveIndex.Count; i++)
			{
				GameObject quest = (GameObject)Instantiate (Resources.Load ("Prefabs/Quest"));
				quest.transform.SetParent (questDay.transform,false);
				quest.transform.localScale = Vector3.one;

				QuestPanel questPanel = quest.gameObject.GetComponent<QuestPanel> ();
				questObjects.Add (questPanel);
			}

			Quest (false);	//Data Dispatch

			TimerWarpper (GameManager.Instance.curLeftQuestTime_Minute,GameManager.Instance.curLeftQuestTime_Second);

			isTimeOn = false;
			isInitConfirm = false;
		}

	}

	public void Quest(bool _isInit)
    {
		if (_isInit == true)
		{
			foreach (QuestPanel quest in questObjects)
			{
				if (quest.textQuestContents.text == "") 
				{
					int random = Random.Range (0, nQuestMaxCount);
					quest.GetQuest (questDatas [random], this);
				}
			}
		} 
		else 
		{
			int index = 0;
			
			foreach (QuestPanel quest in questObjects)
			{
				if (quest.textQuestContents.text == "") 
				{
					
					quest.GetQuest (questDatas [GameManager.Instance.cQuestSaveIndex[index]], this);
					index++;
				}
			}
		}

	  
    }

	public void TimerWarpper(float _curM, float _curS)
	{
		StartCoroutine (Timer (_curM, _curS ));	//TimerInit
	}

	public IEnumerator Timer(float _curMin, float _curSec)
	{
		float curMin = _curMin;
		float curSecond = _curSec;
		int second = 0;
		curMin--;
		isTimeOn = false;
		while (curMin >= 0f) 
		{
			curSecond -= Time.deltaTime;
			second = (int)curSecond;
			timerText.text = curMin.ToString () + ":" + second.ToString ();


			if (curMin != 0 && second == 0f) 
			{
				curSecond = 60f;
				curMin--;
			}

			if (curMin == 0 && second == 0f)
			{
				isTimeOn = true;
				break;
			}

			yield return null;
		}
		yield  break;
	}
}
