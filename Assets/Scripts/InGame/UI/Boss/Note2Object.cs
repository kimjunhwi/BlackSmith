using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Note2Object : MonoBehaviour  ,IPointerDownHandler 
{
	GameObject getInfoGameObject;
	GameObject note3_Left;
	GameObject note3_Right;

	public SimpleObjectPool note2ObjPull;		//해당 오브젝트 풀
	public RectTransform parentTransform;		
	private RectTransform myRectTransform;		

	public RepairObject repairObj;

	//private;
	public float fTime;

	private float fRandomX;
	private float fRandomY;
	private float fMoveSpeed = 200.0f;
	private float fBossSpeed = 5.0f;
	private float fDecreaseWeaponSpeedRate = 0.1f;

	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float skullSizeWidth = 60f;
	private float skullSizeHeight = 80f;

	private SimpleObjectPool note3ObjectPool;

	//tmp value
	Note3Object note3Obj;

	void Start()
	{
		fBossSpeed = 5.0f;
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-2.0f, 2.0f);
		fRandomY = Random.Range (-2.0f, 2.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
		note3ObjectPool = GameObject.Find ("Note3Pool").GetComponent<SimpleObjectPool>();

	}



	void Update()
	{

		transform.Translate ( randomDir * fMoveSpeed * Time.deltaTime);

		//4면 충돌 확인
		if (myRectTransform.anchoredPosition.x >= ((canvasWidth / 2) - (skullSizeWidth / 2))) {
			//Debug.Log ("Right Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.left);
		}

		if (myRectTransform.anchoredPosition.x <= -((canvasWidth / 2) - (skullSizeWidth / 2))) 
		{
			//Debug.Log ("Left Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.right);
		}

		if (myRectTransform.anchoredPosition.y >= (canvasHeight/2) - (skullSizeHeight / 2)) 
		{
			//Debug.Log ("Top Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.down);
		}

		if (myRectTransform.anchoredPosition.y <= -((canvasHeight / 2) - (skullSizeHeight / 2))) {
			//Debug.Log ("Down Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.up);
		}

	}
	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;


		if (getInfoGameObject.gameObject.name == "Note2") 
		{
			
			CreateNote ();
		} else
			return;
	}

	public void CreateNote()
	{
		note3ObjectPool = GameObject.Find ("Note3Pool").GetComponent<SimpleObjectPool>();

		note2ObjPull.ReturnObject (gameObject);
		repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate/2));

		note3_Left = note3ObjectPool.GetObject ();
		note3_Left.name = "Note3";
		note3_Left.transform.SetParent (parentTransform);
		note3_Left.transform.position = new Vector3 (gameObject.transform.position.x - 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		note3Obj = note3_Left.GetComponent<Note3Object> ();
		note3Obj.note3ObjPull = note3ObjectPool;
		note3Obj.parentTransform = parentTransform;
		note3Obj.repairObj = repairObj;
		repairObj.AddBossWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));

		note3_Right = note3ObjectPool.GetObject ();
		note3_Right.name = "Note3";
		note3_Right.transform.SetParent (parentTransform);
		note3_Right.transform.position = new Vector3 (gameObject.transform.position.x + 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		note3Obj = note3_Right.GetComponent<Note3Object> ();
		note3Obj.note3ObjPull = note3ObjectPool;
		note3Obj.parentTransform = parentTransform;
		note3Obj.repairObj = repairObj;
		repairObj.AddBossWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));
	}

	public void EraseObj()
	{
		note2ObjPull.ReturnObject (gameObject);
	}
}
