using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biker : MonoBehaviour {
	private Animator anim;
	private int speedID = Animator.StringToHash("Vertical");
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		float v = Input.GetAxisRaw ("Vertical");
		anim.SetFloat (speedID, v);
//		transform.Translate (Vector3.forward * v * Time.deltaTime * 4);
	}
}
