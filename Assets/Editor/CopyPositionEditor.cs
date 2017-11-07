using UnityEngine;
using UnityEditor;

// Adds MenuItems to Unity Editor for copy/paste selected transform's position
public class CopyPositionEditor : ScriptableObject
{

	static Vector3 cachedPosition;
	static Vector3 cachedRotation;

	[MenuItem("CONTEXT/Transform/Copy Transform Position and Rotation %#c")]
	static void CopyPosition()
	{
		if(Selection.activeTransform != null)
		{
			cachedPosition = Selection.activeTransform.position;
			cachedRotation = Selection.activeTransform.rotation.eulerAngles;
			EditorGUIUtility.systemCopyBuffer = Selection.activeTransform.name + ": " +
												cachedPosition.x + ", " +
												cachedPosition.y + ", " +
												cachedPosition.z + "; " +
												cachedRotation.x + ", " +
												cachedRotation.y + ", " +
												cachedRotation.z + ";";
		}
	}

}