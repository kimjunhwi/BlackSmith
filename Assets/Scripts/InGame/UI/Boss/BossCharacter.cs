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



	public Boss boss;
	public int nIndex;
	public RepairObject repairObj;
	public EBOSS_STATE eCureentBossState;


	protected virtual IEnumerator BossWait() 	{ yield return null;}		//보스 대기(연출)

	protected virtual IEnumerator BossSkillStandard() { yield return null;}	//기본스킬
	protected virtual IEnumerator BossSkill_01() { yield return null;}		//스킬1
	protected virtual IEnumerator BossSKill_02() { yield return null;}		//스킬2
	 
	protected virtual IEnumerator BossDie() 	{ yield return null;}		//보스 격퇴
	protected virtual IEnumerator BossResult() {yield return null;}			//보스 결과

	void Awake()
	{
		repairObj = FindObjectOfType<RepairObject> ();
	}
}
