﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public int sceneIdx;
	public bool gameStarted;

	public GameObject instructions;
	public GameObject instructionsClone;

	// Use this for initialization
	void Start () {

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

		Instantiate (backgrounds);
		nextSpawnTime = Time.time;
		player.canMove = true;


		//SET THE MOVEMENT BOUNDS FOR THE PLAYER
		player.maxLeftX = -7;
		player.maxRightX = 9;

		soren = (Guy_thoughts)Instantiate (soren);
		thought = (thoughtBubble)Instantiate (thought);


		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		instructionsClone = Instantiate (instructions);

		victory.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		victory.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		defeat.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		defeat.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			Destroy (instructionsClone);
		}
		if (endGame) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
			}
		}

		progress.transform.localScale = new Vector3 ((float)player.Score / 1f, .5f, 1f);

		if (player.Score == 10 && !endGame){
//VICTORY
			endGame = true;
			player.canMove = false;
			Instantiate (victory);
		}
		if ((player.health <= 0) && !endGame) {
//DEATH
			endGame = true;
			player.canMove = false;
			Instantiate (defeat);
			Destroy (healthBar);

		}
		thought.type = player.wantGear;
		thought.changeState (thought.type);

		health = player.health;
		healthBar.transform.localScale = new Vector3 ((float) 8 *health / maxHealth,.5f, 1f);

	
		if (Time.time >= nextSpawnTime && !endGame) {
			Instantiate (falling);
			nextSpawnTime += Random.Range (.8f, 1.5f);
		}
	
		
	}
}
