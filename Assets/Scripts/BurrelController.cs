using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrelController : MonoBehaviour {

	// 位置座標
	private Vector3 position;
	// スクリーン座標をワールド座標に変換した位置座標
	private Vector3 screenToWorldPointPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		position = Input.mousePosition;//マウスの位置座標を変数positionに格納！
		position.z = 0;// Z軸修正
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);// positionをスクリーン座標→ワールド座標に変換！
		// ワールド座標に変換されたマウス座標を代入
		gameObject.transform.position = screenToWorldPointPosition;
	}
}
