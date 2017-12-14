using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class M3_Manager : MonoBehaviour {
	public M3_Player player;
	public M3_Enemy enemy;

	public bool instructionsClosed;
	public bool gameStarted;
	public bool gameOver;

	// UI
	public GameObject winObj;
	public GameObject lossObj;
	public GameObject lossObj2;
	public GameObject instructions;
	public Button closeButton;

	public int sceneIdx;

	// Use this for initialization
	void Start () {
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		winObj.SetActive (false);
		lossObj.SetActive (false);
		lossObj2.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (gameStarted);
		closeButton.onClick.AddListener (CloseInstructions);
		if (instructionsClosed) {
			gameStarted = true;
		}

		if (gameOver) {
			lossObj2.SetActive (true);
		}

		if (enemy.GetComponent<M3_Enemy> ().dead) {
			// display win screen
			winObj.SetActive (true);
		} else if (player.GetComponent<M3_Player> ().dead) {
			// else display lose screen
			lossObj.SetActive (true);
		} 
	}

	void CloseInstructions() {
		instructions.SetActive (false);
		instructionsClosed = true;
	}
}
