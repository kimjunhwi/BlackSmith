using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	//변화될 값 
	public CGamePlayerData changeStats;

	//기본 값 
	public CGamePlayerData defaultStats;


    public List<CGameEquiment> List_items;

	CGameEquiment BaseEquimnet = null;
    //CGameEquiment WeaponEquimnet;
    //CGameEquiment GearEquimnet;
    //CGameEquiment AccessoryEquimnet;

    public Inventory inventory;

	public int GetSmithLevel() { return changeStats.nBlackSmithLevel; }
	public void SetSmithLevel(int _nValue) { changeStats.nBlackSmithLevel = _nValue; }

	public int GetRepairLevel() {return changeStats.nEnhanceRepaireLevel; }
	public void SetRepairLevel(int _nValue){changeStats.nEnhanceRepaireLevel = _nValue;}

	public int GetMaxWaterLevel(){return changeStats.nEnhanceMaxWaterLevel;}
	public void SetMaxWaterLevel(int _nValue) {changeStats.nEnhanceMaxWaterLevel = _nValue; }

	public int GetWaterPlusLevel(){ return changeStats.nEnhanceWaterPlusLevel;}
	public void SetWaterPlusLevel(int _nValue){changeStats.nEnhanceWaterPlusLevel = _nValue;}

	public int GetAccuracyRateLevel(){return changeStats.nEnhanceAccuracyRateLevel;}
	public void SetAccuracyRateLevel(int _nValue){changeStats.nEnhanceAccuracyRateLevel = _nValue;}

	public int GetCriticalLevel() {return changeStats.nEnhanceCriticalLevel; }
	public void SetCriticalLevel(int _nValue){changeStats.nEnhanceCriticalLevel = _nValue;}

	public float GetWaterPlus() { return changeStats.fWaterPlus; }
	public void SetWaterPlus(float _fValue) { changeStats.fWaterPlus = _fValue; }

	public float GetRepairPower(){ return changeStats.fRepairPower; }
	public void SetRepairPower(float _fValue) { changeStats.fRepairPower = _fValue; }

	public float GetMaxWaterPlus() { return changeStats.fMaxWaterPlus; }
	public void SetMaxWaterPlus(float _fValue) { changeStats.fMaxWaterPlus = _fValue; }

	public float GetAccuracyRate() { return changeStats.fAccuracyRate; }
	public void SetAccuracyRate(float _fValue) { changeStats.fAccuracyRate = _fValue; }

    public float GetCriticalChance() { return changeStats.fCriticalChance; }
	public void SetCriticalChance(float _fValue) { changeStats.fCriticalChance = _fValue; }

	public float GetTemperatureMinus() { return changeStats.fTemperatureMinus; }
	public void SetTemperatureMinus(float _fValue) { changeStats.fTemperatureMinus = _fValue; }


	public void SetDefaultRepair(float _fValue) {defaultStats.fRepairPower += _fValue;}
	public void SetDefaultMaxWater(float _fValue) {defaultStats.fMaxWaterPlus += _fValue;}
	public void SetDefaultWaterPlus(float _fValue) {defaultStats.fWaterPlus += _fValue;}
	public void SetDefaultAccuracy(float _fValue) {defaultStats.fAccuracyRate += _fValue;}
	public void SetDefaultCritical(float _fValue) {defaultStats.fCriticalChance += _fValue;}


    public void Awake()
    {
        BaseEquimnet = new CGameEquiment();

        //WeaponEquimnet = BaseEquimnet;
        //GearEquimnet = BaseEquimnet;
        //AccessoryEquimnet = BaseEquimnet;
    }

    public void Init(List<CGameEquiment> _itemList, CGamePlayerData _defaultStats)
    {
        List_items = _itemList;
		defaultStats = new CGamePlayerData( _defaultStats);
		changeStats = new CGamePlayerData( _defaultStats);
    }

    public void SetInventroy(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.SetInventory(this, List_items);
    }

	public void SetGold(float _gold)
	{
        defaultStats.fGold = _gold;
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




