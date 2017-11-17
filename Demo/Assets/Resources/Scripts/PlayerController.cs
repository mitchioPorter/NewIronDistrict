using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Animator anim;
	Rigidbody2D rigidBody;
	public AudioSource source;
	public EnemyController enemy;

	private bool onGround;
	private Vector3 velocity;

	private float dist;

	private float leftBorder;
	private float rightBorder;

	// attack stuff
	float totalAttackTime = 1.000f;
	public int attackDamage;
	float attackTime;
	bool attacking = false;
	public AudioClip attackSound;


	public GameObject healthBar;
	private float calc_playerHealth;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;
	public bool dead = false;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();

		onGround = true;
		velocity = new Vector3 (.1f, 0f, 0f);

		playerCurrHealth = playerMaxHealth;
		attackDamage = 10;

		// check of player is at screen edge
		dist = (transform.position - Camera.main.transform.position).z;
		leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,dist)).x;
		rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1f,0f,dist)).x;

		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("PLAYER'S HEALTH: " + playerCurrHealth);
		// jump
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			Debug.Log ("pressed key to jump");
			rigidBody.AddForce (new Vector2 (0, 75));
			onGround = false;
			anim.SetBool ("Jumping", true);
		}
			
		// attacking
		if (Input.GetKey (KeyCode.Space)) {
			source.PlayOneShot (attackSound);
			anim.SetBool ("IsAttacking", true);
			attacking = true;
			enemy.GetComponent<EnemyController>().setEnemyHealth(2);
			attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
		}
			

		if (attacking && Time.time > attackTime) {
			anim.SetBool ("IsAttacking", false);
			attacking = false;
		}	
	}

	public void setPlayerHealth(float damage) {
		Debug.Log ("Amount of Damage taken from player health: " + damage);
		playerCurrHealth -= damage;
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
	}

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		playerCurrHealth -= 10f;
		Debug.Log("player current health: " + playerCurrHealth);
		float newHealth = playerCurrHealth / playerMaxHealth;
		healthBar.transform.localScale = new Vector3 (newHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.tag == "Ground") {
			anim.SetBool("Jumping", false);
			onGround = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Player Attack Damage: " + attackDamage);
		if (other.tag == "Enemy") {
			Debug.Log ("** PLAYER HIT ATTACKED ENEMY **");
			// enemy lose health
			enemy.GetComponent<EnemyController>().setEnemyHealth(attackDamage);
		}
	}
}
