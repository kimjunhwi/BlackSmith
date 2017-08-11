using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour 
{

	public RepairObject repairObject;

	private Vector3 mouseTouchPosition;

	List<RaycastResult> results = new List<RaycastResult> ();

	public GraphicRaycaster graphicRaycaster;

	PointerEventData pointData;

	void Start()
	{
		pointData = new PointerEventData (null);
	}

	void Update()
	{
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			results.Clear();

			pointData.position = Input.mousePosition;

			graphicRaycaster.Raycast(pointData, results); 

			if (results.Count !=0) 
			{ 
				GameObject obj = results[0].gameObject; 

				if (obj.CompareTag("WeaponButton")) // 히트 된 오브젝트의 태그와 맞으면 실행 
				{ 
					repairObject.TouchWeapon (pointData.position);
					return;
				} 
			}
		}
		

		#elif UNITY_ANDROID

		int nbTouches = Input.touchCount;

		if(nbTouches > 0)
		{
			for (int nIndex = 0; nIndex < nbTouches; nIndex++) {

				results.Clear();

				Touch touch = Input.GetTouch(nIndex);

				if (touch.phase == TouchPhase.Began) {

					pointData.position = touch.position;

					graphicRaycaster.Raycast(pointData, results); 

					if (results.Count !=0) 
					{ 
						GameObject obj = results[0].gameObject; 

						if (obj.CompareTag("WeaponButton")) // 히트 된 오브젝트의 태그와 맞으면 실행 
						{ 
							repairObject.TouchWeapon (pointData.position);
							break;
						} 
					} 
				}
			}
		}
		#endif
	}
}
