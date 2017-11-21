using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
<<<<<<< HEAD

=======
	int currentSong = 0;
>>>>>>> master

										//create the health bar
	public GameObject healthBar;		//stores the health bar prefab
	public GameObject barBG;
	GameObject barBG1,barBG2;		//store the attack bar prefab
	public GameObject enemyBar;
	public GameObject HUD;
	public GameObject backgrounds;
	public GameObject instructionAccess;
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
	private GameObject instructions;

	//checks if you are in game
	private bool inGame;
	private bool gameEnd;


	// sound effects
	public AudioSource source;
	public AudioClip arrowSound;
	public AudioClip badArrow;

	public int sceneIdx;
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
		instructions = (GameObject)Instantiate(instructionAccess);
		RotatingGear = (RotatingRhythmGear)Instantiate (RotatingGear);

		PGB_ = (PGB)Instantiate(PGB_);
		player.canMove = false;

		source = GetComponent<AudioSource> ();

		instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
		enemyGauge.transform.position = new Vector3 (enemy.transform.position.x, enemy.transform.position.y + 2, 1);
		healthGauge.transform.position = new Vector3 (player.transform.position.x-3, player.transform.position.y + 2, 1);
		barBG1 = (GameObject)Instantiate (barBG);
		barBG2 = (GameObject)Instantiate (barBG);
		barBG1.transform.position = new Vector3 (enemy.transform.position.x, enemy.transform.position.y + 2, 2);
		barBG2.transform.position = new Vector3 (player.transform.position.x-3, player.transform.position.y + 2, 2);

		Popup = (GameObject)Instantiate (Popup);
		Popup.transform.position = new Vector3(0,0,20);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
			//Destroy(instructions);
			instructions.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.Return) && gameEnd == true) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
		}
//
//		if (Input.GetKeyDown(KeyCode.Return)) {
//			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
//		}
//

		//when the attack bar is full you attack the enemy and it resets the bar
		/*if (attackPoints >= 50) {
			attackPoints = 0;
			enemyHealth -= attackDamage;
			consecutiveHits = 1;

			//set breya animation to attack
			player.changeState (1);
		} 
		*/



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

				Instantiate (Defeat);
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
			enemyGauge.transform.position = new Vector3 (enemy.transform.position.x- (3 - 3*enemyHealth / enemyMaxHealth)*.5f, enemy.transform.position.y + 2, 1);

		} else {
			if (!gameEnd) {
				gameEnd = true;


				//Victory
				Instantiate (Victory);

				if (Input.GetKeyDown(KeyCode.Return)) {
					SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
				}

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
			nextSpawnTime += (.261f * Random.Range(1,4) );									// sets the next interval that it spawns
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

		//display instructions after a round
		if (Time.time > timeEnd + 3  && Time.time > PGB_.shown + 2.5f && health > 0 && enemyHealth > 0) {
			instructions.SetActive (true);
			instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
			//instructions.transform.position = new Vector3 (instructions.transform.position.x, instructions.transform.position.y, -8);
			if (Input.GetKeyDown (KeyCode.Return)) {
				instructions.SetActive (false);
			}
		}


		//after the round ends, press ENTER to start a new round

		if (inGame == false && Input.GetKeyDown (KeyCode.Return) && health > 0 && enemyHealth > 0 && Time.time > PGB_.shown + 2.5f) {
			timeEnd = Time.time + timeLength;
			nextSpawnTime =Time.time + (.261f);
			totalCards = 0;
			correctCards = 0;
			//instructions.SetActive (true);
			instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
			//instructions.transform.position = new Vector3 (instructions.transform.position.x, instructions.transform.position.y, 20);
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

			if(Input.GetKeyDown(KeyCode.UpArrow)){						//checks the card and input
				
			if (arrows.Count >= 1) {								//if the top of the queue matches input

				if (arrows [0].type == 0 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {								//success, else damages you
					//- 1.27f
					correctInput();
				} else if((arrows[0].transform.position.x ) < 1){
					badInput ();
				}
				}
			}
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				if (arrows.Count >= 1) {
				if (arrows [0].type == 1 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {
					correctInput ();
				} else  if(arrows[0].transform.position.x  < 1) {
					badInput ();
				}
				}
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (arrows.Count >= 1) {
				if (arrows [0].type == 2 && arrows[0].transform.position.x  > -1.8f && arrows[0].transform.position.x  <.2f ) {
					correctInput ();
				} else if(arrows[0].transform.position.x < 1) {
					badInput ();
				}
				}
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
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
					//Destroy (arrows [0].gameObject);
					// arrows.RemoveAt (0);
					
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

	}
											//what happens when you get it wrong

	void badInput(){
		source.PlayOneShot (badArrow);
		Destroy (arrows [0].gameObject);
		arrows.RemoveAt (0);
		RotatingGear.changeColor ("bad");


	}


	void animateTheStats(){
		//checks if attack has been played

		if (Time.time > PGB_.shown + 2.5) {
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
				enemyHealth -= attackDamage;

				//set breya animation to attack
				player.changeState (1);

				//PErFECT!!!

			} else if ( totalCards / 2 <  correctCards) {
				PGB_.changeState(2);
				enemyHealth -= attackDamage / 2;
				//set breya animation to attack
				player.changeState (1);
				enemy.changeState (1);
				health -= 10;
				//good
			} else {
				//bad
				PGB_.changeState(3);
				enemy.changeState (1);

				health -= 20;

			}
		}


	}
}