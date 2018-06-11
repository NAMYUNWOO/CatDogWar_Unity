using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletPush : Bullet {

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "TargetCoin" || collider.gameObject.tag == "TargetSkull") {
			//Vector2 targetpos = collider.gameObject.transform.position;
			collider.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (base.pushPower, 0f));

		}
		base.OnTriggerEnter2D (collider);
	}
}
