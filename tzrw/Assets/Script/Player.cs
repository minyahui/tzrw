using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Animator anim;
	private int speedID = Animator.StringToHash("Speed");
	private int  horizontalID = Animator.StringToHash("Horizontal");
	private int isSpeedUpId = Animator.StringToHash ("IsSpeedUp");
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat (speedID, Input.GetAxis ("Vertical") * 4.1f);
//		anim.SetFloat (horizontalID, Input.GetAxis ("Horizontal"));
//		if (Input.GetKeyDown (KeyCode.LeftShift)) {
//			anim.SetBool (isSpeedUpId, true);
//		}
//		if (Input.GetKeyUp (KeyCode.LeftShift)) {
//			anim.SetBool (isSpeedUpId, false);
//		}
	}
}
