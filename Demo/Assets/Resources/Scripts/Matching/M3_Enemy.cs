using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Enemy : MonoBehaviour {
	public M3_Enemy instance;
	public Animator anim;

	public GameObject playerObj;
	public GameObject enemyHealthBar;

	float enemyAttackDamage = 15f;
	public float enemyMaxHealth = 50f;
	public float enemyCurrHealth = 0f;

	float enemyAttackTime = 0.633f;
	float attackTime;
	private bool attacking = false;
	public bool dead = false;

	//GameObject timerObj;
	public float time;
	public float setTime = 10;
	public float waitTime = 3f;
	private bool timeUp;

	private AudioSource source;
	public AudioClip enemyStruck;
	public AudioClip enemyAttackSound;

	// Use this for initialization
	void Start () {
		instance = GetComponent<M3_Enemy> ();
		anim = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();

		playerObj = GameObject.FindGameObjectWithTag ("Player");

		enemyCurrHealth = enemyMaxHealth;

		time = setTime;
		timeUp = false;

		// set power bar to 0 in x so bar is empty at start of game
		//powerUpBar.transform.localScale = new Vector3(0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);

		// function call to test health bars
		//InvokeRepeating("decreasingHealth", 1f, 1f);
		// function call to test power up bar
		//InvokeRepeating("increaseBar", 1f, 1f);

		//InvokeRepeating ("TimeAttack", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;

		if (time <= 0) {
			TimeAttack();
		}

		if (attacking && Time.time > attackTime) {
			anim.SetBool ("IsAttacking", false);
			attacking = false;
		}
	}

	public void setEnemyHealth(float damage) {
		//Debug.Log ("Amount of Damage taken from enemy health: " + damage);
		source.PlayOneShot (enemyStruck);
		enemyCurrHealth -= damage;
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		//Debug.Log ("changing enemyhealth bar by factor:" + newHealth);
		if (newHealth > 0) {
			enemyHealthBar.transform.localScale = new Vector3 (newHealth, enemyHealthBar.transform.localScale.y, enemyHealthBar.transform.localScale.z);
		} else {
			enemyHealthBar.transform.localScale = new Vector3 (0f, enemyHealthBar.transform.localScale.y, enemyHealthBar.transform.localScale.z);
			//powerUpBar.transform.localScale = new Vector3 (0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
			anim.SetBool ("Dead", true);
			dead = true;
			Debug.Log ("Enemy Died!!");
		}
	}

	public void AttackPlayer() {
		Debug.Log ("Enemy Attack Player");
		anim.SetBool ("IsAttacking", true);
		source.PlayOneShot (enemyAttackSound);
		attacking = true;
		playerObj.GetComponent<M3_Player> ().setPlayerHealth (enemyAttackDamage);
		attackTime = Time.time + enemyAttackTime;
	}

	void TimeAttack() {
		Debug.Log ("TIME RAN OUT!! ENEMY ATTACKING!!");
		//anim.SetBool ("IsAttacking", true);
		anim.SetTrigger ("IsAttacking");
		playerObj.GetComponent<M3_Player> ().setPlayerHealth (enemyAttackDamage);
		source.PlayOneShot (enemyAttackSound);
		resetTimer();
	}

	public void resetTimer() {
		time = setTime;
	}

	// function to test health bar
	void decreasingHealth() {
		enemyCurrHealth -= 2f;
		Debug.Log (enemyCurrHealth);
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		Debug.Log ("testing enemy health bar");
		enemyHealthBar.transform.localScale = new Vector3 (newHealth, enemyHealthBar.transform.localScale.y, enemyHealthBar.transform.localScale.z);
	}

	// function to test power up bar
//	void increaseBar() {
//		Debug.Log ("testing enemy power bar");
//		barAmount += 10f;
//		Debug.Log ("current enemy bar amount: " + barAmount);
//		powerUpBar.transform.localScale = new Vector3 (barAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
//	}

	//	public void setPowerBar(float newAmount) {
	//		//Debug.Log ("** adding enemy power up bar: " + newAmount);
	//		barAmount += newAmount;
	//		//Debug.Log ("enemy bar amount: " + barAmount);
	//		float calcAmount = barAmount / totalAmount;
	//		//Debug.Log ("** adding enemy power up bar by factor: " + calcAmount);
	//
	//		if (barAmount < totalAmount) {
	//			powerUpBar.transform.localScale = new Vector3 (calcAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	//		}
	//
	//		if (barAmount >= totalAmount && calcAmount  == 1) {
	//			anim.SetBool("IsAttacking", true);
	//			source.PlayOneShot (enemyAttackSound);
	//			playerObj.GetComponent<M3_Player> ().setPlayerHealth (enemyAttackDamage);
	//			attacking = true;
	//			attackTime = Time.time + enemyAttackTime;
	//			// reset power bar
	//			powerUpBar.transform.localScale = new Vector3 (0, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	//			barAmount = 0;
	//		}
	//	}
}
