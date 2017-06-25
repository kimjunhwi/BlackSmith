using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArrival : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        //도착된게 몬스터일경우 비활성화로 한다.
        if (other.CompareTag("Monster"))
            other.gameObject.SetActive(false);
    }
}
