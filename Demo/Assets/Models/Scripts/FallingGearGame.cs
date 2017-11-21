using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingGearGame : MonoBehaviour
{
	public int time;
	private double nextSpawnTime;
	public Gear falling;
	private int health;
	private int maxHealth;
	public int wantGear;
	public int Score = 0;

	public thoughtBubble thought;
	public GameObject victory;
	public GameObject defeat;
	public GameObject healthBar;
	public GameObject player;
	public GameObject backgrounds;
	public GameObject progress;
	private bool endGame;

	private int sceneIdx;

	public AudioClip rightGear;
	public AudioClip wrongGear;
	private AudioSource source;

	// Use this for initialization
	void Start ()
	{
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
		source = GetComponent<AudioSource> ();

		health = 10;
		player = (GameObject)Instantiate (player);
		healthBar = (GameObject)Instantiate (healthBar);
		healthBar.transform.position -= new Vector3 (3, 0, 0);
		maxHealth = health;
		Instantiate (backgrounds);
		nextSpawnTime = Time.time;
		progress = (GameObject)Instantiate (progress);
		endGame = false;
		thought = (thoughtBubble)Instantiate (thought);
		wantGear = Random.Range (1, 4);
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetMouseButtonDown (0) && endGame == true) {
			SceneManager.LoadScene (sceneIdx + 1);
		}

		progress.transform.localScale = new Vector3 ((float)Score / 2f, .5f, 1f);

		if (Score >= 10 && !endGame) {
//VICTORY
			endGame = true;
			Instantiate (victory);


		}

		if ((health <= 0) && !endGame) {
//DEATH
			endGame = true;
			Instantiate (defeat);
			Destroy (healthBar);

//			if (Input.GetMouseButtonDown (0)) {
//				SceneManager.LoadScene (sceneIdx + 1);
//			}
		}


		thought.type = wantGear;
		thought.changeState (thought.type);

		healthBar.transform.localScale = new Vector3 ((float)8 * health / maxHealth, .5f, 1f);

	
		if (Time.time >= nextSpawnTime && !endGame) {
			Instantiate (falling);
			nextSpawnTime += Random.Range (.8f, 1.5f);
		}
	
		//this check for touch controls
		if (Input.touchCount > 0) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Vector2 touchPos = new Vector2 (worldPos.x, worldPos.y);


			//on click it checks if overlaos with gear scripted object
			var hit = Physics2D.OverlapPoint (touchPos);
			if (hit && hit.gameObject.GetComponent<Gear> () != null) {
				int a = hit.gameObject.GetComponent <Gear> ().type;
				if (a == wantGear) {
					wantGear = Random.Range (1, 4);
					Score += 1;
					Destroy (hit.gameObject);
					source.PlayOneShot(rightGear);

				} else if (a == 0 ) {
					health -= 20;
					Destroy (hit.gameObject);
					source.PlayOneShot(wrongGear);
				} else  {
					health -= 10;
					Destroy (hit.gameObject);
					source.PlayOneShot (wrongGear);
				}
			}
				
		}
	}
}
