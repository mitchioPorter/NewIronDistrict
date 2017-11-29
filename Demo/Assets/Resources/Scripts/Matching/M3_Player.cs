﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class M3_Player : MonoBehaviour {
	public M3_Player instance;
	public Animator anim;
	public Vector3 startPos;


	public GameObject powerUpBar;
	public ParticleSystem particles;
	public GameObject playerHealthBar;

	private float calc_playerHealth;

	public float weakAtk = 2f;
	public float specialAtk = 20f;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;
	public float barAmount = 0f;
	public float totalAmount = 50f;


	float totalAttackTime = 1.000f;
	float attackTime;
	bool attacking = false;
	public bool dead = false;


	// Audio and Sound Effects;
	private AudioSource source;
	public AudioClip powerFill;
	public AudioClip completeFill;
	public AudioClip playerAttackSound;

	GameObject enemyObj;

	// Use this for initialization
	void Start () {
		instance = GetComponent<M3_Player> ();
		anim = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();

		playerCurrHealth = playerMaxHealth;
		startPos = transform.localPosition;

		// set power bar to 0 in x so bar is empty at start of game
		//powerUpBar.transform.localScale = new Vector3(0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);

		// create reference for enemy 
		enemyObj = GameObject.FindGameObjectWithTag("Enemy");


		// function call to test health bars
		//InvokeRepeating("decreasingHealth", 1f, 1f);

		// function call to test power up bar
		//InvokeRepeating("increaseBar", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (attacking && Time.time > attackTime) {
			anim.SetBool ("IsAttacking", false);
			attacking = false;
		}

	//	particles.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public void setPlayerHealth(float damage) {
		//Debug.Log ("Amount of Damage taken from player health: " + damage);
		playerCurrHealth -= damage;
		//Debug.Log ("player current health: " + playerCurrHealth);
		float newHealth = playerCurrHealth / playerMaxHealth;
		//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
		if (newHealth > 0) {
			playerHealthBar.transform.localScale = new Vector3 (newHealth, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
		} else {
			playerHealthBar.transform.localScale = new Vector3 (0f, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
			powerUpBar.transform.localScale = new Vector3 (0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
			anim.SetBool ("Dead", true);
			dead = true;
		}
	}

	public void setPowerBar(float newAmount) {
		//Debug.Log ("** adding power up bar: " + newAmount);
		barAmount += newAmount;
		//Debug.Log ("plaer bar amount: " + barAmount);
		float calcAmount = barAmount / totalAmount;
		//Debug.Log ("** adding power up bar by factor: " + calcAmount);

		if (barAmount < totalAmount) {
			powerUpBar.transform.localScale = new Vector3 (calcAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
		} 
		if (barAmount >= totalAmount && calcAmount  == 1) {
			transform.localPosition = startPos;
			anim.SetBool("IsAttacking", true);
			//source.PlayOneShot (playerAttackSound);
			attacking = true;
			enemyObj.GetComponent<M3_Enemy> ().setEnemyHealth (weakAtk);
			attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
			// reset power bar
			//powerUpBar.transform.localScale = new Vector3 (0, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
			barAmount = 0;
		}
	}

	public void SetPowerUp(float newAmount) {
		if (barAmount < 50) {
			source.volume = 0.25f;
			source.PlayOneShot (powerFill);
			barAmount += newAmount;
			enemyObj.GetComponent<M3_Enemy> ().setEnemyHealth (weakAtk);
			float calcAmount = barAmount / totalAmount;
		} else if (barAmount >= 50) {   // get 5 matches
			source.volume = 5f;
			particles.Play ();
			source.PlayOneShot (completeFill);
		}
		barAmount = 0;
	}

	public void Attack() {
		transform.localPosition = startPos;
		anim.SetBool("IsAttacking", true);
		source.PlayOneShot (playerAttackSound);
		attacking = true;
		enemyObj.GetComponent<M3_Enemy> ().setEnemyHealth (weakAtk);
		attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
	}

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		playerCurrHealth -= 10f;
		Debug.Log("player current health: " + playerCurrHealth);
		playerHealthBar.transform.localScale = new Vector3 (barAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	}

	// function to test power up bar
	void increaseBar() {
		Debug.Log ("testing player power bar");
		barAmount += 10f;
		Debug.Log ("current player bar amount: " + barAmount);
		powerUpBar.transform.localScale = new Vector3 (barAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	}
}
