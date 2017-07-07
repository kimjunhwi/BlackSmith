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
	BossCharacter bossCharacter;		//보스 캐릭터 받는 것
	private float fBossMaxComplete;		//보스 캐릭터 최대 완성도
	GameObject waterObject;				//물 오브젝트
	GameObject bossWeaponObject;		//보스 무기 버튼
	GameObject bossWaterObject;			//보스 물 버튼 

	Image BossWeaponAlphaSprite;
	Image BossWeaponSprite;

	//BossSasinText
	//tmp value
	private GameObject textObj;
	BossMissText bossMissText;
	public RectTransform textRectTrasnform;
	//textPool
	public SimpleObjectPool textObjectPool;
	//RandomPosition
	private float fRandomXPos;
	private float fRandomYPos;
	private float fXPos;
	private float fYPos;

	int nChancePercent = 50;			//미스 확률

	//BossMusic
	private Vector3 randomDir;									//루시오 보스무기가 갈 랜덤 방향
	private Vector3 getReflectDir;
	private float fRandomX;										//랜덤 방향 X
	private float fRandomY;										//랜덤 방향 Y
	private float canvasWidth = 655f;							//체크 할 
	private float canvasHeight = 1100f;
	private float fMoveSpeed;
	private RectTransform bossWeaponRectTransform;
	public RectTransform bossNoteRectTransform;
	public BossMusic bossMusic;
	public NoteObject noteObj;
	public Note2Object note2Obj;
	public Note3Object note3Obj;
	private Transform noteGameObject;							//물 사용시 없어질 노트 obj
	private Vector3 bossWeaponObjOriginPosition;				//원래 수리 패널에 있을때의 무기 위치
	private Vector2 bossWeaponObjOriginSize;					//원래 수리 패널에 터치 인식 범위의 크기
	private Vector2 bossWeaponSize;								//무기 이미지 만큼의 크기
	private bool isMoveWeapon = false;


	void Start()
	{
		bossWeaponObjOriginSize = new Vector2 (590f, 470f);
		bossWeaponSize = new Vector2 (380f, 270f);
		fXPos = textRectTrasnform.position.x;
		fYPos = textRectTrasnform.position.y;

		fRandomXPos = 0;
		fRandomYPos = 0;
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

		bossNoteRectTransform = transform.FindChild ("BossEffectRange").transform.FindChild ("BossMusicNote").
			transform.FindChild ("BossNoteCreateArea").GetComponent<RectTransform> ();

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
		//fWeaponDownDamage += 30;
		fMinusTemperature = player.GetTemperatureMinus ();

		TemperatureSlider.maxValue = fMaxTemperature;
		TemperatureSlider.value = 0;

		WaterSlider.maxValue = fMaxWater;
		WaterSlider.value = 0;

		bossWeaponObjOriginPosition = bossWeaponObject.transform.position;


		bossWeaponRectTransform = bossWeaponObject.GetComponent<RectTransform> ();

		bossWeaponObject.SetActive (false);
		bossWaterObject.SetActive (false);
		WeaponObject.SetActive (true);
		waterObject.SetActive (true);
	}
	public void AddBossWeaponSpeed(float _speed)
	{
		fMoveSpeed += _speed;
		Debug.Log("Plus Cur Speed = " + fMoveSpeed);
	}
	public void MinusWeaponSpeed(float _speed)
	{
		fMoveSpeed -= _speed;
		Debug.Log("Minus Cur Speed = " + fMoveSpeed);
	}

	public IEnumerator BossMusicWeaponMove()
	{
		bossWeaponRectTransform.sizeDelta = bossWeaponSize;
		//무기 초기 스피드
		fMoveSpeed = 5f;		
		
		fRandomX = Random.Range (-2.0f, 2.0f);
		fRandomY = Random.Range (-2.0f, 2.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);

		while (isMoveWeapon == false)
		{

			
			//Debug.Log ("MoveBOssWeapon");
			bossWeaponObject.transform.Translate (randomDir * fMoveSpeed);

			//4면 충돌 확인
			if (bossWeaponRectTransform.anchoredPosition.x >= ((canvasWidth) - bossWeaponRectTransform.sizeDelta.x) - 65f ) {

				getReflectDir = Vector3.Reflect (randomDir, Vector3.left);
				randomDir = new Vector3 (getReflectDir.x, Random.Range (-2.0f, 2.0f), getReflectDir.z);
				//Debug.Log ("Right Collision , Dir = " + randomDir.x + "," + randomDir.y + "," + randomDir.z );
			}

			if (bossWeaponRectTransform.anchoredPosition.x <= -((canvasWidth  - bossWeaponRectTransform.sizeDelta.x) -65f)) {
				
				getReflectDir = Vector3.Reflect (randomDir, Vector3.right);
				randomDir = new Vector3 (getReflectDir.x, Random.Range (-2.0f, 2.0f), getReflectDir.z);
				//Debug.Log ("Left Collision , Dir = " + randomDir.x + "," + randomDir.y + "," + randomDir.z );
			}

			if (bossWeaponRectTransform.anchoredPosition.y >= (canvasHeight) - (bossWeaponRectTransform.sizeDelta.y) - 80f) {

				getReflectDir = Vector3.Reflect (randomDir, Vector3.down);
				randomDir = new Vector3 (Random.Range(-2.0f,2.0f), getReflectDir.y, getReflectDir.z);
				//Debug.Log ("Top Collision , Dir = " + randomDir.x + "," + randomDir.y + "," + randomDir.z );
			}

			if (bossWeaponRectTransform.anchoredPosition.y <= -((canvasHeight) - ((bossWeaponRectTransform.sizeDelta.y * 3f) + 190f))) {
		
				getReflectDir = Vector3.Reflect (randomDir, Vector3.up);
				randomDir = new Vector3 (Random.Range(-2.0f,2.0f), getReflectDir.y, getReflectDir.z);

				//Debug.Log ("Bottom Collision , Dir = " + randomDir.x + "," + randomDir.y + "," + randomDir.z );
			}
		

			yield return null;
		}
		yield break;
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

	public void GetBossWeapon(Sprite _sprite, float _fMaxBossComplete ,float _fComplate,
		float _fTemperator , BossCharacter _bossData)
	{
		Debug.Log ("Arbait Get Damage!");
		//fWeaponDownDamage += SpawnManager.Instance.GetActiveArbaitRepair ();

		fWeaponDownDamage = player.GetRepairPower ();
		bossWeaponObject.SetActive (true);
		bossWaterObject.SetActive (true);

		WeaponObject.SetActive (false);
		waterObject.SetActive (false);


		if (_bossData.nIndex == 0)
			bossCharacter = _bossData;
		else if (_bossData.nIndex == 1)
			bossCharacter = _bossData;
		
			
		else if (_bossData.nIndex == 2)
			bossCharacter = _bossData;
		else if (_bossData.nIndex == 3)
			bossCharacter = _bossData;
		else
			return;

		if (bossCharacter.nIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) 
		{
			isMoveWeapon = false;
			StartCoroutine (BossMusicWeaponMove ());
		}
		//input Image
		BossWeaponSprite.sprite = _sprite;
		//weaponData = data;

		fMaxTemperature = bossCharacter.bossInfo.fComplate * 0.3f;
		TemperatureSlider.maxValue = fMaxTemperature;

		fCurrentComplate = _fComplate;
		fBossMaxComplete = bossCharacter.bossInfo.fComplate ;
		ComplateSlider.maxValue = _fMaxBossComplete;
		ComplateSlider.value = fCurrentComplate;

		fCurrentTemperature = _fTemperator;
		ComplateText.text = string.Format("{0} / {1}", _fComplate, bossCharacter.bossInfo.fComplate);
	}

    //무기터치
    public void TouchWeapon()
    {
        if (weaponData == null)
            return;
		
		fCurrentComplate = fCurrentComplate + player.GetRepairPower();

        //크리티컬 확률 
        if( Random.Range(1, 100) <= Mathf.Round(player.GetCriticalChance()))
        {
            SpawnManager.Instance.PlayerCritical();
            fCurrentComplate *= 1.5f;
        }

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

		fRandomXPos = Random.Range (fXPos - (textRectTrasnform.sizeDelta.x/2), fXPos + (textRectTrasnform.sizeDelta.x/2));
		fRandomYPos = Random.Range (fYPos - (textRectTrasnform.sizeDelta.y/2), fYPos + (textRectTrasnform.sizeDelta.y/2));

		if (bossCharacter == null)
			return;
		//Sasin
		if (bossCharacter.nIndex == 1 ) 
		{ 
			if (bossCharacter.eCureentBossState < Character.EBOSS_STATE.PHASE_01) {
				Debug.Log ("SasinPhase00");
				fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
				fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);

			}
			else if (bossCharacter.eCureentBossState >= Character.EBOSS_STATE.PHASE_01 && bossCharacter.eCureentBossState < Character.EBOSS_STATE.PHASE_02) {
				Debug.Log ("SasinPhase01");
				if (nRandom <= nChancePercent) {
					Debug.Log ("SasinPhase01");
					fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
					fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);
				} else {
					Debug.Log ("Miss");

					textObj = textObjectPool.GetObject ();
					textObj.transform.SetParent (textRectTrasnform.transform);
					textObj.transform.position = new Vector3 (fRandomXPos, fRandomYPos, textObj.transform.position.z);
					textObj.name ="Miss";

					bossMissText = textObj.GetComponent<BossMissText> ();
					bossMissText.textObjPool = textObjectPool;
					bossMissText.leftSecond = 2.0f;
					bossMissText.parentTransform = textRectTrasnform;
				}
			} 
			else if (bossCharacter.eCureentBossState >= Character.EBOSS_STATE.PHASE_02) 
			{
				Debug.Log ("SasinPhase02");

				if (nRandom <= nChancePercent) {
					//Debug.Log ("SasinPhase02");
					fWeaponDownDamage -= (fWeaponDownDamage * 0.3f);
					fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
					fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);
					fWeaponDownDamage = 40;
				} else {
					Debug.Log ("Miss");
					textObj = textObjectPool.GetObject ();
					textObj.transform.SetParent (textRectTrasnform.transform);
					textObj.transform.position = new Vector3 (fRandomXPos, fRandomYPos, 0);
					textObj.name ="Miss";

					bossMissText = textObj.GetComponent<BossMissText> ();
					bossMissText.textObjPool = textObjectPool;
					bossMissText.leftSecond = 2.0f;
					bossMissText.parentTransform = textRectTrasnform;
				}
					
			}
		}

		//MusicMan
		if (bossCharacter.nIndex == 3 ) 
		{ 
			if (bossCharacter.eCureentBossState < Character.EBOSS_STATE.PHASE_01) {
				Debug.Log ("MusicPhase00");
				fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
				fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);
			}
			else if (bossCharacter.eCureentBossState >= Character.EBOSS_STATE.PHASE_01 && bossCharacter.eCureentBossState < Character.EBOSS_STATE.PHASE_02) {

				fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
				fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);
			} 
			else if (bossCharacter.eCureentBossState >= Character.EBOSS_STATE.PHASE_02) {
				fWeaponDownDamage -= (fWeaponDownDamage * 0.3f);
				fCurrentComplate = fCurrentComplate + fWeaponDownDamage;
				fCurrentTemperature += (((fWeaponDownDamage * fMaxTemperature) / bossCharacter.bossInfo.fComplate) * (1 + (fCurrentTemperature / fMaxTemperature) * 1.5f)) + (bossCharacter.bossInfo.fComplate * 0.01f);
				fWeaponDownDamage = 40;
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
		
		ComplateText.text = string.Format("{0} / {1}", ComplateSlider.value, bossCharacter.bossInfo.fComplate);



	}

    public void TouchWater()
    {
		if (weaponData == null)
			return;

        if (fCurrentWater >= fUseWater)
        {

			SpawnManager.Instance.UseWater ();

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

			//물 터치시 노트 한단계씩 떨어진다.
			if (bossCharacter.nIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) 
			{
				bossNoteRectTransform = transform.FindChild ("BossEffectRange").transform.FindChild ("BossMusicNote").
					transform.FindChild ("BossNoteCreateArea").GetComponent<RectTransform> ();
				int nChildCount = bossNoteRectTransform.childCount;
			
				while(nChildCount != 0)
				{
					
					Debug.Log ("CurCount = " + nChildCount);
					if (bossNoteRectTransform.FindChild("Note")) {
						
						noteGameObject = bossNoteRectTransform.FindChild("Note");
						noteObj = noteGameObject.gameObject.GetComponent<NoteObject> ();
						noteObj.CreateNote ();

					} else if (bossNoteRectTransform.FindChild("Note2")) {
						noteGameObject = bossNoteRectTransform.FindChild("Note2");
						note2Obj = noteGameObject.gameObject.GetComponent<Note2Object> ();
						note2Obj.CreateNote ();
					} else if (bossNoteRectTransform.FindChild("Note3")) {
						Debug.Log ("Delete Note");
						noteGameObject = bossNoteRectTransform.FindChild("Note3");
						note3Obj = noteGameObject.gameObject.GetComponent<Note3Object> ();
						note3Obj.EraseObj ();
					} 
					nChildCount--;
				}
			}
			//useWater
			fMinusTemperature = (fMaxTemperature * 0.3f) * (1 + fWeaponDownTemperature);

			fMinusWater = ((1 + (fCurrentComplate / fMinusTemperature) * fWeaponDownDamage) * (1 + (fUseWater * 0.01f) + fWeaponDownTemperature));

			fCurrentWater -= fMinusTemperature;
			fCurrentTemperature -= fMinusTemperature;

			fCurrentComplate += fMinusWater;

			WaterSlider.value = fCurrentWater;

			if (fCurrentComplate > bossCharacter.bossInfo.fComplate)
				fCurrentComplate = bossCharacter.bossInfo.fComplate;

			if (fCurrentWater < 0)
				fCurrentWater = 0;

			if (fCurrentTemperature < 0)
				fCurrentTemperature = 0;

			int nCurComplete = (int)fCurrentComplate;

			ComplateSlider.value = (float)nCurComplete;
			TemperatureSlider.value = fCurrentTemperature;

			ComplateText.text = string.Format ("{0} / {1}", ComplateSlider.value, bossCharacter.bossInfo.fComplate);

			if (ComplateSlider.value >= bossCharacter.bossInfo.fComplate) {
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

		int nCurComplete = (int)fCurrentComplate;


		ComplateText.text = string.Format("{0} / {1}", nCurComplete, ComplateSlider.maxValue);
	}
	public void SetFinishBoss()
	{
		StopCoroutine (BossMusicWeaponMove ());
		bossWeaponObject.transform.position = bossWeaponObjOriginPosition;
		bossWeaponRectTransform.sizeDelta = bossWeaponObjOriginSize;

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
		fUseWater = 10.0f;
		fPlusWater = player.GetWaterPlus ();
		fMaxWater = player.GetMaxWaterPlus ();
		fWeaponDownDamage = player.GetRepairPower ();

		fMinusTemperature = player.GetTemperatureMinus ();

		TemperatureSlider.maxValue = fMaxTemperature;
		TemperatureSlider.value = 0;

		WaterSlider.maxValue = fMaxWater;
		WaterSlider.value = 0;

		WeaponSprite = null;
		ComplateText.text = string.Format ("{0} / {1}", fCurrentComplate, ComplateSlider.maxValue);


		//물 터치시 노트 한단계씩 떨어진다.
		if (bossCharacter.nIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) {
			bossNoteRectTransform = transform.FindChild ("BossEffectRange").transform.FindChild ("BossMusicNote").
				transform.FindChild ("BossNoteCreateArea").GetComponent<RectTransform> ();
			int nChildCount = bossNoteRectTransform.childCount;

			while (nChildCount != 0) {

				Debug.Log ("CurCount = " + nChildCount);
				if (bossNoteRectTransform.FindChild ("Note")) {

					noteGameObject = bossNoteRectTransform.FindChild ("Note");
					noteObj = noteGameObject.gameObject.GetComponent<NoteObject> ();
					noteObj.EraseObj ();

				} else if (bossNoteRectTransform.FindChild ("Note2")) {
					noteGameObject = bossNoteRectTransform.FindChild ("Note2");
					note2Obj = noteGameObject.gameObject.GetComponent<Note2Object> ();
					note2Obj.EraseObj ();
				} else if (bossNoteRectTransform.FindChild ("Note3")) {
					
					noteGameObject = bossNoteRectTransform.FindChild ("Note3");
					note3Obj = noteGameObject.gameObject.GetComponent<Note3Object> ();
					note3Obj.EraseObj ();
				} 
				nChildCount--;
			}
			isMoveWeapon = true;
		}
	}
}
