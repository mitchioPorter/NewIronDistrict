using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGB : MonoBehaviour {
<<<<<<< HEAD
	Animator animator;
	int state;
	public float shown;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
=======
	SpriteRenderer sprtrndr;
	Animator animator;
	int state;
	public float shown;
	float rotationFactor;
	float opacity;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		sprtrndr = GetComponent<SpriteRenderer> ();
>>>>>>> MitchiEdit
	}
	
	// Update is called once per frame
	void Update () {
		
<<<<<<< HEAD
		if (state != 0) {
			transform.position = new Vector3 (0, 1, -9);
			if (Time.time > shown + 2) {
				changeState (0);
			}
		} else {

			transform.position = new Vector3 (0, 1, 15);
=======
		if (state != 0 && Time.time > shown + .1) {
			transform.position = new Vector3 (0, 3, -9);
			rotationFactor += 1;
			if (Time.time > shown + .5 && Time.time < shown + 1) {
				transform.Rotate(0, 0, Time.deltaTime*730);
				sprtrndr.color = new Color(1,1,1,.2f*rotationFactor);
			}
			if (Time.time > shown + 2.5) {
				changeState (0);
			}

		} else {

			transform.position = new Vector3 (0, 3, 15);
>>>>>>> MitchiEdit
		}
		
	}

	public void changeState(int state_){
		animator.SetInteger ("State", state_);
			state = state_;
			shown = Time.time;
<<<<<<< HEAD
=======
			sprtrndr.color = new Color(1,1,1,0);
			rotationFactor = 0;
			
>>>>>>> MitchiEdit
	}
}
