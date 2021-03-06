﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ArrowGame : MonoBehaviour {
										
									//time things, like end time and next create time, and how long a round is
	private float timeEnd;
	private float nextSpawnTime;
	public int timeLength;


									//player/enemy health and their max health
	public int health;
	private int maxHealth;


	private int totalCards;					//This is the stuff that track the cards 
	private int correctCards;				//spawnded per round and how many you got right
	bool animatedOnce =true;

	public int enemyHealth;
	private int enemyMaxHealth;
	private float playerY, enemyY;

	public ArrowCard aCard;
										//queue for arrow cards
	public List<ArrowCard> arrows;

										//Set enemy damage,health
	public int enemyDamage;
										//your "attack" damage
	public int attackDamage = 50;
	float delayB4Round;

										//create the health bar
	public GameObject healthBar;		//stores the health bar prefab
	public GameObject barBG;
	GameObject barBG1,barBG2;		//store the attack bar prefab
	public GameObject enemyBar;
	public GameObject HUD;
	public GameObject backgrounds;
	//public GameObject instructionAccess;
	public GameObject Victory;
	public GameObject Defeat;
	public GameObject Popup;
	public RotatingRhythmGear RotatingGear;

	//these are the popups
	//state 0 = blank, state 1 = perfect, state 2 = good, state 3 = perfect.
	public PGB PGB_;


	public Player player;
	public Enemy enemy;

	private GameObject healthGauge;		//health bar obj
	private GameObject enemyGauge;
	//private GameObject instructions;

	//checks if you are in game
	private bool inGame;
	private bool gameEnd;
	private bool gameStarted;


	// sound effects
	public AudioSource source;
	public AudioSource source2;
	public AudioClip arrowSound;
	public AudioClip badArrow;


	public AudioClip song1, song2, song3;
	int currentSong = 0;
	float spawnTime;

	public int sceneIdx;

	public GameObject instructions;
	public Button closeBtn;

	// Use this for initialization
	void Start () {
		
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		totalCards = 0;
		correctCards = 0;
		//this sets how long befor the game stops instantiating arrows
		timeEnd = Time.time -1;

		maxHealth = health;				//set player and enemy max health to initial health
		enemyMaxHealth = enemyHealth;
		nextSpawnTime = Time.time;


		Instantiate (backgrounds);	
		Instantiate (HUD);	

		//Create the enemy and player
		enemy = (Enemy)Instantiate (enemy);
		player = (Player)Instantiate (player);
		playerY = player.transform.position.y;
		enemyY = enemy.transform.position.y;
		//create the bars for your and enemy health
		healthGauge = (GameObject)Instantiate (healthBar);
		enemyGauge = (GameObject)Instantiate (enemyBar);
		//instructions = (GameObject)Instantiate(instructionAccess);
		RotatingGear = (RotatingRhythmGear)Instantiate (RotatingGear);

		PGB_ = (PGB)Instantiate(PGB_);
		player.canMove = false;

		source = GetComponent<AudioSource> ();

		//instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
		enemyGauge.transform.position = new Vector3 (enemy.transform.position.x+3, enemy.transform.position.y + 2, 1);
		healthGauge.transform.position = new Vector3 (player.transform.position.x-3, player.transform.position.y + 2, 1);
		barBG1 = (GameObject)Instantiate (barBG);
		barBG2 = (GameObject)Instantiate (barBG);
		barBG1.transform.position = new Vector3 (enemy.transform.position.x+3, enemy.transform.position.y + 2, 2);
		barBG2.transform.position = new Vector3 (player.transform.position.x-3, player.transform.position.y + 2, 2);

		Popup = (GameObject)Instantiate (Popup);
		Popup.transform.position = new Vector3(0,0,20);
		//instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
		//instructions.SetActive (true);

		gameStarted = false;

		Victory.SetActive (false);
		Defeat.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		closeBtn.onClick.AddListener (CloseInstructions);;
//		if (Input.anyKeyDown) {
//			
//			if (!gameStarted) {
//				instructions.SetActive(false);
//				//instructions.transform.position = new Vector3 (instructions.transform.position.x, instructions.transform.position.y, 20);
//				gameStarted = true;
//				timeLength = 20;
//				timeEnd = Time.time + timeLength;
//				nextSpawnTime = Time.time + (spawnTime);
//				totalCards = 0;
//				correctCards = 0;
//				spawnTime = .260869f;
//				source2.clip = song1;
//				source2.Play ();
//			}
//		}

//		if (Input.GetKeyDown(KeyCode.Return) && gameEnd == true) {
//			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
//		}
			

//if you are still alive
		if (health > 0) {
			healthGauge.transform.localScale = new Vector3 ((float)3 * health / maxHealth, .3f, 1f);
			healthGauge.transform.position = new Vector3 (player.transform.position.x- 3 - (3 - 3 *health / maxHealth)*.5f, player.transform.position.y + 2, 1);

		} else {
			if (!gameEnd) {
				
	//GAME OVER DEATH
	//stops arrow spawns
				healthGauge.transform.localScale = new Vector3 ((float)3 * health / maxHealth, .3f, 1f);
				timeEnd = Time.time - 2f;
				player.changeState (66);

	//create death display

				//Instantiate (Defeat);
				Defeat.SetActive(true);
				gameEnd = true;


	//removes all the extra arrow cards
			while (arrows.Count > 0) {
				Destroy (arrows [0].gameObject);
				arrows.RemoveAt (0);
				}
			}

		}

	//changes health gauge
		if (enemyHealth > 0) {
			enemyGauge.transform.localScale = new Vector3 (((float)3 * enemyHealth / enemyMaxHealth), .3f, 1f);
			enemyGauge.transform.position = new Vector3 (enemy.transform.position.x+3- (3 - 3*enemyHealth / enemyMaxHealth)*.5f, enemy.transform.position.y + 2, 1);

		} else {
			if (!gameEnd) {
				gameEnd = true;

				//Victory
				//Instantiate (Victory);
				Victory.SetActive(true);
				//if (Input.GetKeyDown(KeyCode.Return)) {
				//	SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
				//}

				enemy.changeState (66);
				enemyGauge.transform.localScale = new Vector3 (((float)12.4 * enemyHealth / enemyMaxHealth), .5f, 1f);


				timeEnd = Time.time - 1;
				int y = 0;
				while (y < arrows.Count) {
					Destroy (arrows [0].gameObject);
					arrows.RemoveAt (0);
					y++;
				}
			}
		}
			

//THIIS IS EWHERE WE CREATE CARDS

		if (Time.time <= timeEnd && Time.time >= nextSpawnTime) {		//every round within the time limit it will spawn a arrow card
			inGame = true;
			arrows.Add ((ArrowCard)Instantiate (aCard));			//instantiates the arrow card prefan
			arrows[arrows.Count-1].velocity = new Vector3 (spawnTime * 12,0,0);
			//nextSpawnTime += (spawnTime * 2 );	
			nextSpawnTime += (spawnTime * (int)Random.Range(1,3) );									// sets the next interval that it spawns
			totalCards += 1;
			animatedOnce = false;
		//htis is post all cards created
		} else {
			if (Time.time > timeEnd + 3) {
				//after the end of the sequence
				inGame = false;
				animateTheStats ();
			}
		}


//after the round ends, autostart after time

		if (inGame == false && health > 0 && enemyHealth > 0 && Time.time > PGB_.shown + delayB4Round && gameStarted == true) {
			
			currentSong = (currentSong + 1) % 3;


			if (currentSong == 0) {
				source2.clip = song1;
				source2.Play ();
				spawnTime = .260869f;
				timeLength = 20;
				nextSpawnTime =Time.time + (spawnTime)* 1 ;

			}
			if (currentSong == 1) {
				source2.clip = song2;
				source2.Play ();
				spawnTime = .3f;
				timeLength = 15;
				nextSpawnTime =Time.time + (spawnTime)* 1 ;
			}
			if (currentSong == 2) {
				source2.clip = song3;
				source2.Play ();
				//spawnTime = .22222f;
				spawnTime = .272727272727f;
				timeLength = 17;
				nextSpawnTime =Time.time + (spawnTime)* 2 ;

			}


			inGame = true;
			timeEnd = Time.time + timeLength;
			totalCards = 0;
			correctCards = 0;
		}


								/*\\----------------------------
								 * -----------------------------
								 *----- IMPORTANT: 	------------
								 *-----  UP == 0	------------
								 *-----  Down == 1	------------
								 *-----  Left == 2	------------
								 *-----  Right == 3	------------
								 * -----------------------------
								 * ----------------------------
								*/

		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){						//checks the card and input
					
				if (arrows.Count >= 1) {								//if the top of the queue matches input

					if (arrows [0].type == 0 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {								//success, else damages you
						//- 1.27f
						correctInput();
					} else if((arrows[0].transform.position.x ) < 1){
						badInput ();
					}
				}
			}
		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
				if (arrows.Count >= 1) {
					if (arrows [0].type == 1 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {
						correctInput ();
					} else  if(arrows[0].transform.position.x  < 1) {
						badInput ();
					}
				}
			}
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
				if (arrows.Count >= 1) {
					if (arrows [0].type == 2 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {
						correctInput ();
					} else if(arrows[0].transform.position.x < 1) {
							badInput ();
							}
				}
			}
		if (Input.GetKeyDown (KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.D)) {
				if (arrows.Count >= 1) {
					if (arrows [0].type == 3 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ){
						correctInput ();
					} else if((arrows[0].transform.position.x ) < 1) {
							badInput ();	
							}
				}
			}

			
		if (arrows.Count >= 1) {
			if (arrows [0].transform.position.x <= -1.8f) {
					badInput ();
				}
			}


		}
		

//what happens if you get it right
	void correctInput(){

		//plays good sound
		source.PlayOneShot (arrowSound);
		//destroys object
		Destroy (arrows [0].gameObject);
		//removes it from the array
		arrows.RemoveAt (0);
		correctCards += 1;
		RotatingGear.changeColor ("good");
		if (enemyHealth >= 2) {
			enemyHealth -= 1;
		}
		if (correctCards % 50 == 0) {
			player.animator.SetTrigger("Attack");
		}
	}

//what happens when you get it wrong

	void badInput(){
		source.PlayOneShot (badArrow);
		Destroy (arrows [0].gameObject);
		arrows.RemoveAt (0);
		RotatingGear.changeColor ("bad");
		enemy.animator.SetTrigger ("Attack");
		if (health >= 3) {
			health -= 5;
		}
	}


	void animateTheStats(){
//checks if attack has been played

		if (Time.time > PGB_.shown + delayB4Round) {
			Popup.transform.position = new Vector3(0,0,20);
			player.transform.position = new Vector3(player.transform.position.x, playerY,-1);
			enemy.transform.position = new Vector3(enemy.transform.position.x, enemyY,-1);;
		}
		if (!animatedOnce) {
			Popup.transform.position = new Vector3(0,0,0);
			player.transform.position = new Vector3(player.transform.position.x, -1,-8);
			enemy.transform.position = new Vector3(enemy.transform.position.x, -1,-8);;

			RotatingGear.changeColor ("neutral");
			PGB_.changeState(1);
			animatedOnce = true;
			if (totalCards == correctCards) {
				enemyHealth -= 10;

				//set breya animation to attack
				player.changeState (1);
				delayB4Round = 2f;
				//PErFECT!!!

			} else if ( totalCards / 2 <  correctCards) {
				PGB_.changeState(2);
				enemyHealth -= 5;
				//set breya animation to attack
				player.changeState (1);
				enemy.changeState (1);
				health -= 5;
				delayB4Round = 2.5f;
				//good
			} else {
				//bad
				PGB_.changeState(3);
				enemy.changeState (1);
				health -= 10;
				delayB4Round = 2.3f;

			}
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
		//instructions.transform.position = new Vector3 (instructions.transform.position.x, instructions.transform.position.y, 20);
		gameStarted = true;
		timeLength = 20;
		timeEnd = Time.time + timeLength;
		nextSpawnTime = Time.time + (spawnTime);
		totalCards = 0;
		correctCards = 0;
		spawnTime = .260869f;
		source2.clip = song1;
		source2.Play ();
	}
}