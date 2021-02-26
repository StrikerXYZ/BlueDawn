using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceSphere : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, 1.0f);
	}
}
