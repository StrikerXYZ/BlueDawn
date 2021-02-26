using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVolume : MonoBehaviour
{
	//[SerializeField]
	private int amount = 0;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private Transform spawnParent;

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
        for(int i = 0; i < amount; ++i)
        {
			Spawn();
		}
    }

    public GameObject Spawn()
	{
		GameObject gameObject = Instantiate(prefab, spawnParent);
		gameObject.transform.localPosition = SpawnPoint;
		return gameObject;
	}
	public GameObject Spawn(AvoidanceSphere[] avoidance)
	{
		int tryLimit = 100;
		bool hasCollision;
		Vector3 spawnPoint;
		do
		{
			spawnPoint = SpawnPoint;
			hasCollision = false;
			foreach (var avoid in avoidance)
			{
				var posA = spawnPoint;
				var posB = transform.position;
				var direction = posB - posA;
				float spawnDistSquare = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
				float avoidDistSquare = avoid.transform.localScale.z;
				avoidDistSquare *= avoidDistSquare;
				if (avoidDistSquare > spawnDistSquare)
				{
					hasCollision = true;
					break;
				}
			}
		} while (hasCollision && tryLimit-- > 0);

		GameObject gameObject = Instantiate(prefab, spawnParent);
		gameObject.transform.localPosition = spawnPoint;
		return gameObject;
	}


	public GameObject[] Spawn(AvoidanceSphere[] avoidance, int amount, float radius)
	{
		int tryLimit = 100;
		bool hasCollision;
		Vector3 spawnPoint;
		do
		{
			spawnPoint = SpawnPoint;
			hasCollision = false;
			foreach (var avoid in avoidance)
			{
				var posA = spawnPoint;
				var posB = transform.position;
				var direction = posB - posA;
				float spawnDistSquare = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
				float avoidDistSquare = avoid.transform.localScale.z;
				avoidDistSquare *= avoidDistSquare;
				if (avoidDistSquare > spawnDistSquare)
				{
					hasCollision = true;
					break;
				}
			}
		} while (hasCollision && tryLimit-- > 0);

		GameObject[] list = new GameObject[amount];
		for (int i = 0; i < amount; ++i)
		{
			list[i] = Instantiate(prefab, spawnParent);
			list[i].transform.position = spawnPoint + SpawnZone(radius);
		}

		GameObject spawnObject = new GameObject();
		spawnObject.transform.position = spawnPoint;
		spawnObject.name = "Spawn";

		return list;
	}
	private Vector3 SpawnZone(float radius)
	{
		return new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;chrome://vivaldi-webui/startpage?section=Speed-dials&activeSpeedDialIndex=0&background-color=#2e2e2e
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}
