﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_parent : MonoBehaviour {
	float moveSpeed = 0.275f;
	public GameObject Gun;
	float xBorderLeft = 5.0f;
	float xBorderRight = 8.2f;
	float yBorderUp = 3.0f;
	float yBorderDown = -4.3f;
	// Movable for virtual pad 
	void AnimationChange(bool isRun){
		if (isRun) {
			GetComponent<Animator> ().SetTrigger ("Run");
		} else {
			GetComponent<Animator> ().SetTrigger ("Idle");
		}
	}
	protected void Movable(Vector2 deltaVec){
		AnimationChange(deltaVec.magnitude >= 0.1f);
		float xPos = deltaVec.x/2.0f;
		float yPos = deltaVec.y/2.0f;
		float xPosCur = transform.localPosition.x;
		float yPosCur = transform.localPosition.y;
		float newxPosCur = xPosCur + xPos;
		float newyPosCur = yPosCur + yPos;
		/*
		if (newxPosCur > xBorderLeft &&
			newxPosCur < xBorderRight) {
			xPosCur = newxPosCur;
		}
		*/
		if (newyPosCur < yBorderUp &&
			newyPosCur > yBorderDown) {
			yPosCur = newyPosCur;
		}
		transform.localPosition = new Vector2 (xPosCur, yPosCur);
		if (transform.localPosition.y > yBorderUp) {
			transform.localPosition = new Vector2 (xPosCur, yBorderUp);
		}
		if (transform.localPosition.y < yBorderDown) {
			transform.localPosition = new Vector2 (xPosCur, yBorderDown);
		}
	}


	// Movable for keyboard
	protected void Movable(bool left,bool right,bool up,bool down){
		AnimationChange (left || right || up || down);
		Vector2 dogPos = transform.localPosition; 
		if (left&& dogPos.x > xBorderLeft) {
			//transform.localPosition = new Vector2 (dogPos.x - moveSpeed, dogPos.y);
		}
		if (right && dogPos.x < xBorderRight) {
			//transform.localPosition = new Vector2 (dogPos.x + moveSpeed, dogPos.y)
		}
		if (up && dogPos.y < yBorderUp) {
			transform.localPosition = new Vector2 (dogPos.x, dogPos.y+ moveSpeed);
		}
		if (down && dogPos.y > yBorderDown) {
			transform.localPosition = new Vector2 (dogPos.x, dogPos.y- moveSpeed);
		}



	}


		
	protected void Shootable(bool pushKeyDown, bool pullKeyDown, bool pullKeyUp){
		Gun.GetComponent<Gun_> ().Shootable (pushKeyDown, pullKeyDown, pullKeyUp);
		
	}



	void Start () {

		if (transform.localPosition.x < 0.0f) {
			float temp = xBorderLeft;
			xBorderLeft = xBorderRight;
			xBorderRight = temp;
			xBorderLeft *=-1.0f;
			xBorderRight *= -1.0f;
		}
	}
		


}
