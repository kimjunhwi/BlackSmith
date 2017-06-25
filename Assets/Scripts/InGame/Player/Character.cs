using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;
using UnityEngine.EventSystems;

[System.Serializable]
public class Character : MonoBehaviour 
{
    public enum ENORMAL_STATE
    {
        WALK,
        WAIT,
        BACK,
    }
    public enum CHARACTER_GRADE
    {
        NORMAL = 0,
        MAGIC,
        LEGEND,
    }
    public float fGold;
    //완성도
    public float m_fComplate { get; set; }
    //온도
    public float m_fTemperator { get; set; }

	//캐릭터 상태
	public ENORMAL_STATE E_STATE;

    //Max온도치
	protected const int n_nMinTemperature = 0;

    

    //캐릭터(무기) 등급
    protected CHARACTER_GRADE E_GRADE;

    public bool m_bIsRepair = false;
    
    //움직이는 속도
    protected float fSpeed = 1.0f;

    //캐릭터 내부 시간
    protected float m_fCharacterTime = 0.0f;

    //캐릭터가 기다리는 시간
    protected float m_fCharacterWaitTime = 0.0f;

    //캐릭터 애니메이터 (추후 추가)
    protected Animator m_anim;

    //저장될 무기 데이터
    public CGameWeaponInfo weaponData { get; set; }

    //수리하는곳을 보여주는 오브젝트
    protected RepairObject RepairShowObject;

    //무기 이미지
    protected SpriteRenderer weaponsSprite;

    //자신 이미지
    protected SpriteRenderer mySprite;

    protected BoxCollider2D boxCollider;

    protected void Awake()
	{
		E_GRADE = CHARACTER_GRADE.NORMAL;

		mySprite = GetComponent<SpriteRenderer> ();

		m_anim = gameObject.GetComponent<Animator> ();
	}
    
    public virtual void Complate(float _fComplate = 0.0f) { }

    
}
