using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    //public GameObject healthBar;
    public int maxHealth;
    public bool canMove;
    public Animator animator;
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

    private float invinciTimer;

    private Vector3 velocity;
    SpriteRenderer sprtRndr;

    // sound effects
    public AudioSource source;
    public AudioClip goodGear;
    public AudioClip badGear;

    // Use this for initialization
    void Start()
    {


        //Instantiate (healthBar);
        wantGear = Random.Range(1, 4);
        maxHealth = health;

        animator = GetComponent<Animator>();
        sprtRndr = GetComponent<SpriteRenderer>();
        velocity = new Vector3(.1f, 0f, 0f);

        rb = GetComponent<Rigidbody2D>();
        onGround = true;

        invinciTimer = 0.0f;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.position.y < -2.5) {
			onGround = true;
		}
        invinciTimer += Time.deltaTime;
        if (state == 100)
        {
            changeState(0);
        }
        else if (state == 1)
        {
            state = 100;

        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (canMove)
        {
            if (!sliding)
            {
				if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && transform.position.x <= maxRightX)
                {
                    transform.Translate(velocity);
                    sprtRndr.flipX = false;

                }
				if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && transform.position.x >= maxLeftX)
                {
                    transform.Translate(-1 * velocity);
                    sprtRndr.flipX = true;
                }
				if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Translate(0f, 0f, 0f);

                }
				if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && onGround)
                {
                    rb.AddForce(new Vector2(0, 80));
                    onGround = false;
                    animator.SetInteger("State", 2);
                }
                if (Input.GetKey(KeyCode.Space) && Time.time > lastSlide + 1.5f)
                {
					animator.SetTrigger ("Slide");
                    lastSlide = Time.time;
                    sliding = true;

                }

            }
            if (sliding && Time.time > lastSlide + .7f)
            {
                animator.SetInteger("State", 0);
                sliding = false;
				sprtRndr.color = new Color (1,1,1,1);
            }


            //when breya is sliding
			if (sliding) {

				sprtRndr.color = new Color (.9f, .9f, .9f, .8f);

				if (sprtRndr.flipX == true && transform.position.x >= maxLeftX) {
					transform.Translate (new Vector3 (-.25f, 0, 0f));
				} else if (sprtRndr.flipX == false && transform.position.x <= maxRightX) {
					transform.Translate (new Vector3 (.25f, 0, 0f));
				}
			} else {
				rb.gravityScale = 2;
			}

        }

    }


    void OnCollisionEnter2D(Collision2D coll)
    {

		if (!sliding) {
			

			if (coll.gameObject.GetComponent<Gear> () != null) {
				int a = coll.gameObject.GetComponent<Gear> ().type;
				if (a == wantGear && canMove) {
					wantGear = Random.Range (1, 4);
					Score += 1;
					source.PlayOneShot (goodGear);
					Destroy (coll.gameObject);

				} else if (a == 0 && canMove) {
					if (invinciTimer > 0.8f) {
						source.PlayOneShot (badGear);
						health -= 20;
						Destroy (coll.gameObject);
					}

				} else if (canMove) {
					if (invinciTimer > 0.8f) {
						source.PlayOneShot (badGear);
						health -= 10;
						Destroy (coll.gameObject);
					}
				}
			}

			if (coll.transform.tag == "Ground") {
				animator.SetInteger ("State", 0);
				onGround = true;

			}
		} else {
			if (coll.gameObject.GetComponent<Gear> () != null) {
				Physics2D.IgnoreCollision (coll.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
				coll.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, -400));
				rb.gravityScale = 0;

			}
		}
    }



    public void changeState(int state_)
    {
		animator.SetTrigger ("Attack");
    }
}
