using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCharacter : Character
{
	/*
	protected int m_nIndex;				//index
	protected string m_sName;				//보스 이름 
	protected string m_sSkill_01;			//스킬1
	protected string m_sSkill_02;			//스킬2

	protected string m_sStuff;				//물건 
	protected string m_sCompleteGear; 		//완료 장비 
	protected int m_nPerfection;			//완성도 
	protected int m_nWaitingTime;			//대기 시간 
	protected int m_nCompleteReward_Gold;	//보상 골드
	protected int m_nCompleteReward_Honor;	//보상 명예
	protected int m_nCompleteReward_Jam;	//보상 보석
	protected float m_nGearDropPercent;	//장비 드랍율
	*/

	public Boss bossInfo;						//해당 보스의 정보
	public int nIndex;							//해당 보스의 인덱스
	public RepairObject repairObj;				//수리 패널
	public EBOSS_STATE eCureentBossState;		//현재 보스의 상태
	public GameObject bossTimer_Obj;			//보스 타이머 Obj
	public BossTimer bossTimer;					//보스 타이머
	public BossEffect bossEffect;				//보스에 따른 이펙트
	public BossBackGround bossBackGround;		//보스등장시 바뀌는 배경
	public BossPopUpWindow bossPopUpWindow;		//보스 보상창
	public string sBossWeaponSprite;			//보스 무기 이미지
	public GameObject bossUIDisable;			//UIBlockArea
	public BossTalkPanel bossTalkPanel;			//보스 말풍선 
	public GameObject bossWeapon;				//보스 무기 obj
	public UIDisable uiDisable;
	public UIManager uiManager;
	public GameObject bossPanel;

	public string[] bossWord = new string[4];					//보스 말풍선(시작시 할당) 

	protected bool isFailed = false;								//실패시 띄우는 창에 대한 변수
	protected bool isStandardPhaseFailed = false;					//만약 1페이즈도 못가고 죽을 때
	protected bool isFirstActive = false;							//처음에 켜져있을때 한번만 동작하게 하는 변수  

	public GameObject GuestPanel;

	protected Animator animator;

	protected virtual IEnumerator BossWait() 			{ yield return null;}		//보스 대기(연출)

	protected virtual IEnumerator BossSkillStandard() 	{ yield return null;}	//기본스킬(Phase 00)
	protected virtual IEnumerator BossSkill_01() 		{ yield return null;}		//스킬1
	protected virtual IEnumerator BossSKill_02() 		{ yield return null;}		//스킬2
	 
	protected virtual IEnumerator BossDie() 			{ yield return null;}		//보스 격퇴
	protected virtual IEnumerator BossResult() 			{yield return null;}			//보스 결과
	protected virtual IEnumerator BossFinish() 			{yield return null;}			//보스 끝

	void Awake()
	{
		repairObj = FindObjectOfType<RepairObject> ();
	}
}
