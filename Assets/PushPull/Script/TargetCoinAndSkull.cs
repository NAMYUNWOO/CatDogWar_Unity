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
		bool isDog = collider.gameObject.tag == "Dog";
		bool isCat = collider.gameObject.tag == "Cat";
		bool isBorderEnd = collider.gameObject.tag == "BorderEnd";
		bool isBorderRight = collider.gameObject.name == "BorderRight";
		if (isDog || isCat || isBorderEnd) {
			
			if (!isCoin && ( isDog || isCat)) {
				return;
			}

			/*
			if(isCat || collider.gameObject.name == "BorderLeft")
				ScoreBoardtoGo = GameObject.Find ("CatScore").transform.localPosition;
			
			isDeadTarget = true;
			GetComponent<CircleCollider2D> ().enabled = false;
			*/
			mgm.ScoreCounter (isCoin, isBorderRight || isDog);
			//bool success = targetGen.makeTarget (isCoin);
			

			if (isCoin) {
				TakeDamage ("+1");
			} else {
				TakeDamage ("-1");
			}
			targetGen.makeTarget (isCoin);
			Destroy (gameObject);

             
		}

	}

	public void TakeDamage(string amount)
	{
		
		FloatingTextController.CreateFloatingText(amount,transform,gameObject.tag == "TargetCoin");
		//Debug.LogFormat("{0} was dealt {1} damage", gameObject.name, amount);

	}


	void Start () {
		mgm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
		//ScoreBoardtoGo = GameObject.Find ("DogScore").transform.localPosition;
		targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
		FloatingTextController.Initialize();
	}

	void destroy_coin_skull() {
		Destroy (gameObject);
	}
	// Update is called once per frame
	/*
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
	*/
}
