﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGB : MonoBehaviour {
	Animator animator;
	int state;
	public float shown;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
<<<<<<< HEAD
		if (state != 0 && Time.time > shown + .1) {
			transform.position = new Vector3 (0, 1, -9);
			if (Time.time > shown + 2.5) {
=======
		if (state != 0) {
			transform.position = new Vector3 (0, 1, -9);
			if (Time.time > shown + 2) {
>>>>>>> gmrBranch
				changeState (0);
			}
		} else {

			transform.position = new Vector3 (0, 1, 15);
		}
		
	}

	public void changeState(int state_){
		animator.SetInteger ("State", state_);
			state = state_;
			shown = Time.time;
	}
}
