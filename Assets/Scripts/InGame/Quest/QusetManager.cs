using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QusetManager : MonoBehaviour, IPointerClickHandler
{
	
	public int nQuestCount = 0;
	public int nQuestMaxCount = 0;
	public int nQuestMaxHaveCount = 3;
	private int nQuestMileCount = 0;
	private int nQeustMaxMileCount = 0;				
	private int nQuestTotalCount = 32;				//전체 퀘스트 개수

	public GameObject questPopUpWindow_YesNo;		//Yes or No
	public Button questPopUpWindow_YesButton;		//Yes or No가 있는 창에서의 YesButton
	public Button questPopUpWindow_NoButton;		//Yes or No가 있는 창에서의 NoButton
	public Text questPopUpWindowYesNo_Text;			//Yes or No창의 text
	private bool isInitConfirm;

	public GameObject questPopUpWindow_Yes;			//Yes
	public Button completeButton;					//Yes만 있는 창에서의 YesButton
	public Text rewardText;							//Yes만 있는 창에서의 YesButton


	public GameObject questDay;						//QuestElement를 가지는 Obj
		

	//현재 퀘스트의 정보를 가지는 리스트
    public List<QuestPanel> questObjects = new List<QuestPanel>();
    public CGameQuestInfo[] questDatas;

	private float sliderSpeed = 0.5f;
	public Slider silder;
	public Image rewardCheckImage01;
	public Image rewardCheckImage02;
	public Image rewardCheckImage03;

	private int nFirstReward = 3;
	private int nSecondReward = 1;
	private int nThirdReward = 7;

	public SimpleObjectPool questObjectPool;

	Color CheckColor;

	//Timer
	public 	Text timerText;
	public QuestTimer questTimer;

	private bool isLoginAndFirstActive = false;

	public void SetUp()
	{
		gameObject.SetActive (true);

		if (isLoginAndFirstActive == false) {
			Debug.Log ("Quest first Active");

			questTimer.LoadTime ();
			isLoginAndFirstActive = true;
		}
		else
		{
			if (questDay.transform.childCount < 3)
			{
				questTimer.gameObject.SetActive (true);
				questTimer.LoadTime();
			}
		}

		questObjectPool.PreloadPool ();
		questDatas = GameManager.Instance.cQusetInfo;
		nQuestMaxCount = questDatas.Length;
		nQeustMaxMileCount = 7;

		CheckColor = new Color (255.0f, 0, 0, 255.0f);

		//초기화 시간이 지나 있으면 자동으로 초기화 해준다
		if (questTimer.checkIsTimeGone() == true) {
			isInitConfirm = true;
			QuestInitStart ();
		} 
		else 
		{
			//아직 지나있지 않으면 저장되 있던 퀘스트 리스트를 불러온다
			//저장되어 있는 퀘스트를 불러온다 없으면 무작위로 뿌린다.
			if (GameManager.Instance.cQuestSaveListInfo.Count != 0)
			{
				isInitConfirm = false;
				//퀘스트 카운트가 3개
				QuestSaveInitStart ();
			}
		}

		questPopUpWindow_NoButton.onClick.AddListener(() => GameObjectSetActive(questPopUpWindow_YesNo, false));
		completeButton.onClick.AddListener (() => CompleteQuest(0f));
	}


	void Update()
	{
		//마일리지 슬라이더
		if (silder.value <= ((float)nQuestMileCount / (float)nQeustMaxMileCount))
			silder.value += ((float)nQuestMileCount / (float)nQeustMaxMileCount) * sliderSpeed * Time.deltaTime;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		GameObject  getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject.name == "QuestPanel")
		{
			GameManager.Instance.cQuestSaveListInfo.Clear ();
			int curItemCount = questDay.transform.childCount;
			for (int i = 0; i < curItemCount; i++) 
			{
				Transform child = questDay.transform.GetChild (i);
				QuestPanel childQuestPanel = child.GetComponent<QuestPanel> ();
				GameManager.Instance.cQuestSaveListInfo.Add (childQuestPanel.questData);
			}
			questTimer.SaveTime ();



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

	//포기 버튼을 누를시
	public void GiveUpQuest()
	{
		if (!questPopUpWindow_YesNo.activeSelf) 
		{
			questPopUpWindow_YesNo.SetActive (true);
			questPopUpWindowYesNo_Text.text = "퀘스트를 포기 하시겠습니까?";
			questPopUpWindow_YesButton.onClick.AddListener (CheckQuestDestroy);
			questPopUpWindow_YesButton.onClick.AddListener (() => GameObjectSetActive(questPopUpWindow_YesNo, false));
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

	//퀘스트완료  
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

		for (int i = 0; i < questDay.transform.childCount; i++)
		{
			GameObject go = questDay.transform.GetChild (i).gameObject;
			deleteQuestPanel = go.GetComponent<QuestPanel> ();

			if (deleteQuestPanel.bIsQuest == false)
			{
				deleteQuestPanel.bIsQuest = false;
				deleteQuestPanel.startButton.SetActive (false);
				questObjectPool.ReturnObject (go);
				questObjects.Remove (deleteQuestPanel);
				questPopUpWindow_YesButton.onClick.RemoveListener (CheckQuestDestroy);
				questPopUpWindow_YesButton.onClick.RemoveListener (() => GameObjectSetActive(questPopUpWindow_YesNo, false));
				deleteQuestPanel = null;
			}
		}

		//퀘스트 타이머의 시간이 꺼져있을때
		if (questTimer.isTimeOn == false)
			questTimer.StartQuestTimer ();
	}
		


	public void AllDestroyQuest()
	{
		while (questDay.transform.childCount != 0) 
		{
			GameObject go = questDay.transform.GetChild (0).gameObject;
			questObjectPool.ReturnObject(go);
		}


	}
		
	//시간이 지나가지 않아도 초기화 버튼으로 초기화 할때
	public void QuestInit_ShowWindow()
	{
		if (!questPopUpWindow_YesNo.activeSelf) 
		{
			//추후 루비 추가 해야됨
			questPopUpWindow_YesNo.SetActive (true);
			questPopUpWindowYesNo_Text.text = "퀘스트를 초기화 하시겠습니까?";
			questPopUpWindow_YesButton.onClick.AddListener (QuestInitStart);
			questTimer.isTimeEnd = true;
		}
		else
		{
			QuestInitStart ();
		}
	}

	//시간이 지나면 호출되는 초기화
	public void QuestInit()
	{
		isInitConfirm = true;
		QuestInitStart ();
	}

	//게임을 시작하고 처음 퀘스트를 켰을때
	public void QuestSaveInitStart()
	{
		
		GameObject quest;
		nQuestCount = 0;
		AllDestroyQuest ();
		questObjects.Clear ();
		//Add
		for (int i = 0; i < GameManager.Instance.cQuestSaveListInfo.Count; i++)
		{
			nQuestCount++;
			quest = questObjectPool.GetObject ();
			quest.transform.SetParent (questDay.transform,false);
			quest.transform.localScale = Vector3.one;
		
			QuestPanel questPanel = quest.gameObject.GetComponent<QuestPanel> ();
			questObjects.Add (questPanel);
		}

		QuestSaveDataDispatch ();	//Data Dispatch

		isInitConfirm = false;

	}


	public void QuestInitStart()
	{
		GameObject quest;
		questPopUpWindow_YesNo.SetActive (false);
		questPopUpWindow_YesButton.onClick.RemoveListener (QuestInitStart);

		//시간이 다 될시   
		if (questTimer.isTimeEnd == true)
		{
			nQuestCount = 0;
			AllDestroyQuest ();
			questObjects.Clear ();
			//Add
			for (int i = 0; i < nQuestMaxHaveCount; i++)
			{
				nQuestCount++;
				quest = questObjectPool.GetObject ();
				quest.transform.SetParent (questDay.transform,false);
				quest.transform.localScale = Vector3.one;

				QuestPanel questPanel = quest.gameObject.GetComponent<QuestPanel> ();
				questObjects.Add (questPanel);
			}

			QuestDataDispatch ();	//Data Dispatch

			questTimer.isTimeOn = false;
			questTimer.isTimeEnd = false;
			questTimer.InitQuestTimer ();

		}
		//초기화 버튼을 누를시
		if(isInitConfirm == true)
		{
			nQuestCount = 0;
			AllDestroyQuest ();
			questObjects.Clear ();
			//Add
			for (int i = 0; i < nQuestMaxHaveCount; i++)
			{
				nQuestCount++;
				quest = questObjectPool.GetObject ();
				quest.transform.SetParent (questDay.transform,false);
				quest.transform.localScale = Vector3.one;

				QuestPanel questPanel = quest.gameObject.GetComponent<QuestPanel> ();
				questObjects.Add (questPanel);
			}

			QuestDataDispatch ();	//Data Dispatch

			isInitConfirm = false;
			questTimer.isTimeOn = false;
			questTimer.InitQuestTimer ();
		}

	}

	//저장되어있던 데이터 배치
	public void QuestSaveDataDispatch()
	{
		for(int i = 0 ; i< questObjects.Count; i++)
		{
			QuestPanel questPanel = questObjects[i].gameObject.GetComponent<QuestPanel> ();
			questPanel.GetQuest (GameManager.Instance.cQuestSaveListInfo[i] , this);
		} 
	
	}

	//Data 할당
	public void QuestDataDispatch()
    {
		for(int i = 0 ; i< questObjects.Count; i++)
		{
			QuestPanel questPanel = questObjects[i].gameObject.GetComponent<QuestPanel> ();
				
			int random = Random.Range (0, nQuestTotalCount);
			questPanel.GetQuest (questDatas [random], this);
		} 
    }
}
