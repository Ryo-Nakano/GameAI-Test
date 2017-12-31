using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	GameObject player;

//	[SerializeField] float speed;//Enemyの移動速度
//	[SerializeField] float rotationSmooth;//playerの方向く時の滑らかさ(速さ？)を調節

	float timer;

	//追跡行動に使う関数
	NavMeshAgent agent;//NavMeshAgent型の変数agentを定義
	[SerializeField] float chaseInterval;//敵の位置座標更新の頻度司る変数→小さければ小さいほど頻繁に目標地点の更新を行う！
	[SerializeField] float levelSize;//取得する徘徊目的地の自分の位置座標からの範囲を司る→小さければ小さいほど小刻みに移動！

	//徘徊行動に使う変数
	Vector3 loiteringPosition;//徘徊行動時の目的地
	float loiteringInterval;//徘徊時の目的地更新頻度を司る変数

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");//HierarcyからPlayerを見つけてきて変数playerの中に格納
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();//NavMeshAgentをGetComponent！
		loiteringInterval = Random.Range(0, 5f);
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		//後はいつどのタイミングでChaseを呼ぶのかをコントロールすればいいだけかな！
//		Chase ();//追跡行動
		Loitering();//徘徊行動
	}




	//==========徘徊行動の関数==========
	void Loitering()
	{
		if(timer > loiteringInterval)//一定時間毎に徘徊目的地の位置座標更新！
		{
			//先ずは徘徊目的地取得
			loiteringPosition = new Vector3 //自分の位置座標から一定範囲だけ離れた位置をランダムに取得→変数loiteringPositionに格納
				(Random.Range(this.transform.position.x - levelSize, this.transform.position.x + levelSize),
				this.transform.position.y, 
				Random.Range(this.transform.position.z - levelSize, this.transform.position.z + levelSize));
			agent.destination = loiteringPosition;//Enemyが向かう徘徊目的地を設定！
			
			if (agent.hasPath == true) { //目的地設定先がNavMesh範囲内だった場合→目的地に設定
				loiteringInterval = Random.Range (0, 7f);//徘徊目的地の更新を初期化
				timer = 0;//timerの初期化
				Debug.Log("Destination Completed");
			} 
			else
			{
				Debug.Log ("Destination Failed");
			}
		}
	}


	//==========追跡行動の関数==========
	void Chase()
	{
		//===NavMesh使うスタイル===
		if(timer > chaseInterval)//一定時間毎に敵の位置座標更新！
		{
			agent.destination = player.transform.position;//Enemyが向かう目的地を設定！
			timer = 0;//timer初期化
		}



		//===自力でコード書くスタイル===
		//敵の方向を向く処理①
		//実装はできているけど、きれいな、思い描いてるような動きではまだないかなという感じ。
		//重力働かせちゃうのが手っ取り早いかな...
//		Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);//自分から見たplayerの方向ベクトルを出して、その方向のrotationを取得
//		this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);//自分の回転状態を段階的にtargetROtationに変化させていく

		//敵の方向を向く処理②
		//この実装でもできなくはないけど、やっぱちょい動ききもいなあ...
//		if(timer > 0.1f)
//		{
//		this.transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
//			timer = 0;
//		}

		//y軸正方向に進む
//		this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	//==========攻撃行動の関数==========
	void Attack()
	{
		
	}
}
