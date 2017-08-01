using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class ArbaitCharacter : MonoBehaviour {

    public int nIndex;
    private int nEnhanceCost;
    private int nGetBatchIndex;

    protected bool m_bIsBatch = false;

	private Button m_BuyButtonEvent;
	private Button m_EnhanceButtonEvent;

    protected Sprite m_NoneImage;
    protected Sprite m_CharacterImage;

	public Sprite m_NoneActiveSprite;
	public Sprite m_ActiveSprite;
	public Sprite m_GoldSprite;
	public Sprite m_HonorSprite;
    
    protected Image m_CharacterImageObject;
    
	protected ArbaitData m_CharacterData;
	protected CGameArbaitGrade[] ArbaitEnhanceData; 


    private Toggle m_SettingToggle;

    private GameObject m_BuyButton;
    private GameObject m_SettingPanel;
    private SpawnManager spawnManager;

	public GameObject TextPanel;

	public Text LevelText;
	public Text NameText;
	public Text SkillExplainText;
	public Text RepairPowerText;
	public Text AttackSpeedText;
	public Text CriticalText;
	public Text AccuracyText;
	public Text GoldText;

    public virtual void Upgrade(){}

	// Use this for initialization
	protected void Awake () {
		
        m_BuyButton = transform.FindChild("BuyButton").gameObject;

		m_BuyButtonEvent = m_BuyButton.GetComponent<Button> ();

		m_BuyButtonEvent.onClick.AddListener (BuyCharacter);
        
		TextPanel = transform.FindChild ("TextPanel").gameObject;

		LevelText = TextPanel.transform.FindChild ("LevelText").GetComponent<Text>();
		NameText = TextPanel.transform.FindChild ("NameText").GetComponent<Text>();
		SkillExplainText = TextPanel.transform.FindChild ("SkillExplain").GetComponent<Text>();
		RepairPowerText = TextPanel.transform.FindChild ("RepairPower").GetComponent<Text>();
		AttackSpeedText = TextPanel.transform.FindChild ("AttackSpeed").GetComponent<Text>();
		CriticalText = TextPanel.transform.FindChild ("Critical").GetComponent<Text>();
		AccuracyText = TextPanel.transform.FindChild ("Accuracy").GetComponent<Text>();

		m_SettingPanel = transform.FindChild("SettingPanel").gameObject;

		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
		m_CharacterImageObject = transform.FindChild("CharacterImage").GetComponent<Image>();
        
		m_SettingToggle = m_SettingPanel.transform.FindChild("SettingToggle").GetComponent<Toggle>();

		m_EnhanceButtonEvent = m_SettingPanel.transform.FindChild ("EnhanceButton").GetComponent<Button> ();

		GoldText = m_EnhanceButtonEvent.transform.FindChild ("CostText").GetComponent<Text>();

		m_SettingToggle.onValueChanged.AddListener (OnBatchToggle);

		m_EnhanceButtonEvent.onClick.AddListener (EnhanceEvent);

	}

	public void SetUp(int _nIndex,Sprite _sprite)
	{
		nIndex = _nIndex;

		m_CharacterImageObject.sprite = _sprite;

		m_CharacterData = GameManager.Instance.GetArbaitData(nIndex);

        ArbaitEnhanceData = GameManager.Instance.GetArbaitGradeEnhanceData(m_CharacterData.grade);

        CheckBuyCharacter();
	}

    public void BuyCharacter()
    {
        //스코어 매니저를 만들었을 경우 개선

        if (m_CharacterData.nScoutGold >= ScoreManager.ScoreInstance.GetGold())
        {
            //추후 추가
            m_CharacterData.level = 1;

        }
        CheckBuyCharacter();
    }

    public void CheckBuyCharacter()
    {
        //구매하지 않았을 경우
        if (m_CharacterData.level == 0)
        {
            m_BuyButton.SetActive(true);
            m_SettingPanel.SetActive(false);
            TextPanel.SetActive(false);

            gameObject.GetComponent<Image>().sprite = m_NoneActiveSprite;
            //m_CharacterImageObject.sprite = m_CharacterNoneSprite;
        }

        //구매했을경우
        else
        {
            m_BuyButton.SetActive(false);
            m_SettingPanel.SetActive(true);
            TextPanel.SetActive(true);

            gameObject.GetComponent<Image>().sprite = m_ActiveSprite;

            ChangeArbaitText();

            if (m_CharacterData.batch != -1)
            {
                m_SettingToggle.isOn = true;

                spawnManager.AddArbait(m_CharacterData.index, m_CharacterData.batch, gameObject, m_CharacterData);

                m_bIsBatch = true;
            }
            else
                m_SettingToggle.isOn = false;
        }
    }

    //아르바이트 배치 및 해제 부분
    public void OnBatchToggle(bool _bIsToggle)
    {
        if (m_CharacterData == null)
            return;

        bool bIsToggle = _bIsToggle;

        Debug.Log(m_SettingToggle.isOn);

        //만약 배치를 해야하고 배치상태가 아니라면
        if (bIsToggle && m_bIsBatch == false)
        {
            //배치 될 수 있는지를 확인
            nGetBatchIndex = spawnManager.AddArbaitCheck();

            //만약 배치 할 수 있다면 현재 인덱스, 배치 인덱스, 현재 페널, 캐릭터 데이터를 보낸후 배치
            if (nGetBatchIndex != (int)E_CHECK.E_FAIL)
            {
                spawnManager.AddArbait(m_CharacterData.index, nGetBatchIndex, gameObject, m_CharacterData);
                m_bIsBatch = true;
            }
            else
            {
                m_SettingToggle.isOn = false;
            }
        }
        //해제를 원하고 배치상태라면
        else if ((bIsToggle == false) && m_bIsBatch)
        {
            //배치된 아르바이트를 지움
            spawnManager.DeleteArbait(gameObject);

            m_CharacterData.batch = -1;

            m_bIsBatch = false;
        }

        Debug.Log(m_SettingToggle.isOn);
    }

    public void EnhanceEvent()
    {
        //현재 골드가 비용보다 높을 경우 
        if (ScoreManager.ScoreInstance.GetGold() >= ArbaitEnhanceData[m_CharacterData.level - 1].nGoldCost)
        {
            ScoreManager.ScoreInstance.GoldPlus(-ArbaitEnhanceData[m_CharacterData.level - 1].nGoldCost);

            m_CharacterData.fRepairPower += GameManager.Instance.GetArbaitData(nIndex).fRepairPower * (float)(0.01 * ArbaitEnhanceData[m_CharacterData.level - 1].nPercentPlusRepair);
            m_CharacterData.fCritical += GameManager.Instance.GetArbaitData(nIndex).fCritical * (float)(0.01 * ArbaitEnhanceData[m_CharacterData.level - 1].nPercentPlusCritical);
            m_CharacterData.fAccuracyRate += GameManager.Instance.GetArbaitData(nIndex).fAccuracyRate * (float)(0.01 * ArbaitEnhanceData[m_CharacterData.level - 1].nPercentPlusAccuracy);

            if (ArbaitEnhanceData[m_CharacterData.level - 1].nPercentPlusSkill != 0)
            {

            }

            m_CharacterData.level++;

            ChangeArbaitText();
        }
    }

    void ChangeArbaitText()
    {
        LevelText.text = m_CharacterData.level.ToString();
        NameText.text = m_CharacterData.name;
        SkillExplainText.text = m_CharacterData.strExplains;
        RepairPowerText.text = m_CharacterData.fRepairPower.ToString("F1");
        AttackSpeedText.text = m_CharacterData.fAttackSpeed.ToString("F1");
        CriticalText.text = m_CharacterData.fCritical.ToString("F1");
        AccuracyText.text = m_CharacterData.fAccuracyRate.ToString("F1");

        if (ArbaitEnhanceData[m_CharacterData.level - 1].nHonorCost == 0)
        {
            GoldText.text = ArbaitEnhanceData[m_CharacterData.level - 1].nGoldCost.ToString();
            m_EnhanceButtonEvent.GetComponent<Image>().sprite = m_GoldSprite;
        }
        else
        {
            GoldText.text = ArbaitEnhanceData[m_CharacterData.level - 1].nHonorCost.ToString();
            m_EnhanceButtonEvent.GetComponent<Image>().sprite = m_HonorSprite;
        }
    }
}
