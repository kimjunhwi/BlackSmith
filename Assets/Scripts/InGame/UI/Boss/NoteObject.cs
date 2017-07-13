using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NoteObject : MonoBehaviour  ,IPointerDownHandler 
{
	GameObject getInfoGameObject;				//touch시 가져오는 노트의 데이터
	GameObject note2_Left;
	GameObject note2_Right;

	public SimpleObjectPool noteObjPull;		//해당 오브젝트 풀
	public RectTransform parentTransform;		
	private RectTransform myRectTransform;		
	private BossMusic bossMusic;
	public RepairObject repairObj;

	//private;
	public float fTime;

	private float fRandomX;
	private float fRandomY;
	public  float fMoveSpeed = 200.0f;
	public  float fBossSpeed = 5.0f;
	private float fDecreaseWeaponSpeedRate = 0.1f;


	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float noteSizeWidth = 128.0f;
	private float noteSizeHeight = 128.0f;

	private SimpleObjectPool note2ObjectPool;

	//tmp value
	Note2Object note2Obj;

	void Start()
	{
		fBossSpeed = 5.0f;
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-2.0f, 2.0f);
		fRandomY = Random.Range (-2.0f, 2.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
		note2ObjectPool = GameObject.Find ("Note2Pool").GetComponent<SimpleObjectPool>();
		bossMusic = GameObject.Find ("BossMusic").GetComponent<BossMusic> ();
	}



	void Update()
	{
		
	

		transform.Translate ( randomDir * fMoveSpeed * Time.deltaTime);

		//4면 충돌 확인
		if (myRectTransform.anchoredPosition.x >= (((canvasWidth / 2) - (noteSizeWidth / 2)) + 42f))
		{
			//Debug.Log ("Right Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.left);
		}

		if (myRectTransform.anchoredPosition.x <= -(((canvasWidth / 2) - (noteSizeWidth / 2)) + 29f))
		{
			//Debug.Log ("Left Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.right);
		}

		if (myRectTransform.anchoredPosition.y >= (((canvasHeight/2) - (noteSizeHeight / 2)) + 18f))
		{
			//Debug.Log ("Top Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.down);
		}

		if (myRectTransform.anchoredPosition.y <= -(((canvasHeight / 2) - (noteSizeHeight / 2)) - 15f)) 
		{
			//Debug.Log ("Down Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.up);
		}

	}
	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

	

		if (getInfoGameObject.gameObject.name == "Note") 
		{
			CreateNote ();
		} 
		else
			return;

	}

	public void CreateNote()
	{

		noteObjPull.ReturnObject (gameObject);
		repairObj.MinusWeaponSpeed (fBossSpeed * fDecreaseWeaponSpeedRate);

		note2ObjectPool = GameObject.Find ("Note2Pool").GetComponent<SimpleObjectPool>();


		note2_Left = note2ObjectPool.GetObject ();
		note2_Left.name = "Note2";
		note2_Left.transform.SetParent (parentTransform,false);
		note2_Left.transform.localScale = Vector3.one;
		note2_Left.transform.position = new Vector3 (gameObject.transform.position.x - 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		//예외 처리
		if(note2_Left.transform.position.x <= -332.0f)
			note2_Left.transform.position = new Vector3 (gameObject.transform.position.x + 96f, gameObject.transform.position.y,
				gameObject.transform.position.z);



		note2Obj = note2_Left.GetComponent<Note2Object> ();
		note2Obj.note2ObjPull = note2ObjectPool;
		note2Obj.parentTransform = parentTransform;
		note2Obj.repairObj = repairObj;
		repairObj.AddBossWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 2));

		note2_Right = note2ObjectPool.GetObject ();
		note2_Right.name = "Note2";
		note2_Right.transform.SetParent (parentTransform, false);
		note2_Right.transform.localScale = Vector3.one;
		note2_Right.transform.position = new Vector3 (gameObject.transform.position.x + 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		if(note2_Right.transform.position.x >= 323.0f)
			note2_Right.transform.position = new Vector3 (gameObject.transform.position.x - 96f, gameObject.transform.position.y,
				gameObject.transform.position.z);


		note2Obj = note2_Right.GetComponent<Note2Object> ();
		note2Obj.note2ObjPull = note2ObjectPool;
		note2Obj.parentTransform = parentTransform;
		note2Obj.repairObj = repairObj;
		repairObj.AddBossWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 2));

		bossMusic.nNoteCount--;
	}

	public void EraseObj()
	{
		noteObjPull.ReturnObject (gameObject);
	}
}
