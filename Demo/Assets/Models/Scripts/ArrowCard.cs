using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCard : MonoBehaviour {
	Animator animator;
	public Vector3 velocity;
	public int type;


	// Use this for initialization
	void Start () {
		
		animator = GetComponent<Animator> ();


		velocity =  new Vector3(-4.5f, 0, 0);	
		type = (int)Random.Range (0, 4);
		if (type == 0) {
			transform.position = new Vector3 (11, 2.8f, 0);
		} else if (type == 2) {
			transform.position = new Vector3 (11, 2.3f, 0);
		} else if (type == 3) {
			transform.position = new Vector3 (11, 1.8f, 0);
		} else if (type == 1) {
			transform.position = new Vector3 (11, 1.3f, 0);
		}
		animator.SetInteger("State",type);


	}
	
	// Update is called once per frame
	void Update () {
		
			transform.Translate (velocity * Time.deltaTime);
					


	
	}
}
