using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour 
{

	public float shakeDecay = 0.002f;
	public float coefShakeIntensity = 0.3f;
	private Vector3 originPosition;
	private Quaternion originRotation;
	private float shakeIntensity;

	void Update ()
	{  
		if (shakeIntensity > 0) 
		{  
			transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;  
			transform.rotation = new Quaternion (
				originRotation.x + Random.Range (-shakeIntensity, shakeIntensity) * 2f,  
				originRotation.y + Random.Range (-shakeIntensity, shakeIntensity) * 2f,  
				originRotation.z + Random.Range (-shakeIntensity, shakeIntensity) * 2f,  
				originRotation.w + Random.Range (-shakeIntensity, shakeIntensity) * 2f);  
			shakeIntensity -= shakeDecay;  
		}  
	}

	// カメラを揺らすメソッド
	public void Shake ()
	{  
		originPosition = transform.position;  
		originRotation = transform.rotation;  
		shakeIntensity = coefShakeIntensity;  
	}  
}
