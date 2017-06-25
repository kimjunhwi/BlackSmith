using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairObject : MonoBehaviour {

    public Slider ComplateSlider;
    public Slider TemperatureSlider;
    public Slider WaterSlider;

    public Text ComplateText;

    //진행중인 오브젝트
    GameObject AfootObject;

	GameObject WeaponObject;

	private float fCurrentComplate = 0;
    private float fWeaponDownDamage = 70.0f;
    private float fWeaponDownTemperature = 0;
    private float fMaxTemperature;
    private float fCurrentTemperature= 0;
    private float fDownTemperature = 0;

    public float fMaxWater;         //최고치
    public float fCurrentWater;     //남은 량
    public float fUseWater;         //사양되는 량
    public float fPlusWater;        //충전되는 량


    private float fMinusTemperature;
    private float fMinusWater;


	Image WeaponSprite;
    Image WeaponAlphaSpirte;

    CGameWeaponInfo weaponData;

	Player player;

	void Start()
	{
		WaterSlider.minValue = 0;
		ComplateSlider.minValue = 0;
		TemperatureSlider.minValue = 0;

		WaterSlider.maxValue = 0;
		ComplateSlider.maxValue = 0;
		TemperatureSlider.maxValue = 0;


        WeaponObject = transform.FindChild("WeaponButton").gameObject;

        WeaponAlphaSpirte = WeaponObject.transform.GetChild(0).GetComponent<Image>();
        WeaponSprite = WeaponObject.transform.GetChild(1).GetComponent<Image>();



        this.StartCoroutine(this.PlusWater());

		player = GameManager.Instance.player;

		fCurrentWater = 0f;
		fUseWater  = 10.0f;
		fPlusWater = player.GetWaterPlus ();
		fMaxWater = player.GetMaxWaterPlus();
		fWeaponDownDamage = player.GetRepairPower ();
		fMinusTemperature = player.GetTemperatureMinus ();

		TemperatureSlider.maxValue = fMaxTemperature;
		TemperatureSlider.value = 0;

		WaterSlider.maxValue = fMaxWater;
		WaterSlider.value = 0;
	}

    IEnumerator PlusWater()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);

            if(fMaxWater > fCurrentWater)
            {
                fCurrentWater += fPlusWater;
                WaterSlider.value = fCurrentWater;
            }

            if (fCurrentTemperature > 0)
            {
				fDownTemperature = (fMaxTemperature - fCurrentTemperature) * 0.1f;

                fCurrentTemperature -= fDownTemperature;

				if (fCurrentTemperature < 0)
					fCurrentTemperature = 0;

                TemperatureSlider.value = fCurrentTemperature;
            }
        }
    }

    public void GetWeapon(GameObject obj, CGameWeaponInfo data, float _fComplate, float _fTemperator)
    {
        if (AfootObject == obj)
            return;

        if(weaponData == null)
        {
            AfootObject = obj;
            
            weaponData = data;
        }
        else
        {
			SpawnManager.Instance.ReturnInsertData(AfootObject, fCurrentComplate, fCurrentTemperature);

            AfootObject = obj;

            weaponData = data;
        }

        fMaxTemperature = weaponData.fComplate * 0.3f;
        TemperatureSlider.maxValue = fMaxTemperature;

		fCurrentComplate = _fComplate;
        ComplateSlider.maxValue = weaponData.fComplate;
		ComplateSlider.value = fCurrentComplate;

        fCurrentTemperature = _fTemperator;
		WeaponSprite.sprite = weaponData.WeaponSprite;

        ComplateText.text = string.Format("{0} / {1}", _fComplate, weaponData.fComplate);
    }

    //무기터치
    public void TouchWeapon()
    {
        if (weaponData == null)
            return;

		fCurrentComplate = fCurrentComplate + fWeaponDownDamage;

        fCurrentTemperature += ((fWeaponDownDamage * fMaxTemperature) / weaponData.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f);

		if(fCurrentComplate >= ComplateSlider.maxValue)
        {
            SpawnManager.Instance.ComplateCharacter(AfootObject,weaponData.fComplate);
            return;
        }

        if(fCurrentTemperature >= fMaxTemperature)
        {
            SpawnManager.Instance.ComplateCharacter(AfootObject, weaponData.fComplate);
            //무기 실패취급으로 리턴

            return;
        }

		ComplateSlider.value = fCurrentComplate;
        TemperatureSlider.value = fCurrentTemperature;

        ComplateText.text = string.Format("{0} / {1}", ComplateSlider.value, weaponData.fComplate);
    }

    public void TouchWater()
    {
		if (weaponData == null)
			return;

        if (fCurrentWater >= fUseWater)
        {
            //useWater
            fMinusTemperature = (fMaxTemperature * 0.3f) * (1 + fWeaponDownTemperature);

			fMinusWater = ((1 + (fCurrentComplate / fMinusTemperature) * fWeaponDownDamage) * (1 + fUseWater + fMaxTemperature));

            fCurrentWater -= fMinusWater;

			fCurrentComplate += fMinusWater;

            WaterSlider.value = fCurrentWater;

			if (fCurrentComplate > weaponData.fComplate)
				fCurrentComplate = weaponData.fComplate;

			if (fCurrentWater < 0)
				fCurrentWater = 0;

			ComplateSlider.value = fCurrentComplate;

            ComplateText.text = string.Format("{0} / {1}", ComplateSlider.value, weaponData.fComplate);

            if(ComplateSlider.value >= weaponData.fComplate)
                SpawnManager.Instance.ComplateCharacter(AfootObject, weaponData.fComplate);
        }
    }

    //만약 수리중에 대기시간이 다 지나서 되돌아갈때 확인함
    public void CheckMyObject(GameObject _obj)
    {
        if (_obj == AfootObject)
        {
			SpawnManager.Instance.ReturnInsertData(AfootObject, fCurrentComplate, TemperatureSlider.value);
            InitWeaponData();
        }
    }

    //전체 초기화
    private void InitWeaponData()
    {
        weaponData = null;
        AfootObject = null;

        WeaponSprite.sprite = null;
        WeaponAlphaSpirte.sprite = null;
        
		fCurrentComplate = 0;

        ComplateSlider.value = 0;
        ComplateSlider.maxValue = 0;
        TemperatureSlider.value = 0;
        fCurrentTemperature = 0;

		ComplateText.text = string.Format("{0} / {1}", fCurrentComplate, ComplateSlider.maxValue);
    }
}
