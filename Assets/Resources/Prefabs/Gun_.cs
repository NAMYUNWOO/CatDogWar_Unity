using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun_ : MonoBehaviour {

	public GameObject Push;
	public GameObject Pull;
	GameObject bullet;	bool PushOrPull = false;

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	void shootBullet(){
		//Debug.Log ("Shoooot");
		Vector2 bulletforce= new Vector2(transform.position.x * -50f, transform.position.y);
		bullet.GetComponent<Rigidbody2D> ().AddForce (bulletforce);

	}

	public void Shootable(bool pushKeyDown, bool pullKeyDown, bool pullKeyUp){
		
		if (pushKeyDown) {
			PushOrPull = false;
			initBullet ();
		}
		if (pullKeyDown) {
			PushOrPull = true;
			initBullet ();
			transform.GetComponent<SpriteRenderer> ().enabled = true;
		}
		if (pullKeyUp) {
			transform.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}


	void initBullet(){
		
		if (!PushOrPull) {
			bullet = (GameObject)Instantiate (Push, transform.position, Quaternion.identity);

		}else{
			bullet = (GameObject)Instantiate (Pull, transform.position, Quaternion.identity);

		}
		shootBullet ();


	}
}
