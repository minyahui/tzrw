using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] 
[AddComponentMenu("Control Script/FPS Input")] 
public class KeyboardMove : MonoBehaviour {
	public float speed = 6.0f; 
	public float gravity = 0; 
	private CharacterController _charController; //用于引用CharacterController的变量 
	// Use this for initialization 
	void Start () {
		_charController = GetComponent<CharacterController>(); //使用附加到相同对象上的其他组件 
	} 
	// Update is called once per frame 
	void Update () { 
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed; 
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed); //将对角移动的速度限制为和沿着轴移动的速度一样 
		movement.y = gravity; 
		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement); //把movement向量从本地变换为全局坐标 
		_charController.Move(movement); //告知CharacterController通过movement向量移动 
	} 
} 
