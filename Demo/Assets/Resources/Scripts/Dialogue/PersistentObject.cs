using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour {
    public bool badDecision;
    public static PersistentObject Instance;
	// Use this for initialization
	void Start () {
        badDecision = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance) {
            Destroy(this.gameObject);
        }
    } 

}
