using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private Vector3 cameraPos;
	private Vector2 shakeCameraPos;
	private float fShake;

	public float amplitude = 0.1f;
	public float decreaseFactor = 1.0f;
	public bool isShaking = false;



	// Use this for initialization
	void Start () {
		//카메라 포지션을 가져 옴
		cameraPos = transform.localPosition;
	}

	//흔드는 정도와 시간을 가져 
	public void Shake(float _amplitude, float _duraction)
	{
		amplitude = _amplitude;
		isShaking = true;

		//동작 중 이라면 취소
		CancelInvoke();

		Invoke("StopShaking", _duraction);
	}

	public void StopShaking()
	{
		isShaking = false;
	}

	// Update is called once per frame
	void Update () {
		if (isShaking)
		{
			transform.localPosition = cameraPos + Random.insideUnitSphere * amplitude;
		}
	}
}
