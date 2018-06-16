using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPlayer : MonoBehaviour {



	float moveSpeed = 0.15f;
	float invertMult;
	public bool invertX;
	Vector2 gunPos;
	public GameObject Push;
	public GameObject Pull;
	float xBorderLeft = 5.0f;
	float xBorderRight = 8.2f;
	float yBorderUp = 4.4f;
	float yBorderDown = -2.4f;
	int TARGETNUM = 2;
	GameObject[] coinObjs;
	GameObject[] skullObjs;
	public bool actionable = true;
	float ComputerPower = 18f; 
	int cntFrame = 5;
	float enoughYdist = 0.5f;


	void CollectState()
	{
		coinObjs = GameObject.FindGameObjectsWithTag ("TargetCoin");
		skullObjs = GameObject.FindGameObjectsWithTag ("TargetSkull");

	}

	void AgentStep()
	{	
		CollectState ();

		if (!actionable || (coinObjs.Length<1  && skullObjs.Length < 1)) {
			return;
		}




		transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;
		Vector2 computerPos = transform.position;

		GameObject coin = coinObjs [0];
		GameObject skull = skullObjs [0];
		Vector2 coinpos = coin.transform.position;
		Vector2 skullpos = skull.transform.position;
		float coinDist = Vector2.Distance(computerPos,coinpos);
		float skullDist = Vector2.Distance(computerPos,skullpos);
		//float coinDist_y = Mathf.Abs(computerPos.y - coinpos.y);
		//float skullDist_y = Mathf.Abs(computerPos.y - skullpos.y);
		if (Mathf.Abs (skullpos.x) > 5.0f) {
			Vector2 movetopos = new Vector2 (computerPos.x, skullpos.y);
			transform.localPosition = Vector2.MoveTowards (transform.position, movetopos, moveSpeed);
			if (Mathf.Abs (skullpos.y - computerPos.y) < enoughYdist ) {
				gunPos = gameObject.transform.GetChild (0).position;
				GameObject newBullet = Instantiate (Push, gunPos, Quaternion.identity);
				newBullet.GetComponent<Bullet> ().Power = ComputerPower;
			}
		} else {
			
			if (coinDist < skullDist) {
				Vector2 movetopos = new Vector2 (computerPos.x, coinpos.y);
				transform.localPosition = Vector2.MoveTowards (transform.position, movetopos, moveSpeed);
				if (Mathf.Abs (coinpos.y - computerPos.y) < enoughYdist ) {
					gunPos = gameObject.transform.GetChild (0).position;
					GameObject newBullet = Instantiate (Pull, gunPos, Quaternion.identity);
					newBullet.GetComponent<Bullet> ().Power = ComputerPower;
				}
			} else {
				Vector2 movetopos = new Vector2 (computerPos.x, skullpos.y);
				transform.localPosition = Vector2.MoveTowards (transform.position, movetopos, moveSpeed);
				if (Mathf.Abs (skullpos.y - computerPos.y) < enoughYdist ) {
					gunPos = gameObject.transform.GetChild (0).position;
					GameObject newBullet = Instantiate (Push, gunPos, Quaternion.identity);
					newBullet.GetComponent<Bullet> ().Power = ComputerPower;
				}

			}
		}
		return;

	}

	void Start () {	
		if (invertX) {
			invertMult = -1f;	
			//float y1 = Random.Range (-2.2f, 4.4f);
			//gameObject.transform.localPosition = new Vector2 (-8.0f, 0f);
		} else {
			invertMult = 1f;
			//float y2 = Random.Range (-2.2f, 4.4f);
			//gameObject.transform.localPosition = new Vector2(8.0f, 0f);
		}
		CollectState ();
		GetComponent<Animator> ().SetTrigger ("Run");
	}

	// Update is called once per frame
	void Update () {
		if (!actionable)
			return;
		if (cntFrame == 5) {
			CollectState ();		
			cntFrame = 0;
		}
		AgentStep ();


		cntFrame ++;
	}



}
