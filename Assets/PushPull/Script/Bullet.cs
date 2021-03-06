﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour {

	Vector2 bulletDirec;
	SpriteRenderer rend;
	float colorGadd;
	protected float pushPower;
	protected float pullPower;
	public float Power = 23.0f;
	float speed = 1.0f;

	protected float RandomSign() {
		if (Random.Range(0, 2) == 0) {
			return -50f;
		}
		return 50f;
	}

	protected void OnTriggerEnter2D(Collider2D collider){
		
		if (collider.tag != "Pull" && collider.tag != "Push" && collider.tag != "Border" && collider.tag != "Wheel"){
			Destroy (gameObject);

		}
	}



	void shoot(){
		transform.localPosition = Vector2.MoveTowards (transform.localPosition, bulletDirec, speed);
	}
	void Start () {	
		float xOpposite = GameObject.Find ("BorderLeft").transform.position	.x;
		if (transform.position.x < 0.0f) {
			xOpposite *= -1.0f;
		}
		bulletDirec = new Vector2 (xOpposite*200f, transform.localPosition.y);
		if (transform.position.x > 0.0f) {
			pushPower = -Power;
			pullPower = Power;
		} else {
			pushPower = Power;
			pullPower = -Power;
		}

	}
	
	// Update is called once per frame
	void Update () {
		shoot ();
	}


}
