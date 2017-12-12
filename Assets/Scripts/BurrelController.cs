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

		BurrelControl ();

	}


	//======================以下関数集=========================

	void BurrelControl()
	{
		position = Input.mousePosition;//マウスの位置座標を変数positionに格納！
		position.z = 15;// Z軸修正。＊＊マウスの位置座標からの相対位置であることに注意！＊＊

		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);// positionをスクリーン座標→ワールド座標に変換！
		this.gameObject.transform.up = (screenToWorldPointPosition - transform.position).normalized;
	}



}
