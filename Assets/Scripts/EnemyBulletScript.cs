using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour {

	Rigidbody rb;//Rigidbodyを格納しておく為の変数
	[SerializeField] float speed;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();//Rigidbodyコンポーネント取得！
		rb.velocity = this.transform.forward * speed;
		Destroy (this.gameObject, 10);//10s後自動消去
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
