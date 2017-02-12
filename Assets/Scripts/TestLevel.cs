using UnityEngine;
using System.Collections;

public class TestLevel : MonoBehaviour
{
	public Vector3 dim;
	public float percent;
	public GameObject test;
	public int size;

	void Start()
	{
		for(int x = -(int)dim.x; x <=(int)dim.x; x++)
		{
			for (int y = -(int)dim.y; y <= (int)dim.y; y++)
			{
				for (int z = -(int)dim.z; z <= (int)dim.z; z++)
				{
					if (Random.Range(0f, 100f) < percent)
						(Instantiate(test, new Vector3(x, y, z) * size, Quaternion.identity) as GameObject).transform.parent = transform;
				}
			}
		}
	}
}
