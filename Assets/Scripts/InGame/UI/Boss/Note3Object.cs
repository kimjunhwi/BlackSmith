﻿using System.Collections;
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
	private float fMoveSpeed = 200.0f;
	private float fBossSpeed = 5.0f;
	private float fDecreaseWeaponSpeedRate = 0.1f;

	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float noteSizeWidth = 64f;
	private float noteSizeHeight = 64f;


	Vector2 vec2;
	void Start()
	{
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-2.0f, 2.0f);
		fRandomY = Random.Range (-2.0f, 2.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
	}



	void Update()
	{
		
		vec2 = new Vector2 (myRectTransform.anchoredPosition.x, myRectTransform.anchoredPosition.y);


		//지속 시간
		//if (fTime <= 0f)
		//	note3ObjPull.ReturnObject (gameObject);

		transform.Translate ( randomDir * fMoveSpeed * Time.deltaTime);

		//4면 충돌 확인
		if (myRectTransform.anchoredPosition.x >= (((canvasWidth / 2) - (noteSizeWidth / 2)) + 8f ))
		{
			//Debug.Log ("Right Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.left);
		}

		if (myRectTransform.anchoredPosition.x <= -(((canvasWidth / 2) - (noteSizeWidth / 2)) + 18f )) 
		{
			//Debug.Log ("Left Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.right);
		}

		if (myRectTransform.anchoredPosition.y >= (((canvasHeight/2) - (noteSizeHeight / 2)) + 17f )) 
		{
			//Debug.Log ("Top Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.down);
		}

		if (myRectTransform.anchoredPosition.y <= -(((canvasHeight / 2) - (noteSizeHeight / 2)) -16f )) {
			//Debug.Log ("Down Collision");
			randomDir = Vector3.Reflect (randomDir, Vector3.up);
		}

	}
	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;

		if (getInfoGameObject.gameObject.name == "Note3") 
		{
			
			repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));
			note3ObjPull.ReturnObject (gameObject);
		}

	}

	public void EraseObj()
	{
		repairObj.MinusWeaponSpeed (fBossSpeed * (fDecreaseWeaponSpeedRate / 4));
		note3ObjPull.ReturnObject (gameObject);
	}
}
