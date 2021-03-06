﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FallingGearGame : MonoBehaviour {
	public int time;
	private double nextSpawnTime;
	public Gear falling;
	private int health;
	private int maxHealth;

	public Guy_thoughts soren;
	public thoughtBubble thought;
	public GameObject victory;
	public GameObject defeat;
	public GameObject healthBar; 
	public Player player;
	public GameObject backgrounds;
	public GameObject ground;
	public GameObject progress;
	private bool endGame;
	private bool gameStart;
	public GameObject gearThing;
	public GameObject[] gearThingList = new GameObject[10];

	public int sceneIdx;
	public bool gameStarted;

	public GameObject instructions;
	public Button closeBtn;
	//public GameObject instructionsClone;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			gearThingList [i] = Instantiate ((GameObject)gearThing);
			gearThingList [i].transform.position = new Vector2 (gearThingList [i].transform.position.x + .9f * i, gearThingList [i].transform.position.y);

			if (i % 2 == 1) {
				gearThingList [i].GetComponent<SpriteRenderer> ().flipX = true;
				gearThingList [i].transform.rotation = Quaternion.Euler(0, 0, -28.25f);
			}
		}

		gameStart = false;
		player = (Player)Instantiate (player);

		healthBar = (GameObject)Instantiate (healthBar);
		healthBar.transform.Translate (new Vector3 (-9, 0, 0));
		healthBar.transform.Rotate(new Vector3(0,0,90));

		progress = (GameObject)Instantiate (progress);
		progress.transform.Translate (new Vector3 (-8, 0, 0));
		progress.transform.Rotate(new Vector3(0,0,90));

		ground = (GameObject)Instantiate (ground);
		health = player.health;
		maxHealth = player.health;
		nextSpawnTime = Time.time;
		Instantiate (backgrounds);

		player.canMove = true;


		//SET THE MOVEMENT BOUNDS FOR THE PLAYER
		player.maxLeftX = -7;
		player.maxRightX = 9;

		soren = (Guy_thoughts)Instantiate (soren);
		thought = (thoughtBubble)Instantiate (thought);


		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		//instructions.transform.localScale = new Vector3 (.02f, .02f, .02f);
		//instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		//instructionsClone = Instantiate (instructions);

		//victory.transform.localScale = new Vector3 (1f, 1f, 1f);
		//victory.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		//defeat.transform.localScale = new Vector3 (1f, 1f, 1f);
		//defeat.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
		victory.SetActive(false);
		defeat.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		closeBtn.onClick.AddListener (CloseInstructions);
//		if (Input.anyKeyDown && !gameStart) {
//			gameStart = true;
//			//Destroy (instructionsClone);
//			instructions.SetActive(false);
//			nextSpawnTime = Time.time;
//		}
			
		progress.transform.localScale = new Vector3 ((float)player.Score / 1f, .5f, 1f);

		//score tracker
		if (player.Score > 0 && !endGame){
			for (int i = 0; i < player.Score; i++) {
				gearThingList [i].transform.position = new Vector3 (-6f + .9f * i, gearThingList [i].transform.position.y,-6);
			}
		}


		//VICTORY
		if (player.Score == 10 && !endGame){
			victory.SetActive (true);
			endGame = true;
			player.canMove = false;
			//Instantiate (victory);
		}
		//DEATH
		if ((player.health <= 0) && !endGame) {
			defeat.SetActive (true);
			endGame = true;
			player.canMove = false;
			//Instantiate (defeat);
			Destroy (healthBar);

		}

		if (!endGame) {
			thought.type = player.wantGear;
			thought.changeState (thought.type);

			health = player.health;
			healthBar.transform.localScale = new Vector3 (.2f, (float)3 * health / maxHealth, 1f);
			healthBar.transform.position = new Vector3 (player.transform.position.x - (3 - 3 * health / maxHealth) * .5f, player.transform.position.y + 2, 1);
		}


	
		if (Time.time >= nextSpawnTime && !endGame && gameStart) {
			Instantiate (falling);
			nextSpawnTime += Random.Range (.3f, .8f);
		}
	
		
	}

	void ReloadLevel() {
		SceneManager.LoadScene (sceneIdx);
	}

	void NextScene() {
		SceneManager.LoadScene (sceneIdx + 1);
	}

	void CloseInstructions() {
		instructions.SetActive(false);
		gameStart = true;
		nextSpawnTime = Time.time;
	}
}
