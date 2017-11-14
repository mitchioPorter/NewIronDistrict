using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int health;
	//public GameObject healthBar;
	public int maxHealth;
	public bool canMove;
	Animator animator;
	private int state;
	public int wantGear;
	private float lastSlide = 0;
	private bool sliding;

	public int maxLeftX;
	public int maxRightX;
	public int Score;
	//checks if the player ISerializationCallbackReceiver on the ground
	private Rigidbody2D rb;
	private bool onGround;

	private Vector3 velocity;
	SpriteRenderer sprtRndr;

	// sound effects
	public AudioSource source;
	public AudioClip goodGear;
	public AudioClip badGear;

	// Use this for initialization
	void Start () {


		//Instantiate (healthBar);
		wantGear = Random.Range(1,4);
		maxHealth = health;

		animator = GetComponent<Animator> ();
		sprtRndr = GetComponent<SpriteRenderer>();
		velocity = new Vector3 (.1f, 0f, 0f);

		rb = GetComponent<Rigidbody2D> ();
		onGround = true;

		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 100) {
			changeState (0);
		}
			else if (state == 1) {
			state = 100;
			
		}

		if (health > maxHealth) {
			health = maxHealth;
		}

		if(canMove){
			if (!sliding) {
				if (Input.GetKey (KeyCode.RightArrow) && transform.position.x <= maxRightX) {
					transform.Translate (velocity);
					sprtRndr.flipX = false;

				}
				if (Input.GetKey (KeyCode.LeftArrow) && transform.position.x >= maxLeftX) {
					transform.Translate (-1 * velocity);
					sprtRndr.flipX = true;
				}
				if (!Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
					transform.Translate (0f, 0f, 0f);

				}
				if (Input.GetKey (KeyCode.UpArrow) && onGround) {
					rb.AddForce (new Vector2 (0, 300));
					onGround = false;
					animator.SetInteger ("State", 2);
				}
				if (Input.GetKey (KeyCode.Space) && Time.time > lastSlide + 2f) {
					animator.SetInteger ("State", 1);
					lastSlide = Time.time;
					sliding = true;

				}
			} 
			if (sliding && Time.time > lastSlide + .5f) {
				animator.SetInteger ("State", 0);
				sliding = false;
			}


			//when breya is sliding
			if (sliding) {
				if (sprtRndr.flipX == true && transform.position.x >= maxLeftX) {
					transform.Translate (new Vector3 (-.2f, .1f, 0f));
				} else if(sprtRndr.flipX == false && transform.position.x <= maxRightX){
					transform.Translate (new Vector3 (.2f, .1f, 0f));
				}
			}

		}
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (!sliding) {
			if (coll.gameObject.GetComponent<Gear> () != null) {
				int a = coll.gameObject.GetComponent <Gear> ().type;
				if (a == wantGear && canMove) {
					wantGear = Random.Range (1, 4);
					Score += 1;
					source.PlayOneShot (goodGear);
					Destroy (coll.gameObject);

				} else if (a == 0 && canMove) {
					source.PlayOneShot (badGear);
					health -= 20;
					Destroy (coll.gameObject);

				} else if (canMove) {
					source.PlayOneShot (badGear);
					health -= 10;
					Destroy (coll.gameObject);
				}
			}

			if (coll.transform.tag == "Ground") {
				animator.SetInteger ("State", 0);
				onGround = true;
			}
		}
	}



	public void changeState(int state_){
		animator.SetInteger ("State", state_);
		state = state_;
	}
}
