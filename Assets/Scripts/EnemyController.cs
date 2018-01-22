using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshを使う為に必要
using UnityEngine.UI;//Canvas使う為に必要

public class EnemyController : MonoBehaviour {

//	[SerializeField] Text testText;//Debug用のtext
//	[SerializeField] Text testText2;//Debug用のtext2
//	[SerializeField] Text testText3;//Debug用のtext3

	//Enemyの色を変える為の変数
	Renderer[] renderers;//Rendererを格納しておく為の変数

	float testTimer;

	GameObject player;
	NavMeshAgent agent;//NavMeshAgent型の変数agentを定義


	//===追跡行動・攻撃行動に使う変数===
	[SerializeField] float chaseInterval;//敵の位置座標更新の頻度司る変数→小さければ小さいほど頻繁に目標地点の更新を行う！
	Vector3 targetVctor;//Playerがいる方向ベクトルを格納
	float chaseTimer;//追跡行動をコントロールするtimer
	float angle;//自分の正面方向のベクトルとplayerへの方向ベクトルの角度を格納
	float distance;//PlayerとEnemyの距離格納
	[SerializeField] float chaseSpeed;//追跡状態時のEnemyの移動速度
	[SerializeField] float maxChaseDistance;//Chaseステートに移行する最大距離を定義
	[SerializeField] float maxChaseAngle;//追跡ステートに移る最大角度を定義
	[SerializeField] float maxAttackAngle;//攻撃ステートに移る最大角度を定義

	//===徘徊行動に使う変数===
	[SerializeField] float levelSize;//取得する徘徊目的地の自分の位置座標からの範囲を司る→小さければ小さいほど小刻みに移動！
	[SerializeField] float loiteringSpeed;//徘徊状態時の移動速度
	Vector3 loiteringPosition;//徘徊行動時の目的地
	float loiteringInterval;//徘徊時の目的地更新頻度を司る変数
	float loiteringTimer;//徘徊行動をコントロールするtimer

	//==攻撃行動に使う変数==
	float attackTimer;
	[SerializeField] float attackInterval;//Enemyの攻撃頻度司る変数
	[SerializeField] GameObject enemyBullet;//EnemyBulletを格納



	// Use this for initialization
	void Start () {
		renderers = gameObject.GetComponentsInChildren<Renderer> ();//子要素のRenderer全てをGetCompomnent→renderers格納

		player = GameObject.Find ("Player");//HierarcyからPlayerを見つけてきて変数playerの中に格納
		agent = GetComponent<NavMeshAgent>();//NavMeshAgentをGetComponent！
		loiteringInterval = Random.Range(0, 5f);//徘徊行動の間隔をランダムにリセット
	}


	
	// Update is called once per frame
	void Update () {

//		testText.text = "AttackTimer : " + attackTimer.ToString();
//		testText2.text = "Distance : " + distance.ToString ();
//		testText3.text = "Angle : " + angle.ToString();

		//timer関係
		chaseTimer += Time.deltaTime;
		testTimer += Time.deltaTime;
		attackTimer += Time.deltaTime;


		//角度差求める
		targetVctor = player.transform.position - this.transform.position;//Enemy→Playerの方向ベクトルを出す
		angle = Vector3.Angle (this.transform.forward, targetVctor);//自分の正面方向のベクトルとplayerへの方向ベクトルの角度を出す(値は絶対値で返ってくる)

		//Playerとの距離を算出
		distance = (player.transform.position - this.transform.position).magnitude;//magnitudeを使って2点間距離を計算
//		maxDistance = Vector3.Distance(player.transform.position, this.transform.position);//Vector3.Distanceを使って2点間距離を計算
		//どっちの書き方でも可


		if (distance < maxChaseDistance) {//追跡距離の判定

			//距離条件は満たしているが、角度条件を満たしていない場合→徘徊
			Loitering ();//徘徊！

			if (angle < maxChaseAngle) {//追跡角度の判定
				Chase ();//追跡！
//				Debug.Log ("Chase!");

				if (angle < maxAttackAngle) {
					//攻撃行動のコード
					Attack ();//攻撃！
					Debug.Log ("Attack!!!");
				}
			}
		} 
		else //playerとの距離が追跡範囲でない時
		{
			Loitering ();//徘徊！
		}









		if(testTimer > 1.0f)//1秒毎に呼ぶ
		{
			//testしたい時にてきとーにここにぶち込む


			testTimer = 0;//testTimerの初期化
		}

	}




	//==========徘徊行動の関数==========
	void Loitering()
	{
		ChangeColorByName (Color.blue);//青に色変更
		agent.speed = loiteringSpeed;

		loiteringTimer += Time.deltaTime;//徘徊行動をコントロールするtimer

		if(loiteringTimer > loiteringInterval)//一定時間毎に徘徊目的地の位置座標更新！
		{
			//先ずは徘徊目的地取得
			loiteringPosition = new Vector3 //自分の位置座標から一定範囲だけ離れた位置をランダムに取得→変数loiteringPositionに格納
				(Random.Range(this.transform.position.x - levelSize, this.transform.position.x + levelSize),
				this.transform.position.y, 
				Random.Range(this.transform.position.z - levelSize, this.transform.position.z + levelSize));
			agent.destination = loiteringPosition;//Enemyが向かう徘徊目的地を設定！
			
			if (agent.hasPath == true) { //目的地設定先がNavMesh範囲内だった場合→目的地に設定
				loiteringInterval = Random.Range (0, 7f);//徘徊目的地の更新を初期化
				loiteringTimer = 0;//timerの初期化
//				Debug.Log("Destination Completed");//目的地設定完了
			} 
			else
			{
//				Debug.Log ("Destination Failed");//目的地設定失敗→再度徘徊目的地取得へ
			}
		}
	}


	//==========追跡行動の関数==========
	void Chase()
	{
		agent.speed = chaseSpeed;
		ChangeColorByName (Color.red);//赤に色変更

		//===NavMesh使うスタイル===
		if(chaseTimer > chaseInterval)//一定時間毎に敵の位置座標更新！
		{
			agent.destination = player.transform.position;//Enemyが向かう目的地を設定！
			chaseTimer = 0;//timer初期化
		}
			
		//===自力でコード書くスタイル===
		//敵の方向を向く処理①
		//実装はできているけど、きれいな、思い描いてるような動きではまだないかなという感じ。
		//重力働かせちゃうのが手っ取り早いかな...
//		Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);//自分から見たplayerの方向ベクトルを出して、その方向のrotationを取得
//		this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);//自分の回転状態を段階的にtargetROtationに変化させていく

		//敵の方向を向く処理②
		//この実装でもできなくはないけど、やっぱちょい動ききもいなあ...
//		if(chaseTimer > 0.1f)
//		{
//		this.transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
//			chaseTimer = 0;
//		}

		//y軸正方向に進む
//		this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	//==========攻撃行動の関数==========
	void Attack()
	{
		if(attackTimer > attackInterval)
		{
			Instantiate (enemyBullet, this.transform.position, Quaternion.LookRotation(this.transform.forward));//弾の生成
			attackTimer = 0;//attackTimerのリセット
		}
	}

	//===========Colorを変える関数(Unity上のカラーネームで指定)==========
	void ChangeColorByName(Color colorName)
	{
		foreach(Renderer r in renderers)//renderersの中の全要素に対して処理
		{
			r.material.color = colorName;//RedにColor変更
		}


	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "PlayerBullet")
		{
			Destroy (col.gameObject);
			Destroy (this.gameObject);
		}
	}
}
