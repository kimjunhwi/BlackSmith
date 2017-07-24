using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryButton : MonoBehaviour {



    private Button button;


	public Image WeaponImage;
	public GameObject WeaponObject;

	private InventoryShowPanel inventoryPanel;

    private CGameEquiment equimentData;

    void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(HandleClick);
    }

	public void Setup(CGameEquiment currentEquiment,InventoryShowPanel _inventoryPanel)
    {
        equimentData = currentEquiment;
        
		inventoryPanel = _inventoryPanel;

		WeaponImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(equimentData.strResource);

		WeaponObject.SetActive (true);
        //이미지 등록
    }

    //버튼 클릭시 아이템을 보여주기 위함
    public void HandleClick()
    {
		if (equimentData == null)
			return;
		inventoryPanel.SetUp (equimentData);
    }


}
