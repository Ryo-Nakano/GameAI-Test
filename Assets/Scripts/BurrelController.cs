using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrelController : MonoBehaviour {

	// 位置座標
	Vector3 position;
	// スクリーン座標をワールド座標に変換した位置座標
	Vector3 screenToWorldPointPosition;

	[SerializeField] GameObject playerBullet;//Unity上からアタッチ！
	float playerFireTimer;//Playerが攻撃できる間隔調整

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		playerFireTimer += Time.deltaTime;
		Fire ();//弾を自動的に0.5s毎に撃つ関数

		BurrelControl ();//Burrelの向きをマウスの位置座標に同期させる関数

	}


	//======================以下関数集=========================


	//Burrelの向き調整する関数
	void BurrelControl()
	{
		position = Input.mousePosition;//マウスの位置座標を変数positionに格納！
		position.z = -Camera.main.transform.position.z;// Z軸修正。＊＊マウスの位置座標からの相対位置であることに注意！＊＊
		//毎回カメラのz座標変えるたびにここの数自分で直すの面倒だからこうした。

		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);// positionをスクリーン座標→ワールド座標に変換！
		this.gameObject.transform.up = (screenToWorldPointPosition - transform.position).normalized;
	}


	//弾(PlayerBullet)撃つ関数
	void Fire()
	{
		Vector3 spawnPosition = this.transform.position + ((screenToWorldPointPosition - transform.position).normalized);
		Quaternion spawnRotation = Quaternion.LookRotation ((screenToWorldPointPosition - transform.position).normalized);
		//今簡易的にEmpty作って強引にforwardの軸ずらしてるけど、これ他の方法無いの？(Pivotずらす時と同じ手法)
		//→この処理別にこっちでやる必要無かったね。こっちでは生成するだけで、BulletのStart()内で角度とかいじるもありだった。
		//そうすればthis.transform.up = (任意のベクトル); で終わってた。


		if(playerFireTimer > 0.5f)//0.5s毎に撃てる
		{
			Instantiate (playerBullet, spawnPosition, spawnRotation);//弾の生成
			playerFireTimer = 0;//timerの初期化
		}
	}



}
