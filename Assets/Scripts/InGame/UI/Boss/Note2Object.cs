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
	private float fMoveSpeed = 10.0f;
	private float fBossSpeed = 5.0f;
	private float fDecreaseWeaponSpeedRate = 0.1f;

	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float noteSizeWidth = 96f;
	private float noteSizeHeight = 96f;

	private SimpleObjectPool note3ObjectPool;
	public GameObject bossWeapon_Obj;

	//tmp value
	Note3Object note3Obj;



	public void StartNoteObjMove()
	{
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-0.5f, 0.5f);
		fRandomY = Random.Range (-0.5f, 0.5f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
		randomDir.Normalize ();
		note3ObjectPool = GameObject.Find ("Note3Pool").GetComponent<SimpleObjectPool>();

		StartCoroutine (NoteObjMove ());
	}

	IEnumerator NoteObjMove()
	{
		while (true) 
		{
			transform.Translate (fMoveSpeed * randomDir);

			//4면 충돌 확인
			if (myRectTransform.anchoredPosition.x >= (((canvasWidth / 2) - (noteSizeWidth / 2)) + 11f )) {
				//Debug.Log ("Right Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.left);
			}

			else if (myRectTransform.anchoredPosition.x <= -(((canvasWidth / 2) - (noteSizeWidth / 2)) + 20f )) 
			{
				//Debug.Log ("Left Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.right);
			}

			else if (myRectTransform.anchoredPosition.y >= (((canvasHeight/2) - (noteSizeHeight / 2)) + 16f )) 
			{
				//Debug.Log ("Top Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.down);
			}

			else if (myRectTransform.anchoredPosition.y <= -(((canvasHeight / 2) - (noteSizeHeight / 2)) - 7f )) 
			{
				//Debug.Log ("Down Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.up);
			}

			//randomDir.Normalize ();
			yield return null;
		}
		yield break;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject.name == "Note2")
			note2ObjPull.ReturnObject (getInfoGameObject);
		else
			return;
	}

	public void CreateNote()
	{
		note3ObjectPool = GameObject.Find ("Note3Pool").GetComponent<SimpleObjectPool>();

		note2ObjPull.ReturnObject (gameObject);
		//repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate/2));

		note3_Left = note3ObjectPool.GetObject ();
		note3_Left.name = "Note3";
		note3_Left.transform.SetParent (parentTransform, false);
		note3_Left.transform.localScale = Vector3.one;
		note3_Left.transform.position = new Vector3 (gameObject.transform.position.x - 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		//예외처리
		//left
		if(note3_Left.transform.position.x <= -329f)
			note3_Left.transform.position = parentTransform.transform.position;

		//bottom
		else if(note3_Left.transform.position.y <= -518f)
			note3_Left.transform.position = parentTransform.transform.position;
		
		//right
		else if(note3_Left.transform.position.x <= 324f)
			note3_Left.transform.position = parentTransform.transform.position;
		//top
		else if(note3_Left.transform.position.y >= 545f)
			note3_Left.transform.position = parentTransform.transform.position;


		note3Obj = note3_Left.GetComponent<Note3Object> ();
		note3Obj.note3ObjPull = note3ObjectPool;
		note3Obj.parentTransform = parentTransform;
		note3Obj.repairObj = repairObj;
	//	repairObj.AddBossWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));

		note3_Right = note3ObjectPool.GetObject ();
		note3_Right.name = "Note3";
		note3_Right.transform.SetParent (parentTransform, false);
		note3_Right.transform.localScale = Vector3.one;
		note3_Right.transform.position = new Vector3 (gameObject.transform.position.x + 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		//left
		if(note3_Right.transform.position.x <= -329f)
			note3_Right.transform.position = parentTransform.transform.position;

		//bottom
		else if(note3_Right.transform.position.y <= -518f)
			note3_Right.transform.position = parentTransform.transform.position;

		//right
		else if(note3_Right.transform.position.x <= 324f)
			note3_Right.transform.position = parentTransform.transform.position;
		//top
		else if(note3_Right.transform.position.y >= 545f)
			note3_Right.transform.position = parentTransform.transform.position;
			

		note3Obj = note3_Right.GetComponent<Note3Object> ();
		note3Obj.note3ObjPull = note3ObjectPool;
		note3Obj.parentTransform = parentTransform;
		note3Obj.repairObj = repairObj;
	
	}

	public void EraseObj()
	{
		//StopCoroutine (NoteObjMove ());
		note2ObjPull.ReturnObject (gameObject);
	}
}
