using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject bulletPrefab;//添加public变量bulletPrefab
	public Transform bulletSpawn;//添加子弹local发射点
	// Use this for initialization

	public float speed = 1;  

	public float jumpSpeed = 10;  

	public float gravity = 20;  

	public float margin = 0.1f;  

	private Vector3 moveDirection = Vector3.zero;  

	[Command]//注意[Command]可以声明一个函数可以本客户端调用，但是会在服务端（主机）执行。
	void CmdFire() 
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);
		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
	void start () {
		GetComponent<Rigidbody>().freezeRotation = true;
	}
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
 		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z); 

//		if (IsGrounded()) {  
			if (Input.GetKeyDown(KeyCode.Space))
			{
			print (GetComponent<Rigidbody>());
			print ("空格键被按下了");
				GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed);
			}
//		}



		if (Input.GetMouseButtonDown(0)) {//左
			print ("发射子弹");
			CmdFire(); 
		}
		if (Input.GetMouseButtonDown(1)) {//右
			print ("1");
		}
		if (Input.GetMouseButtonDown(2)) {//中
			print ("2");
		}
	}
	// 通过射线检测主角是否落在地面或者物体上  
	bool IsGrounded() {  
		return Physics.Raycast(transform.position, -Vector3.up,  margin);  
	} 
}
