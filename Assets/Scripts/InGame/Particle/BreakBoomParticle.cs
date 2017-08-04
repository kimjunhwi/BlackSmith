using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoomParticle : MonoBehaviour {

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Play()
    {
        anim.SetTrigger("bIsBoom");

        StartCoroutine(StartDisappearAfter(2f));
    }

    IEnumerator StartDisappearAfter(float time)
    {
        yield return new WaitForSeconds(time);

        BreakBoomPool.Instance.ReturnObject(gameObject);
    }

    public void ResetAnimation()
    {
        anim.SetTrigger("bIsBoom");
    }
}
