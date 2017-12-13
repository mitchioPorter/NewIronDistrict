﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	Animator anim;
	Rigidbody2D rigidBody;
	SpriteRenderer render;
	public SentinelScript senPrefab;

	private bool onGround;
	private Vector3 velocity;

	// attack stuff
	float totalAttackTime = 1.000f;
	public int attackDamage;
	float attackTime;
	float attackStart;
	bool attacking = false;


	public int chargeAmount;

	public AudioSource source;
	public AudioClip lowHealth;
	public AudioClip attackSound;
	public AudioClip hitSound;


	public GameObject healthBar;
	private float calc_playerHealth;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;
	public bool gameStarted;
	public bool dead = false;
	public bool damaged;
	public bool dying;

	private bool flashActive;
	public float flashLength;
	private float flashCounter;
	private Color origColor;

	public Manager manager;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();

		origColor = render.color;
		flashLength = 0.5f;

		gameStarted = false;
		onGround = true;
		velocity = new Vector3 (.1f, 0f, 0f);

		playerCurrHealth = playerMaxHealth;
		attackDamage = 10;
		attackStart = -5;
		senPrefab = FindObjectOfType<SentinelScript> ();

		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		gameStarted = manager.GetComponent<Manager> ().gameStarted;
		if (gameStarted) {
			if (!dead) {
				//Debug.Log ("PLAYER'S HEALTH: " + playerCurrHealth);

				// jump
				if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow) && onGround) {
					Debug.Log ("** JUMPING **");
					anim.SetBool ("Jumping", true);
					//rigidBody.AddForce (new Vector2 (0, 80));
					rigidBody.AddForce (new Vector2 (0, Mathf.Clamp (80, 1, 60)));
					onGround = false;
				}
			
				// attacking
				if (Input.GetKey (KeyCode.Space) && Time.time > attackStart + 1f) {
					Debug.Log ("** ATTACKING **");
					source.PlayOneShot (attackSound);
					attacking = true;

					anim.SetTrigger ("Attack");
					anim.SetBool ("IsAttacking", true);

					attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
					attackStart = Time.time;

					if (transform.position.x > 1 && transform.position.y < -1) {
						senPrefab.GetComponent<SentinelScript> ().setEnemyHealth (10);
					}

					if (attacking && Time.time > attackTime) {
						anim.SetBool ("IsAttacking", false);
						attacking = false;
					}
				}
				// move right
				if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && transform.position.x < 1.5f) {
					Debug.Log ("**RIGHT **");
					transform.Translate (velocity);
					anim.SetBool ("Walking", true);
					render.flipX = false;
				}

				// move left
				if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && transform.position.x > -7) {
					Debug.Log ("** LEFT **");
					transform.Translate (-1 * velocity);
					anim.SetBool ("Walking", true);
					render.flipX = true;
				}

				if (!Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
					transform.Translate (0f, 0f, 0f);
					anim.SetBool ("Walking", false);
				}
			}


			// make player flash red when hit by changing RGB values of sprite
			if (flashActive) {
				if (flashCounter > flashLength * .66f) {
					render.color = new Color (render.color.r, 0f, 0f, render.color.a); // red
				} else if (flashCounter > flashLength * .33f) {
					render.color = origColor; // normal
				} else if (flashCounter > 0f) {
					render.color = new Color (render.color.r, 0f, 0f, render.color.a); // final red
				} else {
					render.color = origColor; // back to normal
					flashActive = false;
				}
				flashCounter -= Time.deltaTime;
			}
		}
	}


	public void setPlayerHealth(float damage) {
		if (!dead) {
			source.PlayOneShot (hitSound);
			Debug.Log ("Amount of Damage taken from player health: " + damage);
			playerCurrHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;

			//Debug.Log ("player current health: " + playerCurrHealth);
			float newHealth = playerCurrHealth / playerMaxHealth;
			//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
			if (newHealth > 0) {
				healthBar.transform.localScale = new Vector3 (newHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
			} else {
				healthBar.transform.localScale = new Vector3 (0f, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
				anim.SetBool ("Dead", true);
				dead = true;
			}

			//if (playerCurrHealth <= 50) {
			//	LowHealth ();
			//}
		}
	}

	void LowHealth() {
		source.PlayOneShot (lowHealth);
	}
		
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.tag == "Ground" ) {
			anim.SetBool("Jumping", false);
			onGround = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Player Attack Damage: " + attackDamage);
		if (other.tag == "Enemy") {
			Debug.Log ("** PLAYER HIT ATTACKED ENEMY **");
			// enemy lose health
			//enemy.GetComponent<EnemyController>().setEnemyHealth(attackDamage);
		}
	}
		
	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		playerCurrHealth -= 10f;
		Debug.Log("player current health: " + playerCurrHealth);
		float newHealth = playerCurrHealth / playerMaxHealth;
		healthBar.transform.localScale = new Vector3 (newHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
