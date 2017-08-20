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
	public BossMusic bossMusic;
	public RepairObject repairObj;
	public GameObject bossWeapon_Obj;
	//private;
	public float fTime;

	private float fRandomX;
	private float fRandomY;
	private  float fMoveSpeed = 10.0f;
	private  float fBossSpeed = 1.0f;
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


	public void StartNoteObjMove()
	{
		myRectTransform = GetComponent<RectTransform> ();
		fRandomX = Random.Range ( -0.5f, 0.5f);
		fRandomY = Random.Range ( -0.5f, 0.5f);;

		randomDir = new Vector3 (fRandomX, fRandomY, 0);
		randomDir = randomDir.normalized;

		note2ObjectPool = GameObject.Find ("Note2Pool").GetComponent<SimpleObjectPool>();
		bossMusic = GameObject.Find ("BossMusic").GetComponent<BossMusic> ();
		note2ObjectPool.PreloadPool ();
		StartCoroutine (NoteObjMove ());
	}

	IEnumerator NoteObjMove()
	{
		while (true) 
		{

			yield return null;


			//4면 충돌 확인
			if (myRectTransform.anchoredPosition.x >= (((canvasWidth / 2) - (noteSizeWidth / 2)) + 42f))
			{
				//Debug.Log ("Right Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.left);
				randomDir = new Vector3 (-1.0f, randomDir.y, 0f);
			}

			else if (myRectTransform.anchoredPosition.x <= -(((canvasWidth / 2) - (noteSizeWidth / 2)) + 29f))
			{
				//Debug.Log ("Left Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.right);
				randomDir = new Vector3 (1.0f, randomDir.y, 0f);
			}

			else if (myRectTransform.anchoredPosition.y >= (((canvasHeight/2) - (noteSizeHeight / 2)) + 10f))
			{
				//Debug.Log ("Top Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.down);
				randomDir = new Vector3 (randomDir.x, -1.0f, 0f);
			}

			else if (myRectTransform.anchoredPosition.y <= -(((canvasHeight / 2) - (noteSizeHeight / 2)) - 15f)) 
			{
				//Debug.Log ("Down Collision");
				randomDir = Vector3.Reflect (randomDir, Vector3.up);
				randomDir = new Vector3 (randomDir.x, 1.0f, 0f);
			}
			randomDir = randomDir.normalized;
		

			#if UNITY_EDITOR
			transform.Translate (fMoveSpeed * randomDir);

			#elif UNITY_ANDROID
			transform.Translate (fMoveSpeed * randomDir * 2f);

			#endif

		}
		yield break;
	}
		
	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;
	
		if (getInfoGameObject.gameObject.name == "Note") 
			CreateNote ();
		 
		else
			return;

	}

	public void CreateNote()
	{

		noteObjPull.ReturnObject (gameObject);
		bossMusic.DecreaseRefectionTime (0.5f);
		note2ObjectPool = GameObject.Find ("Note2Pool").GetComponent<SimpleObjectPool>();


		note2_Left = note2ObjectPool.GetObject ();
		note2_Left.name = "Note2";
		note2_Left.transform.SetParent (parentTransform,false);
		note2_Left.transform.localScale = Vector3.one;
		note2_Left.transform.position = new Vector3 (gameObject.transform.position.x - 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);
		

		//예외 처리
		//left
		if(note2_Left.transform.position.x <= -310.0f)
			note2_Left.transform.position = parentTransform.transform.position;
		//bottom
		else if(note2_Left.transform.position.y <= -500.0f)
			note2_Left.transform.position = parentTransform.transform.position;
		//right
		else if(note2_Left.transform.position.x >=308.0f)
			note2_Left.transform.position = parentTransform.transform.position;
		else if(note2_Left.transform.position.y >= 534.0f)
			note2_Left.transform.position = parentTransform.transform.position;


		note2Obj = note2_Left.GetComponent<Note2Object> ();
		note2Obj.note2ObjPull = note2ObjectPool;
		note2Obj.parentTransform = parentTransform;
		note2Obj.repairObj = repairObj;
		note2Obj.StartNoteObjMove ();
		note2Obj.bossMusic = bossMusic;



		note2_Right = note2ObjectPool.GetObject ();
		note2_Right.name = "Note2";
		note2_Right.transform.SetParent (parentTransform, false);
		note2_Right.transform.localScale = Vector3.one;
		note2_Right.transform.position = new Vector3 (gameObject.transform.position.x + 40f, gameObject.transform.position.y,
			gameObject.transform.position.z);

		//예외 처리
		//left
		if(note2_Right.transform.position.x <= -310.0f)
			note2_Right.transform.position = parentTransform.transform.position;
		//bottom
		else if(note2_Right.transform.position.y <= -490.0f)
			note2_Right.transform.position = parentTransform.transform.position;
		//right
		else if(note2_Right.transform.position.x >=308.0f)
			note2_Right.transform.position = parentTransform.transform.position;
		else if(note2_Right.transform.position.y >= 524.0f)
			note2_Right.transform.position =parentTransform.transform.position;
		


		note2Obj = note2_Right.GetComponent<Note2Object> ();
		note2Obj.note2ObjPull = note2ObjectPool;
		note2Obj.parentTransform = parentTransform;
		note2Obj.repairObj = repairObj;
		note2Obj.StartNoteObjMove ();
		note2Obj.bossMusic = bossMusic;

		bossMusic.IncreaseRefectionTime (0.5f);
		bossMusic.nNoteCount--;
	}

	public void EraseObj()
	{
		//StopCoroutine (NoteObjMove ());
		noteObjPull.ReturnObject (gameObject);
	}
}
