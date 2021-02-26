using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private int amount = 0;

	[SerializeField]
	private GameObject[] prefab;

	[SerializeField]
	private Transform fishContainer;

	private Vector3 SpawnPoint
	{
		get
		{
			Vector3 p;
			p.x = Random.Range(-0.5f, 0.5f);
			p.y = Random.Range(-0.5f, 0.5f);
			p.z = Random.Range(-0.5f, 0.5f);
			return transform.TransformPoint(p);
		}
	}

	private void Start()
	{
		for (int i = 0; i < amount; ++i)
		{
			Spawn();
		}
	}

	public GameObject Spawn()
	{
		GameObject gameObject = Instantiate(prefab[Random.Range(0, prefab.Length)], fishContainer);
		gameObject.transform.localPosition = SpawnPoint;
		return gameObject;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}