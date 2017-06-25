using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{
	
	private string strName 				= "";		//닉네임			
    private float fRepairPower 			= 10;		//수리력 
    private float fTemperatureMinus 	= 0;		//온도 감소 수치량 
    private float fArbaitsPower 		= 0;		//알바수리력 
    private float fHornorPlusPercent 	= 0;		//명예추가 증가량
	private float fGold 				= 0;
    private float fGoldPlusPercent 		= 0;		//골드추가 증가량
    private float fWaterPlus 			= 1;		//물가량
    private float fMaxWaterPlus 		= 30;		//물최대치 증가량 
    private float fCriticalChance 		= 0;		//크리티컬 확률
    private float fCriticalDamage 		= 0;		//크리데미지
    private float fBigSuccessed 		= 0;		//대성공
    private float fAccuracyRate 		= 100;		//정확도

	private int nBlackSmithLevel 		= 0;
	private int nEnhanceRepaireLevel 	= 0;
	private int nEnhanaceWaterLevel 	= 0;

    public List<CGameEquiment> List_items;

	CGameEquiment BaseEquimnet = null;
    CGameEquiment WeaponEquimnet;
    CGameEquiment GearEquimnet;
    CGameEquiment AccessoryEquimnet;

    public Inventory inventory;

	public float GetWaterPlus() { return fWaterPlus; }

	public float GetRepairPower(){ return fRepairPower; }

	public float GetMaxWaterPlus(){ return fMaxWaterPlus; }

	public float GetAccuracyRate() { return fAccuracyRate; }

	public float GetTemperatureMinus() { return fTemperatureMinus; }

    public void Awake()
    {
        BaseEquimnet = new CGameEquiment();

        WeaponEquimnet = BaseEquimnet;
        GearEquimnet = BaseEquimnet;
        AccessoryEquimnet = BaseEquimnet;
    }

    public void Init(List<CGameEquiment> _itemList)
    {
        List_items = _itemList;
    }

    public void SetInventroy(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.SetInventory(this, List_items);
    }

	public void SetGold(float _gold)
	{
		fGold = _gold;
		Debug.Log ("Player Gold :  " + _gold);


	}

	public int GetItemListCount()
	{
		List_items = inventory.GetItemList ();

		if (List_items == null)
			return 0;

		return List_items.Count;
	}
}


