using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using CnControls;
using System.Runtime.InteropServices;

public class MyGameManager : MonoBehaviour {
	[DllImport("__Internal")]
	private static extern void AddCurScore(int catScore,int dogScore);
	[DllImport("__Internal")]
	private static extern void AddCurScore2(int AvengersScore,int ThanosScore);
	[DllImport("__Internal")]
	private static extern void AddRecord(int win,int tie,int lose);



	public bool isEnd = false;
	public int DogScore = 0;
	public int CatScore = 0;
	float GAMETIME;
	bool isSendPost = false;
	public GameObject Wheel;
	public GameObject Dog;
	public GameObject Cat;
	string baseUrl = "";
	bool isDogGame;
	TextMesh dogScoretxt;
	TextMesh catScoretxt;
	TextMesh GameTime;
	string currenturl = "";
	string gameType = "game";
	public int GetDogScore(){
		return DogScore;
	}
	public int GetCatScore(){
		return CatScore;
	}
	void TimeEndScoreCompare(){
		if (DogScore > CatScore) {
			if (isDogGame) {
				GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
			} else {
				GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
			}
		}else if (DogScore < CatScore){
			if (isDogGame) {
				GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
			} else {
				GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
			}
		} else {
			GameObject.Find ("Tie").GetComponent<MeshRenderer> ().enabled = true;
		}
			
	}

	void GameTimeElapse(){
		GameTime.text = string.Format("{0}",GAMETIME.ToString("0"));
		GAMETIME -= Time.deltaTime;
		if (GAMETIME <= 0.1f) {
			TimeEndScoreCompare ();
			isEnd = true;
		}
	}
	public void ScoreCounter(bool isCoinTarget,bool isBorderRight){
		int dogScoreAdd = 0;
		int catScoreAdd = 0;
		if (isCoinTarget) {
			if (isBorderRight) {
				dogScoreAdd = 1;
			} else {
				catScoreAdd = 1;
			}
		} else {
			if (isBorderRight) {
				dogScoreAdd = -1;
				//catScoreAdd = 1;
			} else {
				catScoreAdd = -1;
				//dogScoreAdd = 1;
			}			
		}
		DogScore += dogScoreAdd;
		CatScore += catScoreAdd;
		dogScoretxt.text = string.Format ("{0}",DogScore);
		catScoretxt.text = string.Format ("{0}",CatScore);

		if (gameType == "InfinityWar"){
			WWWForm form = new WWWForm();
			if (isDogGame) {
				form.AddField ("AvengersScore", dogScoreAdd);
				form.AddField ("ThanosScore", catScoreAdd);
			} else {
				form.AddField ("AvengersScore", catScoreAdd);
				form.AddField ("ThanosScore", dogScoreAdd);
			}
			UnityWebRequest www = UnityWebRequest.Post (baseUrl + "scoreAdd_Avengers", form);
			www.SendWebRequest ();
			try{
				if(isDogGame){
					AddCurScore2(DogScore,CatScore);
				}else{
					AddCurScore2(CatScore,DogScore);
				}
			}catch{

			}
		}else{

			WWWForm form = new WWWForm();
			form.AddField ("DogScore",dogScoreAdd);
			form.AddField ("CatScore",catScoreAdd);
			UnityWebRequest www = UnityWebRequest.Post (baseUrl + "scoreAdd", form);
			www.SendWebRequest ();
			try{
				AddCurScore(CatScore,DogScore);
			}catch{
				
			}
			
		}
			

	}


	void Start () {
		
		int isWheelGame = Random.Range (0, 5);
		if (isWheelGame == 1) {
			Instantiate (Wheel, new Vector2(0.0f,-1.0f), Quaternion.identity);
		}

		currenturl = Application.absoluteURL; //"https://infinite-reaches-12370.herokuapp.com/game/cat";//
		var array = currenturl.Split('/');
		try{
			gameType = array [3];
		}catch{
			gameType = "InfinityWar";//"game";//
		}

		string baseU = "";
		for (int idx = 0 ; idx < Mathf.Min(3,array.Length); idx++ ) {
			baseU += array[idx] + "/";
		}
		baseUrl = baseU;
		GameTime =  GameObject.Find ("GameTime").GetComponent<TextMesh> ();
		dogScoretxt =  GameObject.Find ("DogScore").GetComponent<TextMesh> ();
		catScoretxt =  GameObject.Find ("CatScore").GetComponent<TextMesh> ();
		isDogGame = currenturl.EndsWith ("dog");

		if (isDogGame) {
			Dog.GetComponent<ComputerPlayer> ().enabled = false;
			Dog.GetComponent<Dog> ().enabled = true;
			Cat.GetComponent<ComputerPlayer> ().enabled = true;
			Cat.GetComponent<Dog> ().enabled = false;
			if (gameType == "InfinityWar") {
				Sprite thanosSprite = Resources.Load <Sprite> ("Sprites/thanos0");
				Cat.GetComponent<Animator> ().enabled = false;
				Cat.GetComponent<SpriteRenderer> ().sprite = thanosSprite;
			}

		} else {
			Cat.GetComponent<ComputerPlayer> ().enabled = false;
			Cat.GetComponent<Cat> ().enabled = true;
			Dog.GetComponent<ComputerPlayer> ().enabled = true;
			Dog.GetComponent<Dog> ().enabled = false;
			if (gameType == "InfinityWar") {
				Sprite thanosSprite = Resources.Load <Sprite> ("Sprites/thanos1");
				Dog.GetComponent<Animator> ().enabled = false;
				Dog.GetComponent<SpriteRenderer> ().sprite = thanosSprite;

			}

		}
		if (gameType == "InfinityWar") {
			Sprite stoneSprite = Resources.Load <Sprite> ("Sprites/gem0");
			Sprite bgSprite = Resources.Load <Sprite> ("Sprites/space");
			Sprite skull = Resources.Load <Sprite> ("Sprites/skull_w");
			var targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
			targetGen.coin.GetComponent<SpriteRenderer> ().sprite = stoneSprite;
			targetGen.skull.GetComponent<SpriteRenderer> ().sprite = skull;
			targetGen.isInfinityWar = true;
			GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = bgSprite;
			GameObject.Find ("UpBar").GetComponent<SpriteRenderer> ().color = new Color32 (182, 182, 182, 100);
			GameObject.Find ("Win").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);
			GameObject.Find ("Lose").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);
			GameObject.Find ("Tie").GetComponent<TextMesh> ().color = new Color32 (255, 255, 255, 255);

		} else {
			Sprite stoneSprite = Resources.Load <Sprite> ("Sprites/coin");
			Sprite bgSprite = Resources.Load <Sprite> ("Sprites/bgrimg"+Random.Range (0, 2).ToString());
			Sprite skull = Resources.Load <Sprite> ("Sprites/skull_w");
			var targetGen = GameObject.Find ("TargetGen").GetComponent<TargetGen> ();
			targetGen.coin.GetComponent<SpriteRenderer> ().sprite = stoneSprite;
			targetGen.coin.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			targetGen.skull.GetComponent<SpriteRenderer> ().sprite = skull;
			targetGen.isInfinityWar = false;
			GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = bgSprite;
			//GameObject.Find ("UpBar").GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 0);
			//GameObject.Find ("Win").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);
			//GameObject.Find ("Lose").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);
			//GameObject.Find ("Tie").GetComponent<TextMesh> ().color = new Color32 (0, 0, 0, 255);

		}

		DogScore = 0;
		CatScore = 0;
		GAMETIME = 60;
	}

	// Update is called once per frame
	void Update () {
		if (isEnd) {
			if (!isSendPost) {
				int win = 0;int tie = 0; int lose = 0;
				if (DogScore == CatScore) {tie++;} 
				else if (DogScore > CatScore)
				{
					if (isDogGame) {win++;}
					else {lose++;}
				} else {
					if (isDogGame) {lose++;}
					else {win++;}
				}
				WWWForm form2 = new WWWForm();
				form2.AddField ("win",win);
				form2.AddField ("tie",tie);
				form2.AddField ("lose",lose);
				UnityWebRequest www = UnityWebRequest.Post (baseUrl + "gameresult", form2);
				www.SendWebRequest ();
				try{
					AddRecord (win, tie, lose);
				}catch{
				}
				isSendPost = true;
			}
			GameObject.Find ("ContinueBtn").GetComponent<Image>().enabled = true;
			DogScore = 0;
			CatScore = 0;
			GameObject[] gObjs = GameObject.FindGameObjectsWithTag ("TargetCoin");
			for (int i = 0; i < gObjs.Length; i++) {
				Destroy (gObjs [i]);
			}
			GameObject[] skgObjs = GameObject.FindGameObjectsWithTag ("TargetSkull");
			for (int i = 0; i < skgObjs.Length; i++) {
				Destroy (skgObjs [i]);
			}
		} else {
			
			GameTimeElapse ();

		}
		if (isEnd && CnInputManager.GetButtonDown ("Continue")) {
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			isEnd = false;
		}

	}
}
