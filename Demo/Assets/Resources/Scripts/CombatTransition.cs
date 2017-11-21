using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CombatTransition : MonoBehaviour {
	public int index;
	public string levelName;

	public GameObject panel1;
	public GameObject panel2;
	public Animator anim;

	// Use this for initialization
	void Start () {
		index = SceneManager.GetActiveScene ().buildIndex;
		StartCoroutine (Sliding());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Sliding() {
		anim.SetBool ("SlideUp", true);
		anim.SetBool ("SlideDown", true);
		yield return new WaitForSeconds(1f);
	}
}
