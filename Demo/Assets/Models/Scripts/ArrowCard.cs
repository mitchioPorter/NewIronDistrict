using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCard : MonoBehaviour {
	Animator animator;
	Vector3 velocity;
	public int type;

	// Use this for initialization
	void Start () {
		
		animator = GetComponent<Animator> ();

		transform.position = new Vector3(12,2,0);
		transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		velocity =  new Vector3(-5f, 0, 0);	
		type = (int)Random.Range (0, 4);
		animator.SetInteger("State",type);

	}
	
	// Update is called once per frame
	void Update () {
		

		transform.Translate(velocity * Time.deltaTime);
					


	
	}
}
