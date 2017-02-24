using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
	public int sceneIndex;

	public void Load()
	{
		Invoke("NextScene", 1);
	}

	void NextScene()
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
