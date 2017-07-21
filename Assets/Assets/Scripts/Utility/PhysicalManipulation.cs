using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalManipulation
{

	public static IEnumerator Move(Transform transform, Vector3 startPos, Vector3 endPos, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.localPosition = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / during);
			yield return null;
		}
		transform.localPosition = endPos;
	}

	public static IEnumerator Rotate(Transform transform, Quaternion startRot, Quaternion endRot, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.localRotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / during);
			yield return null;
		}
		transform.localRotation = endRot;
	}
}
