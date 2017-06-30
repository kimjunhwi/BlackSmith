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

	private float fCurrentComplate = 0;				//현재완성도
    private float fWeaponDownDamage = 70.0f;		//현재무기 데미지
    private float fWeaponDownTemperature = 0;		//무기 수리시 올라가는 온도
    private float fMaxTemperature;					//최대 온도
    private float fCurrentTemperature= 0;			//현재 온도
    private float fDownTemperature = 0;				//떨어지는 온도3

    public float fMaxWater;         //최고치
    public float fCurrentWater;     //남은 량
    public float fUseWater;         //사용되는 가능 용량
    public float fPlusWater;        //충전되는 량


    private float fMinusTemperature;
    private float fMinusWater;

	Image WeaponSprite;
    Image WeaponAlphaSpirte;



    CGameWeaponInfo weaponData;

	Player player;

	//Boss
	Boss bossCharacter;					//보스 캐릭터 받는 것
	private float fBossMaxComplete;		//보스 캐릭터 최대 완성도
	GameObject waterObject;				//물 오브젝트
	GameObject bossWeaponObject;		//보스 무기 버튼
	GameObject bossWaterObject;			//보스 물 버튼 

	BossSasin bossSasin;

	int nChancePercent = 100;			//성공확률

	Image BossWeaponAlphaSprite;
	Image BossWeaponSprite;


	void Start()
	{
		WaterSlider.minValue = 0;
		ComplateSlider.minValue = 0;
		TemperatureSlider.minValue = 0;

		WaterSlider.maxValue = 0;
		ComplateSlider.maxValue = 0;
		TemperatureSlider.maxValue = 0;

		//Add GameObject(button)
        WeaponObject = transform.FindChild("WeaponButton").gameObject;
		waterObject = transform.FindChild ("WaterButton").gameObject;

		bossWeaponObject = transform.FindChild ("BossWeaponButton").gameObject;
		bossWaterObject = transform.FindChild ("BossWaterButton").gameObject;

        WeaponAlphaSpirte = WeaponObject.transform.GetChild(0).GetComponent<Image>();
        WeaponSprite = WeaponObject.transform.GetChild(1).GetComponent<Image>();


		BossWeaponAlphaSprite = bossWeaponObject.transform.GetChild (0).GetComponent<Image> ();
		BossWeaponSprite = bossWeaponObject.transform.GetChild (1).GetComponent<Image> ();


        this.StartCoroutine(this.PlusWater());

		player = GameManager.Instance.player;

		fCurrentWater = 0f;
		fUseWater  = 10.0f;

		fPlusWater = player.GetWaterPlus ();
		fMaxWater = player.GetMaxWaterPlus();
		fWeaponDownDamage = player.GetRepairPower ();
		fWeaponDownDamage += 30;
		fMinusTemperature = player.GetTemperatureMinus ();

		TemperatureSlider.maxValue = fMaxTemperature;
		TemperatureSlider.value = 0;

		WaterSlider.maxValue = fMaxWater;
		WaterSlider.value = 0;


		bossWeaponObject.SetActive (false);
		bossWaterObject.SetActive (false);
		WeaponObject.SetActive (true);
		waterObject.SetActive (true);
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
				fDownTemperature = (fMaxTemperature - fCurrentTemperature) * 0.05f;

                fCurrentTemperature -= fDownTemperature;

				if (fCurrentTemperature < 0)
					fCurrentTemperature = 0;

                TemperatureSlider.value = fCurrentTemperature;
            }
        }
    }

    public void GetWeapon(GameObject obj, CGameWeaponInfo data, float _fComplate, float _fTemperator)
    {
		bossWeaponObject.SetActive (false);
		bossWaterObject.SetActive (false);
		WeaponObject.SetActive (true);
		waterObject.SetActive (true);

        if (AfootObject == obj)
            return;

        if(weaponData == null)
        {
            AfootObject = obj;
            
            weaponData = data;
        }
        else
        {
			SpawnManager.Instance.ReturnInsertData(AfootObject,false, fCurrentComplate, fCurrentTemperature);

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

	public void GetBossWeapon(CGameWeaponInfo data, float _fMaxBossComplete ,float _fComplate,
		float _fTemperator , Boss _bossData, BossSasin _bossSasinData)
	{

		bossWeaponObject.SetActive (true);
		bossWaterObject.SetActive (true);
		WeaponObject.SetActive (false);
		waterObject.SetActive (false);


		if (_bossData.nIndex == 0)
			bossCharacter = _bossData;
		else if (_bossData.nIndex == 1)
		{
			bossCharacter = _bossData;
			bossSasin = _bossSasinData;
		}
			
		else if (_bossData.nIndex == 2)
			bossCharacter = _bossData;
		else if (_bossData.nIndex == 3)
			bossCharacter = _bossData;
		else
			return;

		BossWeaponSprite.sprite = data.WeaponSprite;
		//weaponData = data;

		fMaxTemperature = bossCharacter.fComplate * 0.3f;
		TemperatureSlider.maxValue = fMaxTemperature;

		fCurrentComplate = _fComplate;
		fBossMaxComplete = bossCharacter.fComplate ;
		ComplateSlider.maxValue = _fMaxBossComplete;
		ComplateSlider.value = fCurrentComplate;

		fCurrentTemperature = _fTemperator;
		ComplateText.text = string.Format("{0} / {1}", _fComplate, bossCharacter.fComplate);
	}

    //무기터치
    public void TouchWeapon()
    {
        if (weaponData == null)
            return;
		
		fCurrentComplate = fCurrentComplate + player.GetRepairPower();

        fCurrentTemperature += ((fWeaponDownDamage * fMaxTemperature) / weaponData.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f);

		SpawnManager.Instance.CheckComplateWeapon (AfootObject, fCurrentComplate);

        if(fCurrentTemperature >= fMaxTemperature)
        {
			fCurrentTemperature = 0.0f;
			fCurrentComplate = (fCurrentComplate) - weaponData.fComplate * 0.3f;

			if (fCurrentComplate > 0)
				SpawnManager.Instance.CheckComplateWeapon (AfootObject,fCurrentComplate);
			
			else
				SpawnManager.Instance.ComplateCharacter (AfootObject, fCurrentComplate);
        }

		if (AfootObject == null)
			return;

		ComplateSlider.value = fCurrentComplate;
        TemperatureSlider.value = fCurrentTemperature;
	    
		ComplateText.text = string.Format("{0} / {1}", (int)ComplateSlider.value, weaponData.fComplate);
	

    }

	public void TouchBossWeapon()
	{
		int nRandom = Random.Range (0, 100);

		if (bossCharacter == null)
			return;
		//Sasin
		if (bossCharacter.nIndex == 1 ) 
		{ 
			if (bossSasin.eCureentBossState < Character.EBOSS_STATE.PHASE_01) {
				Debug.Log ("SasinPhase00");
				fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
				fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.fComplate * 0.01f);

			}
			else if (bossSasin.eCureentBossState >= Character.EBOSS_STATE.PHASE_01 && bossSasin.eCureentBossState < Character.EBOSS_STATE.PHASE_02) {
				Debug.Log ("SasinPhase01");
				if (nRandom <= nChancePercent) {
					Debug.Log ("SasinPhase01");
					fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
					fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.fComplate * 0.01f);
				}
				else
					Debug.Log ("Miss");
			} 
			else if (bossSasin.eCureentBossState >= Character.EBOSS_STATE.PHASE_02) 
			{
				Debug.Log ("SasinPhase02");

				if (nRandom <= nChancePercent) 
				{
					//Debug.Log ("SasinPhase02");
					fWeaponDownDamage -= (fWeaponDownDamage * 0.3f);
					fCurrentComplate = fCurrentComplate  + fWeaponDownDamage;
					fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.fComplate * 0.01f);
					fWeaponDownDamage = 40;
				}

				else
					Debug.Log ("Miss");
			}
		}
			
		//온도
		if(fCurrentTemperature >= fMaxTemperature)
		{
			//SpawnManager.Instance.ComplateCharacter(AfootObject, weaponData.fComplate);
			//무기 실패취급으로 리턴
			fCurrentComplate -= (fMaxTemperature * 0.3f);  
			fCurrentTemperature = 0;

			TemperatureSlider.value = fCurrentTemperature;

			if (fCurrentComplate <= 0)
			{
				SpawnManager.Instance.bIsBossCreate = false;
				return;
			}
							
		}


		int nCurComplete = (int)fCurrentComplate;

		ComplateSlider.value = (float)nCurComplete;

		TemperatureSlider.value = fCurrentTemperature;
		
		ComplateText.text = string.Format("{0} / {1}", ComplateSlider.value, bossCharacter.fComplate);



	}

    public void TouchWater()
    {
		if (weaponData == null)
			return;

        if (fCurrentWater >= fUseWater)
        {
            //useWater
            fMinusTemperature = (fMaxTemperature * 0.3f) * (1 + fWeaponDownTemperature);

			fMinusWater = ((1 + (fCurrentComplate / fMinusTemperature) * fWeaponDownDamage) * (1 + (fUseWater * 0.01f)  + fWeaponDownTemperature));

			fCurrentWater -= fMinusTemperature;

			fCurrentComplate += fMinusWater;

            WaterSlider.value = fCurrentWater;

			fCurrentTemperature -= fMinusTemperature;

			if (fCurrentComplate > weaponData.fComplate)
				fCurrentComplate = weaponData.fComplate;

			if (fCurrentWater < 0)
				fCurrentWater = 0;

			if (fCurrentTemperature < 0)
				fCurrentTemperature = 0;

			int nCurComplete = (int)fCurrentComplate;

			ComplateSlider.value = (float)nCurComplete;
			TemperatureSlider.value = fCurrentTemperature;

            ComplateText.text = string.Format("{0} / {1}", ComplateSlider.value, weaponData.fComplate);

			if(nCurComplete >= weaponData.fComplate)
                SpawnManager.Instance.ComplateCharacter(AfootObject, weaponData.fComplate);
        }
    }

	public void TouchBossWater()
	{
		if (bossCharacter == null)
			return;

		if (fCurrentWater >= fUseWater) {
			//useWater
			fMinusTemperature = (fMaxTemperature * 0.3f) * (1 + fWeaponDownTemperature);

			fMinusWater = ((1 + (fCurrentComplate / fMinusTemperature) * fWeaponDownDamage) * (1 + (fUseWater * 0.01f) + fWeaponDownTemperature));

			fCurrentWater -= fMinusTemperature;
			fCurrentTemperature = fMinusTemperature;

			fCurrentComplate += fMinusWater;

			WaterSlider.value = fCurrentWater;

			if (fCurrentComplate > bossCharacter.fComplate)
				fCurrentComplate = bossCharacter.fComplate;

			if (fCurrentWater < 0)
				fCurrentWater = 0;

			if (fCurrentTemperature < 0)
				fCurrentTemperature = 0;

			int nCurComplete = (int)fCurrentComplate;

			ComplateSlider.value = (float)nCurComplete;
			TemperatureSlider.value = fCurrentTemperature;

			ComplateText.text = string.Format ("{0} / {1}", ComplateSlider.value, bossCharacter.fComplate);

			if (ComplateSlider.value >= bossCharacter.fComplate) {
				SpawnManager.Instance.bIsBossCreate = false;
				//bossCharacter.
			}
		}
	}

    //만약 수리중에 대기시간이 다 지나서 되돌아갈때 확인함
    public void CheckMyObject(GameObject _obj)
    {
        if (_obj == AfootObject)
        {
			SpawnManager.Instance.ReturnInsertData(AfootObject,false, fCurrentComplate, TemperatureSlider.value);
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

	//현재무기의 완성도를 가져온다
	public float GetCurCompletion()
	{
		return fCurrentComplate;
	}


	public void SetCurCompletion(float _value)
	{
		fCurrentComplate -= _value;
		ComplateSlider.value = fCurrentComplate;
		ComplateText.text = string.Format("{0} / {1}", fCurrentComplate, ComplateSlider.maxValue);
	}
	public void SetFinishBoss()
	{
		bossWeaponObject.SetActive (false);
		bossWaterObject.SetActive (false);
		WeaponObject.SetActive (true);
		waterObject.SetActive (true);

		fCurrentComplate = 0;
		WaterSlider.minValue = 0;
		ComplateSlider.minValue = 0;
		TemperatureSlider.minValue = 0;

		WaterSlider.maxValue = 0;
		ComplateSlider.maxValue = 0;
		TemperatureSlider.maxValue = 0;

		fCurrentWater = 0f;
		fCurrentTemperature = 0f;
		fUseWater  = 10.0f;
		fPlusWater = player.GetWaterPlus ();
		fMaxWater = player.GetMaxWaterPlus();
		fWeaponDownDamage = player.GetRepairPower ();

		fMinusTemperature = player.GetTemperatureMinus ();

		TemperatureSlider.maxValue = fMaxTemperature;
		TemperatureSlider.value = 0;

		WaterSlider.maxValue = fMaxWater;
		WaterSlider.value = 0;

		WeaponSprite = null;
		ComplateText.text = string.Format("{0} / {1}", fCurrentComplate, ComplateSlider.maxValue);
	}

}
