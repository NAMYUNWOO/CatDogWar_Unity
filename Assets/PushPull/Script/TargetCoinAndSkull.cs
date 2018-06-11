using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetCoinAndSkull : MonoBehaviour {

	public bool isDeadTarget = false;
	public Vector2 ScoreBoardtoGo;
	TargetGen targetGen;
	MyGameManager mgm;
	void OnCollisionEnter2D(Collision2D collider){
		bool isCoin = gameObject.tag == "TargetCoin";
		bool isSkul = gameObject.tag == "TargetSkull";
		bool isPlayer = collider.gameObject.tag == "Player";
		bool isEnemy = collider.gameObject.tag == "Enemy";
		bool isBorderEnd = collider.gameObject.tag == "BorderEnd";
		bool isBorderLeft = collider.gameObject.name == "BorderLeft";
		bool isBorderRight = collider.gameObject.name == "BorderRight";
		if (isPlayer || isEnemy || isBorderEnd) {
			
			if (isSkul && ( isPlayer || isEnemy)) {
				return;
			}


			if(isEnemy || collider.gameObject.name == "BorderLeft")
				ScoreBoardtoGo = GameObject.Find ("EnemyScore").transform.localPosition;
			isDeadTarget = true;
			GetComponent<CircleCollider2D> ().enabled = false;
			mgm.ScoreCounter (isCoin, isBorderRight || isPlayer);
			targetGen.makeTarget (isCoin);

			if (isCoin) {
				TakeDamage ("+1");
			} else {
				TakeDamage ("-1");
			}
             
		}

	}

	public void TakeDamage(string amount)
	{
		
		FloatingTextController.CreateFloatingText(amount,transform,gameObject.tag == "TargetCoin");
		//Debug.LogFormat("{0} was dealt {1} damage", gameObject.name, amount);

	}


	void Start () {
		mgm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
		ScoreBoardtoGo = GameObject.Find ("PlayerScore").transform.localPosition;
		targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
		FloatingTextController.Initialize();
	}

	void destroy_coin_skull() {
		Destroy (gameObject);
	}
	// Update is called once per frame
	void Update () {
		if (transform.position.x < -9f || transform.position.x > 9f || transform.position.y > 5f || transform.position.y < -5f)
			destroy_coin_skull();
		if (isDeadTarget) {
			if (Vector3.Distance (transform.position, ScoreBoardtoGo) > 1.0f) {
				transform.localPosition = Vector2.MoveTowards (transform.position, ScoreBoardtoGo, 1.0f);
			}else{
				destroy_coin_skull();
			}
		}

	}
}
