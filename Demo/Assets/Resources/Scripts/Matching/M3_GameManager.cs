using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class M3_GameManager : MonoBehaviour {
	public static M3_GameManager instance;
	public GameObject tile;
	public M3_Player player;
	public M3_Enemy enemy;

	// UI
	public GameObject winObj;
	public GameObject lossObj;
	public GameObject instructions;

	public bool gameOver;
	public bool gameStarted;

	public int sceneIdx;

	public Button moveOnButton;
	public Button reloadButton1;
	public Button reloadButton2;

	// Use this for initialization
	void Start () {
		instance = GetComponent<M3_GameManager> ();

		gameOver = false;
		gameStarted = false;

		gameOver = tile.GetComponent<Tile> ().gameEnd;
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		winObj.SetActive (false);
		lossObj.SetActive (false);

		//instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
			
		//winObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//winObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		//lossObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//lossObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("** IN GAME MANAGER **");
		if (Input.anyKey) {
			Debug.Log ("** pressing buttons in game manager ** ");
			instructions.SetActive (false);
			gameStarted = true;
		}

		if (enemy.GetComponent<M3_Enemy> ().dead) {
			Debug.Log ("You Won!!");
			// display win screen
			//Instantiate (winObj);
			winObj.SetActive(true);
			moveOnButton.onClick.AddListener (LoadNextScene);
			reloadButton1.onClick.AddListener (ReloadLevel);
		}

		if (player.GetComponent<M3_Player>().dead) {
			Debug.Log ("You Lost!!");
			//Instantiate (lossObj);
			// else display lose screen
			lossObj.SetActive(true);
			reloadButton2.onClick.AddListener (ReloadLevel);
		} 

//		if (Input.GetKeyDown(KeyCode.Return)) {
//			SceneManager.LoadScene(sceneIdx + 1);
//		}

	}

	void LoadNextScene() {
		SceneManager.LoadScene(sceneIdx + 1);
	}

	void ReloadLevel() {
		SceneManager.LoadScene (sceneIdx);

	}
}
