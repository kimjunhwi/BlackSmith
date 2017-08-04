using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissText : MonoBehaviour 
{
	public float leftSecond;		//사라지기 까지의 시간
	RectTransform textRectTransform;	//텍스트 Obj의 RectTransform
	public SimpleObjectPool textObjPool;
	public RectTransform parentTransform;	

	private float rectTransformX = 0f;	//TextRect를 받는 X값
	private float rectTransformY = 0f;	//TextRect를 받는 Y값

	// Use this for initialization
	void Start () {
		
		textRectTransform = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		leftSecond -= Time.deltaTime;

		if (leftSecond <= 0)
			textObjPool.ReturnObject (gameObject);
		else
		{
			rectTransformX = textRectTransform.anchoredPosition.x;
			rectTransformY = textRectTransform.anchoredPosition.y;

			rectTransformY += 0.5f;
			textRectTransform.anchoredPosition = new Vector2(rectTransformX, rectTransformY);
		}
	}
}
