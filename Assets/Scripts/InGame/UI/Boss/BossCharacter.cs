using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCharacter : Character
{
	
	public int m_nIndex;				//index
	public string m_sName;				//보스 이름 
	public string m_sSkill_01;			//스킬1
	public string m_sSkill_02;			//스킬2

	public string m_sStuff;				//물건 
	public string m_sCompleteGear; 		//완료 장비 
	public int m_nPerfection;			//완성도 
	public int m_nWaitingTime;			//대기 시간 
	public int m_nCompleteReward_Gold;	//보상 골드
	public int m_nCompleteReward_Honor;	//보상 명예
	public int m_nCompleteReward_Jam;	//보상 보석
	public float m_nGearDropPercent;	//장비 드랍율


	public Button bossStartButton;

	public BossPanel bossPanel;
	public UIManager uiManager;
	public SpawnManager spawnManager;

	public virtual void BossSkillStandard () {}	//기본스킬
	public virtual void BossSkill_01() {}		//스킬1
	public virtual void BossSKill_02() {}		//스킬2

	public virtual void BossCreateButton() {}	//보스 버튼 클
	public virtual void BossCreate() {} 		//보스 생성 
	public virtual void BossDie() 	{}			//보스 격퇴



}
