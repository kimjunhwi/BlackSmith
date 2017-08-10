using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScroll : MonoBehaviour {

	private const int nMaxOption = 7;

	public Transform contentPanel;

	List<CGameMainWeaponOption> LIST_OPTION;

	public SimpleObjectPool optionPanelPool;

	public void Start()
	{
		int nActiveOptionCount = 0;

		LIST_OPTION = GameManager.Instance.cMainWeaponOption;
		

		if (LIST_OPTION == null) {

			nActiveOptionCount = 0;

			LIST_OPTION = new List<CGameMainWeaponOption> ();
		}
		else
			nActiveOptionCount = LIST_OPTION.Count;

		RefreshDisplay ();
	}

	public void RefreshDisplay()
	{
		RemoveButtons();
		AddButtons();
	}

	private void RemoveButtons()
	{
		while (contentPanel.childCount > 0)
		{
			GameObject toRemove = transform.GetChild(0).gameObject;
			optionPanelPool.ReturnObject(toRemove);
		}
	}

	//옵션 정렬 방식이다
	private void AddButtons()
	{
		//등급이 높은 것을 정렬
		LIST_OPTION.Sort(delegate(CGameMainWeaponOption A, CGameMainWeaponOption B)
			{
				if (A.nIndex < B.nIndex) return 1;
				else if (A.nIndex > B.nIndex) return -1;
				else return 0;
			});

		for (int i = 0; i < nMaxOption; i++)
		{
			if (i < LIST_OPTION.Count)
			{
				CGameMainWeaponOption item = LIST_OPTION[i];

				GameObject newButton = optionPanelPool.GetObject();
				newButton.transform.SetParent(contentPanel, false);
				newButton.transform.localScale = Vector3.one;

				OptionItem sampleButton = newButton.GetComponent<OptionItem>();
				sampleButton.Setup(item);
			}
			else
			{
				GameObject newButton = optionPanelPool.GetObject();
				newButton.transform.SetParent(contentPanel, false);
				newButton.transform.localScale = Vector3.one;

				OptionItem sampleButton = newButton.GetComponent<OptionItem>();
				sampleButton.SetInit ();
			}
		}
	}

	public void AddItem(int _nIndex,string _strExplain,int _nValue, bool _bIsLock = false)
	{
		CGameMainWeaponOption plusItem = new CGameMainWeaponOption ();


	}
}