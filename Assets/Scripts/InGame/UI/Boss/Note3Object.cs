using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Note3Object : MonoBehaviour  ,IPointerDownHandler 
{
	GameObject getInfoGameObject;

	public SimpleObjectPool note3ObjPull;		//해당 오브젝트 풀
	public RectTransform parentTransform;		
	private RectTransform myRectTransform;		

	public RepairObject repairObj;

	//private;
	public float fTime;

	private float fRandomX;
	private float fRandomY;
	private float fMoveSpeed = 10.0f;
	private float fBossSpeed = 1.0f;
	private float fDecreaseWeaponSpeedRate = 0.1f;

	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float noteSizeWidth = 64f;
	private float noteSizeHeight = 64f;


	void Start()
	{
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-1.0f, 1.0f);
		fRandomY = Random.Range (-1.0f, 1.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
		randomDir.Normalize ();
	}



	void Update()
	{
		transform.Translate (randomDir * fMoveSpeed );

		//4면 충돌 확인
		if (myRectTransform.anchoredPosition.x >= (((canvasWidth / 2) - (noteSizeWidth / 2)) + 15f ))
		{
			//Debug.Log ("Right Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.left);
			randomDir.Normalize ();
			//fMoveSpeed = Random.Range (5f, 10f);
		}

		else if (myRectTransform.anchoredPosition.x <= -(((canvasWidth / 2) - (noteSizeWidth / 2)) + 18f )) 
		{
			//Debug.Log ("Left Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.right);
			randomDir.Normalize ();
			//fMoveSpeed = Random.Range (5f, 10f);
		}

		else if (myRectTransform.anchoredPosition.y >= (((canvasHeight/2) - (noteSizeHeight / 2)) + 5f )) 
		{
			//Debug.Log ("Top Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.down);
			randomDir.Normalize ();
			//fMoveSpeed = Random.Range (5f, 10f);
		}

		else if (myRectTransform.anchoredPosition.y <= -(((canvasHeight / 2) - (noteSizeHeight / 2)) -16f )) {
			//Debug.Log ("Down Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.up);
			randomDir.Normalize ();
			//fMoveSpeed = Random.Range (5f, 10f);
		}

	}
	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;

		if (getInfoGameObject.gameObject.name == "Note3") 
		{
			
			//repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));
			note3ObjPull.ReturnObject (gameObject);
		}

	}

	public void EraseObj()
	{
		//repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));
		note3ObjPull.ReturnObject (gameObject);
	}
}
