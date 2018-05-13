using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPull : Bullet {

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "TargetCoin" || collider.gameObject.tag == "TargetSkull") {
			//Vector2 targetpos = collider.gameObject.transform.position;

			collider.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (base.pullPower, 0f));
	

		}
		//Destroy (gameObject);
		base.OnTriggerEnter2D (collider);

	}
}
