using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossSmallFireObject : MonoBehaviour, IPointerDownHandler
{
	GameObject getInfoGameObject;				//터치하는 오브젝트 정보
	public int nTouchCount;
	public SimpleObjectPool smallFireObjPull;	//해당 오브젝트 풀
	public RectTransform parentTransform;	

	public void StartCheckSmallFire()
	{
		StartCoroutine (CheckSmallFire ());
	}

	public IEnumerator CheckSmallFire()
	{
		while ( true )
		{
			if(nTouchCount <= 0)
				smallFireObjPull.ReturnObject (gameObject);
			yield return null;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;

		if (getInfoGameObject.gameObject.name == "SmallFireTouch") 
		{
			
			if (nTouchCount > 0)
				nTouchCount--;
		}
	}
}
