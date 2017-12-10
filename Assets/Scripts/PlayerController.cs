using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed;//移動速度

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	//Playerの移動に関する関数
	void Move()
	{
		if(Input.GetKey("right"))
		{
			this.transform.position += new Vector3 (-moveSpeed * Time.deltaTime, 0, 0);
		}

		if(Input.GetKey("left"))
		{
			this.transform.position += new Vector3 (moveSpeed * Time.deltaTime, 0, 0);
		}

		if(Input.GetKey("up"))
		{
			this.transform.position += new Vector3 (0, moveSpeed * Time.deltaTime, 0);
		}

		if(Input.GetKey("down"))
		{
			this.transform.position += new Vector3 (0, -moveSpeed * Time.deltaTime, 0);
		}
	}
}
