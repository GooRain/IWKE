using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalManipulation
{

	/// <summary>
	/// Lerping Transform's LOCAL position from startPos to endPos
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="startPos"></param>
	/// <param name="endPos"></param>
	/// <param name="during"></param>
	/// <returns></returns>
	public static IEnumerator LocalMove(Transform transform, Vector3 startPos, Vector3 endPos, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.localPosition = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / during);
			yield return null;
		}
		transform.localPosition = endPos;
		transform.SendMessage("OnCoroutineEnd");
	}

	/// <summary>
	/// Lerping Transform's LOCAL rotation from startRot to endRot
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="startRot"></param>
	/// <param name="endRot"></param>
	/// <param name="during"></param>
	/// <returns></returns>
	public static IEnumerator LocalRotate(Transform transform, Quaternion startRot, Quaternion endRot, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.localRotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / during);
			yield return null;
		}
		transform.localRotation = endRot;
	}

	/// <summary>
	/// Lerping Transform's GLOBAL position from startPos to endPos
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="startPos"></param>
	/// <param name="endPos"></param>
	/// <param name="during"></param>
	/// <returns></returns>
	public static IEnumerator Move(Transform transform, Vector3 startPos, Vector3 endPos, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / during);
			yield return null;
		}
		transform.position = endPos;
		transform.SendMessage("OnCoroutineEnd");
	}

	/// <summary>
	/// Lerping Transform's GLOBAL rotation from startRot to endRot
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="startRot"></param>
	/// <param name="endRot"></param>
	/// <param name="during"></param>
	/// <returns></returns>
	public static IEnumerator Rotate(Transform transform, Quaternion startRot, Quaternion endRot, float during)
	{
		float startTime = Time.time;
		while(Time.time < startTime + during)
		{
			transform.rotation = Quaternion.Lerp(startRot, endRot, (Time.time - startTime) / during);
			yield return null;
		}
		transform.rotation = endRot;
	}
}
