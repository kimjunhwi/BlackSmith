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
	private float fMoveSpeed = 200.0f;

	private Vector3 randomDir;

	private float canvasWidth = 720f;
	private float canvasHeight = 1130f;
	//114

	private float skullSizeWidth = 60f;
	private float skullSizeHeight = 80f;


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

		if (getInfoGameObject.gameObject == null)
			return;

		if (getInfoGameObject.gameObject.name == "Note3") 
		{
			//float fCurComplete = repairObj.GetCurCompletion ();
			//float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

			//repairObj.SetCurCompletion (fMaxComplete * 0.3f);
			note3ObjPull.ReturnObject (gameObject);

		}

	}
}
