using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public thoughtBubble thought;
	public bool canMove;
	public Animator animator;
	private int state;
	private Vector3 position;
	float initiated;
	bool go;


	// Use this for initialization
	void Start () {
		
		animator = GetComponent<Animator> ();
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (go && Time.time >  initiated+1) {
			animator.SetTrigger ("Attack");
			go = false;
		}
	
	}
		

	public void changeState(int state_){
		animator.SetInteger ("State", state_);
		state = state_;

		//remove this part once we fix the sprites(the current grave is too high)
		go = true;
		initiated = Time.time;

	}
}
