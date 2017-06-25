using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour {

    float fSpeed = 1.2f;

    Vector3 vecLeft = new Vector3(-1.0f, 0.0f, 0.0f);

	// Update is called once per frame
	void Update () {
        transform.Translate(vecLeft * fSpeed * Time.deltaTime); // 좌측으로 이동시킨다.
	}
}
