using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Animator m_Animator;

    public Transform particlePosition;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }


    public void UserNormalRepair()
    {
        m_Animator.SetTrigger("bIsNormalRepair");

    }

    public void UserCriticalRepair()
    {
        m_Animator.SetTrigger("bIsCriticalRepair");

    }

    public void CreateNormalEffect()
    {
        Debug.Log("NormalHit");

        GameObject obj = NormalRepairPool.Instance.GetObject();

        obj.transform.position = particlePosition.position;

        obj.GetComponent<NormalRepairParticle>().Play();

        m_Animator.SetTrigger("bIsNormalRepair");
    }

    public void CreateCriticalEffect()
    {
        Debug.Log("CriticalHit");

        GameObject obj = CriticalRepairPool.Instance.GetObject();

        obj.transform.position = particlePosition.position;

        obj.GetComponent<CriticalRepairParticle>().Play();

        m_Animator.SetTrigger("bIsCriticalRepair");
    }


    public void UserBigSuccessed()
    {
        

    }

}
