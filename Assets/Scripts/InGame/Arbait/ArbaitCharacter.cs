using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class ArbaitCharacter : MonoBehaviour {

    public int nIndex;
    private int nGetBatchIndex;

    protected bool m_bIsBatch = false;

    protected Sprite m_NoneImage;
    protected Sprite m_CharacterImage;
    
    protected Image m_CharacterImageObject;
    protected ArbaitData m_CharacterData;


    private Toggle m_SettingToggle;
    private GameObject m_BuyButton;
    private GameObject m_SettingPanel;
    private SpawnManager spawnManager;

    public Animator animator;

    public virtual void Upgrade(){}

	// Use this for initialization
	protected void Start () {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        m_CharacterData = GameManager.Instance.GetArbaitData(nIndex);

        m_BuyButton = transform.FindChild("BuyButton").gameObject;
        m_SettingPanel = transform.FindChild("SettingPanel").gameObject;
        m_CharacterImageObject = transform.FindChild("CharacterImage").GetComponent<Image>();
        m_SettingToggle = m_SettingPanel.transform.FindChild("SettingToggle").GetComponent<Toggle>();
       
        string strPath = string.Format("ArbaitUI/{0}", m_CharacterData.name);

        m_CharacterImage = ObjectCashing.Instance.LoadSpriteFromCache(strPath);
        
        CheckBuyCharacter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuyCharacter()
    {
        //스코어 매니저를 만들었을 경우 개선

        if(m_CharacterData.costGold > 5)
        {
            //추후 추가
            m_CharacterData.level = 1;
            
        }
        CheckBuyCharacter();
    }

    private void CheckBuyCharacter()
    {
        //구매하지 않았을 경우
        if (m_CharacterData.level == 0)
        {
            m_BuyButton.SetActive(true);
            m_SettingPanel.SetActive(false);
            //m_CharacterImageObject.sprite = m_CharacterNoneSprite;
        }

        //구매했을경우
        else
        {
            m_CharacterImageObject.sprite = m_CharacterImage;

            m_BuyButton.SetActive(false);
            m_SettingPanel.SetActive(true);
            

            if (m_CharacterData.batch != -1)
                m_SettingToggle.isOn = true;
            else
                m_SettingToggle.isOn = false;
        }
    }

    public void OnBatchToggle()
    {
        bool bIsToggle = m_SettingToggle.isOn;
        
        if(bIsToggle && m_bIsBatch == false)
        {
            nGetBatchIndex = spawnManager.AddArbaitCheck();

            if (nGetBatchIndex != (int)E_CHECK.E_FAIL)
            {
                spawnManager.AddArbait(nGetBatchIndex, gameObject, m_CharacterData, animator);
                m_bIsBatch = true;
            }
        }
        else if((bIsToggle == false) && m_bIsBatch)
        {
            spawnManager.DeleteArbait(gameObject);

            m_bIsBatch = false;
        }

        Debug.Log(m_SettingToggle.isOn);
    }
}
