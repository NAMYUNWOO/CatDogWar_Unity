using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

	private static FloatingText popupTextCoin;
	private static FloatingText popupTextSkul;
	private static GameObject canvas;
	static FloatingText instance;
	// Use this for initialization
	public static void Initialize () {
		canvas = GameObject.Find ("Canvas");
		if (!popupTextCoin) {
			popupTextCoin = Resources.Load<FloatingText> ("Prefabs/PopupTextParent");
		}
		if (!popupTextSkul) {
			popupTextSkul = Resources.Load<FloatingText> ("Prefabs/PopupTextParent2");
		}
	}

	public static void CreateFloatingText(string text,Transform location,bool isCoin){
		if (isCoin)
			instance = Instantiate (popupTextCoin);
		else
			instance = Instantiate (popupTextSkul);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x, location.position.y));
		instance.transform.SetParent (canvas.transform,false);
		instance.transform.position = screenPosition;
		instance.SetText (text);


			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
