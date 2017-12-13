using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class LoadSceneScript : MonoBehaviour {
	public Slider slider;
	public GameObject loadingScreen;

	AsyncOperation async;
	private int level;
	private int sceneIdx;

	// Use this for initialization
	void Start () {
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
		level = sceneIdx + 1;
		StartCoroutine (LoadingScreen (level));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadingScreen(int scene) {
		loadingScreen.SetActive (true);
		async = SceneManager.LoadSceneAsync (0);
		async.allowSceneActivation = false;

		while (async.isDone == false) {
			slider.value = async.progress;
			if (async.progress == 0.9f) {
				slider.value = 1f;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
