using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	//변화될 값 
	public CGamePlayerData changeStats;

	//플레이어 제작 무기  
	private CreatorWeapon creatorWeapon;

    public List<CGameEquiment> List_items;

    CGameEquiment WeaponEquipment = null;
    CGameEquiment GearEquipmnet = null;
    CGameEquiment AccessoryEquipmnet = null;

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

	public int GetArbaitEnhanceLevel() {return changeStats.nEnhanceArbaitLevel; }
	public void SetArbaitEnhancLevel(int _nValue){changeStats.nEnhanceArbaitLevel = _nValue;}

	public float GetBasicWaterPlus() { return changeStats.fWaterPlus; }
	public void SetBasicWaterPlus(float _fValue) { changeStats.fWaterPlus = _fValue; }

	public float GetBasicRepairPower(){ return changeStats.fRepairPower; }
	public void SetBasicRepairPower(float _fValue) { 
		changeStats.fRepairPower = _fValue;  

		changeStats.fRepairPower = Mathf.Round (changeStats.fRepairPower * 100) * 0.01f;

		SetRepairPower ();
	}

	public float GetBasicArbaitRepairPower() {return changeStats.fArbaitsPower;}
	public void SetBasicArbaitRepairPower(float _fValue){ 
		changeStats.fArbaitsPower = _fValue;
	
		changeStats.fArbaitsPower = Mathf.Round (changeStats.fArbaitsPower * 100) * 0.01f;

		SetArbaitRepairPower ();
	}

	public float GetBasicMaxWaterPlus() { return changeStats.fMaxWaterPlus; }
	public void SetBasicMaxWaterPlus(float _fValue) { changeStats.fMaxWaterPlus = _fValue; }

	public float GetBasicAccuracyRate() { return changeStats.fAccuracyRate; }
	public void SetBasicAccuracyRate(float _fValue) { 

		changeStats.fAccuracyRate = _fValue; 

		changeStats.fAccuracyRate = Mathf.Round (changeStats.fAccuracyRate * 100) * 0.01f;
	
		SetAccuracyRate ();
	}

    public float GetBasicBigSuccessedPercent() { return changeStats.fBigSuccessed; }
	public void SetBasicBigSuccessedPercent(float _fValue) { 

		changeStats.fBigSuccessed = _fValue;

		changeStats.fBigSuccessed = Mathf.Round (changeStats.fBigSuccessed * 100) * 0.01f;
	
		SetBigSuccessed ();
	}

    public float GetBasicCriticalChance() { return changeStats.fCriticalChance; }
	public void SetBasicCriticalChance(float _fValue) { 

		changeStats.fCriticalChance = _fValue; 
	
		SetCriticalChance ();
	}

    public int GetSasinMaterial() { return changeStats.nSasinMaterial; }
    public void SetSasinMaterial(int _nValue) { changeStats.nSasinMaterial = _nValue; }
    public int GetRusiuMaterial() { return changeStats.nRusiuMaterial; }
    public void SetRusiuMaterial(int _nValue) { changeStats.nRusiuMaterial = _nValue; }
    public int GetIceMaterial() { return changeStats.nIceMaterial; }
    public void SetIceMaterial(int _nValue) { changeStats.nIceMaterial = _nValue; }
    public int GetFireMaterial() { return changeStats.nFireMaterial; }
    public void SetFireMaterial(int _nValue) { changeStats.nFireMaterial = _nValue; }

    public int GetDay() { return changeStats.nDay; }
    public void SetDay(int _nValue) { changeStats.nDay = _nValue; }

	public int GetMaxDay() { return changeStats.nMaxDay; }
	public void SetMaxDay(int _nValue) { changeStats.nMaxDay = _nValue; }

    public int GetGuestCount() { return changeStats.nGuestCount; }

	public int GetSuccessedGuestCount() { return changeStats.nSuccessedGuest; }
	public void SetSuccessedGuestCount(int _nValue){changeStats.nSuccessedGuest = _nValue;}

	public int GetFaieldGuestCount()	{ return changeStats.nFaieldGuest; }
	public void SetFaieldGuestCount(int _nValue){ changeStats.nFaieldGuest = _nValue; }
	
	//08.09
	//플레이어 제작 
	public void SetCreatorWeapon(CreatorWeapon _weapon){
		creatorWeapon = _weapon; 

		PlayerStatsSetting ();
	}

	public CreatorWeapon GetCreatorWeapon()
	{
		return creatorWeapon;
	}

	private float m_fRepairPower;
	private float m_fArbaitRepairPower;
	private float m_fGoldPlusPercent;
	private float m_fHonorPlusPercent;
	private float m_fWaterPlus;
	private float m_fCriticalChance;
	private float m_fCriticalDamage;
	private float m_fBigSuccessed;
	private float m_fAccuracyRate;

	public float GetRepairPower(){ return m_fRepairPower; }

	public float GetAccuracyRate(){ return m_fAccuracyRate;}

	public float GetArbaitRepairPower(){ return m_fArbaitRepairPower;}

	public float GetGoldPlusPercent(){	return m_fGoldPlusPercent;}

	public float GetHonorPlusPercent(){ return m_fHonorPlusPercent;}

	public float GetWaterPlus(){ return m_fWaterPlus;}

	public float GetCriticalChance(){ return m_fCriticalChance;}

	public float GetCriticalDamage(){ return m_fCriticalDamage;}

	public float GetBigSuccessed(){ return m_fBigSuccessed;}

	//아이템 이나 캐릭터의 스텟이 바뀌었을때 호출해서 다시 세팅해준다.
	public void PlayerStatsSetting()
	{
		SetRepairPower ();
		SetAccuracyRate ();
		SetArbaitRepairPower ();
		SetGoldPlusPercent ();
		SetHonorPlusPercent ();
		SetWaterPlus ();
		SetCriticalChance ();
		SetCriticalDamage ();
		SetBigSuccessed ();
	}

	public void SetRepairPower () {

		float fResultRepairPowerPercent = 1.0f;

		if (WeaponEquipment != null)	fResultRepairPowerPercent += WeaponEquipment.fReapirPower;
		if (GearEquipmnet != null) 		fResultRepairPowerPercent += GearEquipmnet.fReapirPower;
		if (AccessoryEquipmnet != null) fResultRepairPowerPercent += AccessoryEquipmnet.fReapirPower;
		if (creatorWeapon != null) {
			fResultRepairPowerPercent += creatorWeapon.fRepair;
			fResultRepairPowerPercent += creatorWeapon.fSasinBossValue;
		}

		m_fRepairPower = changeStats.fRepairPower + (changeStats.fRepairPower * fResultRepairPowerPercent * 0.01f);
	}

	public void SetAccuracyRate()
	{
		float fResultAccuracyRatePercent = 1.0f;

		if (WeaponEquipment != null)	fResultAccuracyRatePercent += WeaponEquipment.fAccuracyRate;
		if (GearEquipmnet != null) 		fResultAccuracyRatePercent += GearEquipmnet.fAccuracyRate;
		if (AccessoryEquipmnet != null) fResultAccuracyRatePercent += AccessoryEquipmnet.fAccuracyRate;
		if (creatorWeapon != null) {
			fResultAccuracyRatePercent += creatorWeapon.fAccuracyRate;
			fResultAccuracyRatePercent += creatorWeapon.fIceBossValue;
		}

		m_fAccuracyRate = changeStats.fAccuracyRate + (changeStats.fAccuracyRate * fResultAccuracyRatePercent * 0.01f);
	}

	public void SetArbaitRepairPower()
	{
		float fResultArbaitRepairPower = 1.0f;

		if (WeaponEquipment != null)	fResultArbaitRepairPower += WeaponEquipment.fArbaitRepair;
		if (GearEquipmnet != null) 		fResultArbaitRepairPower += GearEquipmnet.fArbaitRepair;
		if (AccessoryEquipmnet != null) fResultArbaitRepairPower += AccessoryEquipmnet.fArbaitRepair;
		if (creatorWeapon != null) {
			fResultArbaitRepairPower += creatorWeapon.fArbaitRepair;
			fResultArbaitRepairPower += creatorWeapon.fRusiuBossValue;
		}

		m_fArbaitRepairPower = changeStats.fArbaitsPower + (changeStats.fArbaitsPower * fResultArbaitRepairPower * 0.01f);
	}

	public void SetGoldPlusPercent()
	{
		float fResultGoldPlusPercent = 1.0f;

		if (WeaponEquipment != null)	fResultGoldPlusPercent += WeaponEquipment.fGoldPlus;
		if (GearEquipmnet != null) 		fResultGoldPlusPercent += GearEquipmnet.fGoldPlus;
		if (AccessoryEquipmnet != null) fResultGoldPlusPercent += AccessoryEquipmnet.fGoldPlus;
		if (creatorWeapon != null) 		fResultGoldPlusPercent += creatorWeapon.fPlusGoldPercent;

		m_fGoldPlusPercent = changeStats.fGoldPlusPercent + (changeStats.fGoldPlusPercent * fResultGoldPlusPercent * 0.01f);
	}

	public void SetHonorPlusPercent()
	{
		float fResultHonorPlusPercent = 1.0f;

		if (WeaponEquipment != null)	fResultHonorPlusPercent += WeaponEquipment.fHonorPlus;
		if (GearEquipmnet != null) 		fResultHonorPlusPercent += GearEquipmnet.fHonorPlus;
		if (AccessoryEquipmnet != null) fResultHonorPlusPercent += AccessoryEquipmnet.fHonorPlus;
		if (creatorWeapon != null) 		fResultHonorPlusPercent += creatorWeapon.fPlusHonorPercent;

		m_fHonorPlusPercent = changeStats.fHornorPlusPercent + (changeStats.fHornorPlusPercent * fResultHonorPlusPercent * 0.01f);
	}

	public void SetWaterPlus()
	{
		float fResultWaterPlusPercent = 1.0f;

		if (WeaponEquipment != null)	fResultWaterPlusPercent += WeaponEquipment.fWaterChargePlus;
		if (GearEquipmnet != null) 		fResultWaterPlusPercent += GearEquipmnet.fWaterChargePlus;
		if (AccessoryEquipmnet != null) fResultWaterPlusPercent += AccessoryEquipmnet.fWaterChargePlus;
		if (creatorWeapon != null) 		fResultWaterPlusPercent += creatorWeapon.fWaterPlus;

		m_fWaterPlus = changeStats.fWaterPlus + (changeStats.fWaterPlus * fResultWaterPlusPercent * 0.01f);
	}

	public void SetCriticalChance()
	{
		float fResultCriticalChancePercent = 1.0f;

		if (WeaponEquipment != null)	fResultCriticalChancePercent += WeaponEquipment.fCritical;
		if (GearEquipmnet != null) 		fResultCriticalChancePercent += GearEquipmnet.fCritical;
		if (AccessoryEquipmnet != null) fResultCriticalChancePercent += AccessoryEquipmnet.fCritical;
		if (creatorWeapon != null) {
			fResultCriticalChancePercent += creatorWeapon.fCriticalChance;
			fResultCriticalChancePercent += creatorWeapon.fFireBossValue;
		}

		m_fCriticalChance = changeStats.fCriticalChance + (changeStats.fCriticalChance * fResultCriticalChancePercent * 0.01f);
	}

	public void SetCriticalDamage()
	{
		float fResultCriticalDamagePercent = 1.5f;

		if (WeaponEquipment != null)	fResultCriticalDamagePercent += WeaponEquipment.fCriticalDamage;
		if (GearEquipmnet != null) 		fResultCriticalDamagePercent += GearEquipmnet.fCriticalDamage;
		if (AccessoryEquipmnet != null) fResultCriticalDamagePercent += AccessoryEquipmnet.fCriticalDamage;
		if (creatorWeapon != null) 		fResultCriticalDamagePercent += creatorWeapon.fCriticalDamage;

		m_fCriticalDamage = changeStats.fCriticalDamage + (changeStats.fCriticalDamage * fResultCriticalDamagePercent * 0.01f);
	}

	public void SetBigSuccessed()
	{
		float fResultBigSuccessedPercent = 1.0f;

		if (WeaponEquipment != null)	fResultBigSuccessedPercent += WeaponEquipment.fBigCritical;
		if (GearEquipmnet != null) 		fResultBigSuccessedPercent += GearEquipmnet.fBigCritical;
		if (AccessoryEquipmnet != null) fResultBigSuccessedPercent += AccessoryEquipmnet.fBigCritical;
		if (creatorWeapon != null) 		fResultBigSuccessedPercent += creatorWeapon.fBigSuccessed;

		m_fBigSuccessed = changeStats.fBigSuccessed + (changeStats.fBigSuccessed * fResultBigSuccessedPercent * 0.01f);
	}


    public void Init(List<CGameEquiment> _itemList, CGamePlayerData _defaultStats)
    {
        List_items = _itemList;
		changeStats = new CGamePlayerData(_defaultStats);
    }

    public void SetInventroy(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.SetInventory(this, List_items);
    }

	public int GetItemListCount()
	{
		if (inventory == null)
			return 0;

		List_items = inventory.GetItemList ();

		if (List_items == null)
			return 0;

		return List_items.Count;
	}

	/*
	public E_BOSS_WEAPON Check_Equipment()
	{
		if (WeaponEquipment != null)
			return WeaponEquipment.nIndex;

		return null;
	}*/

    //아이템을 장착할 경우
    public void EquipItem(CGameEquiment _item)
	{
		//아이템이 어디 부위인지 확인한다.
		switch (_item.nSlotIndex) {
		case (int)E_EQUIMNET_INDEX.E_WEAPON:

                //만약 무기가 있을 경우 그 무기가 현재 플레이어에 적용되는 값을 빼고 아이템을 넣어줌
                //그 후 다시 아이템 효과를 플레이어에게 적용한다.
			if (WeaponEquipment == _item) {

				WeaponEquipment.bIsEquip = false;

				WeaponEquipment = null;

				PlayerStatsSetting ();

				return;
			}
			else if (WeaponEquipment != null)
				WeaponEquipment.bIsEquip = false;
			

			WeaponEquipment = _item;

			WeaponEquipment.bIsEquip = true;
			break;
		case (int)E_EQUIMNET_INDEX.E_WEAR:

			if (GearEquipmnet == _item) {

				GearEquipmnet.bIsEquip = false;

				GearEquipmnet = null;

				PlayerStatsSetting ();

				return;
			}
			else if (GearEquipmnet != null)
				GearEquipmnet.bIsEquip = false;


			GearEquipmnet = _item;

			GearEquipmnet.bIsEquip = true;
			break;
		case (int)E_EQUIMNET_INDEX.E_ACCESSORY:


			if (AccessoryEquipmnet == _item) {

				AccessoryEquipmnet.bIsEquip = false;

				AccessoryEquipmnet = null;

				PlayerStatsSetting ();

				return;
			}

			else if (AccessoryEquipmnet != null)
				AccessoryEquipmnet.bIsEquip = false;


			AccessoryEquipmnet = _item;

			AccessoryEquipmnet.bIsEquip = true;

			break;
		}
		PlayerStatsSetting ();

		//아이템을 장착했기에 다시 정렬
		inventory.inventorySlots [_item.nSlotIndex].RefreshDisplay ();
	}
}




