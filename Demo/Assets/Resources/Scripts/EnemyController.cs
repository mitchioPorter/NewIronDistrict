using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	Animator anim;
	public GameObject missile;
	public Transform launchPoint;
	public AudioSource source;

	public float speed;
	public float playerRange;
	public PlayerController player;

	public float waitBetweenShots;
	private float shotCounter;
	public int attackDamage;  // regular attack outside of missiles

	public GameObject enemyBar;
	private float calc_EnemyHealth;
	public float enemyMaxHealth = 100f;
	public float enemyCurrHealth = 0f;
	public bool dead = false;
	public bool onGround;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		shotCounter = waitBetweenShots;
		enemyCurrHealth = enemyMaxHealth;
		attackDamage = 10;
		onGround = true;

		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("ENEMY HEALTH: " + enemyCurrHealth);
		ShootMissileAtPlayer ();
		shotCounter -= Time.deltaTime;
	
	}

	public void setEnemyHealth(float damage) {
		Debug.Log ("Amount of Damage taken from enemy health: " + damage);
		enemyCurrHealth -= damage;
		//Debug.Log ("player current health: " + playerCurrHealth);
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
		if (newHealth > 0) {
			enemyBar.transform.localScale = new Vector3 (newHealth, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
		} else {
			enemyBar.transform.localScale = new Vector3 (0f, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
			anim.SetBool ("Dead", true);
			dead = true;
		}
	}

	void ShootMissileAtPlayer() {
		// right side
//		if (transform.localScale.x < 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange) {
//			Instantiate (missile, launchPoint.position, launchPoint.rotation);
//		}

		// left side
		if (!dead) {
			if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && shotCounter < 0) {
				Instantiate (missile, launchPoint.position, launchPoint.rotation);
				shotCounter = waitBetweenShots; //reset counter
			}
		}
	}

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		enemyCurrHealth -= 10f;
		Debug.Log("player current health: " + enemyCurrHealth);
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		enemyBar.transform.localScale = new Vector3 (newHealth, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
	}
}
