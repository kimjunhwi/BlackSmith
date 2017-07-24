using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SkullObject : MonoBehaviour ,IPointerDownHandler 
{

	GameObject getInfoGameObject;				//터치하는 오브젝트 정보
	public SimpleObjectPool skullObjPull;		//해당 오브젝트 풀
	public RectTransform parentTransform;		
	private RectTransform myRectTransform;		

	public RepairObject repairObj;

	//private;
	public float fTime;							//남은 시간을 줄여주는 시간
		
	private float fRandomX;						//랜덤 방향 X
	private float fRandomY;						//랜덤 방향 Y
	private float fMoveSpeed = 100.0f;			//속도

	private Vector3 randomDir;					//랜덤 방향  
								
												//충돌범위
	private float canvasWidth = 720f;			//캔버스 가로
	private float canvasHeight = 520f;			//캔버스 세로

	//해골 사이즈
	private float skullSizeWidth = 80f;			
	private float skullSizeHeight = 80f;

	void Start()
	{
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range (-2.0f, 2.0f);
		fRandomY = Random.Range (-2.0f, 2.0f);

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
	}



	void Update()
	{
		fTime -= Time.deltaTime;

		//지속 시간
		if (fTime <= 0f)
			skullObjPull.ReturnObject (gameObject);
		
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

		if (myRectTransform.anchoredPosition.y >= ((canvasHeight / 2) - (skullSizeHeight / 2))) 
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

		if (getInfoGameObject.gameObject.name == "Skull") 
		{
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

			repairObj.SetCurCompletion (-fMaxComplete * 0.06f);
			skullObjPull.ReturnObject (gameObject);
		}
			
	}
}
