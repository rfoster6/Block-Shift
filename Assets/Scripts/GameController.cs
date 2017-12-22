using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public Text titleText, scoreText, turnsText, directionsText, actionButtonText;
	public GameObject nonGamePlayScreenObject, gamePlayScreenObject;
	public bool planningPhaseActivated, actionPhaseActivated, resolutionPhaseActivated, startScreenActivated, SummaryScreenActivated;
	public int score, turnsLeft, maxTurns, loot;
	public GameObject cubePreFab;
	public GameObject[,] cubeGrid;
	public Vector3[,] gridPositions;
	public Color [] colors = new Color[6];
	public Button playButton;
	string blockShiftText = "Block Shift";
	string startText = "Start";
	string victoryText = "Victory!";
	string playAgainText = "Play Again";
	public int playGridX, playGridY, cubeGridX, cubeGridY, maxCubeSpawnX, maxCubeSpawnY;
	int pusherCube1SpawnX, pusherCube1SpawnY, pusherCube2SpawnX, pusherCube2SpawnY;
	float actionTimerLength;
	//Vector3 [] textAndButtonPositions;


	// Use this for initialization
	void Start () {
		startScreenActivated = true;
		gamePlayScreenObject.SetActive (false);
		actionTimerLength = 4f;
		score = 0;
		maxTurns = 15;
		turnsLeft = maxTurns;
		maxCubeSpawnX = 8;
		maxCubeSpawnY = 5;
		cubeGridX = maxCubeSpawnX + 2;
		cubeGridY = maxCubeSpawnY + 2;
		playGridX = cubeGridX;
		playGridY = cubeGridY;
		pusherCube1SpawnX = 0;
		pusherCube1SpawnY = 1;
		pusherCube2SpawnX = 1;
		pusherCube2SpawnY = 0;
		cubeGrid = new GameObject[cubeGridX, cubeGridY];
		gridPositions = new Vector3[playGridX, playGridY];
		colors [0] = Color.black;
		colors [1] = Color.blue;
		colors [2] = Color.green;
		colors [3] = Color.red;
		colors [4] = Color.yellow;
		colors [5] = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		if (startScreenActivated) {
			DisplayNonGameplayScreen (blockShiftText, startText);
			startScreenActivated = false;
		}
		if (planningPhaseActivated) {
			SetUpPlanningPhase ();
			planningPhaseActivated = false;
		}
		if (actionPhaseActivated) {
			RunActionTimer ();
		}

		if (SummaryScreenActivated) {
			DisplayNonGameplayScreen (victoryText, playAgainText);
			SummaryScreenActivated = false;
		}
	}

	public void ProcessButtonClick () {
		nonGamePlayScreenObject.SetActive (false);
		gamePlayScreenObject.SetActive (true);
		planningPhaseActivated = true;
	}

	void DisplayNonGameplayScreen (string myTitleText, string myButtonText) {
		nonGamePlayScreenObject.SetActive (true);
		titleText.text = myTitleText;
		playButton.GetComponentInChildren <Text> ().text = myButtonText;

	}



	void SetUpPlanningPhase () {
		//First time, set up grid
		if (turnsLeft == maxTurns) {
			CreateGrids ();
		}
		SpawnPusherCubes ();
		directionsText.text = ("Press the Action! button when ready.");
		actionButtonText.text = ("Action!");
		turnsText.text = ("Turns:" + turnsLeft);
		scoreText.text = ("Score:" + score);
	}

	void CreateGrids () {
		print ("setting up grid");
		for (int y = 0; y < playGridY; y++) {
			for (int x = 0; x < playGridX; x++) {
				gridPositions [x, y] = new Vector3 (x*2-15, y*2-8);
				if (x > 0 && x <= maxCubeSpawnX && y > 0 && y <= maxCubeSpawnY) {
					cubeGrid [x, y] = Instantiate (cubePreFab, gridPositions [x, y], Quaternion.identity);
					cubeGrid [x, y].GetComponent<CubeController> ().myColorIndex = Random.Range (0, colors.Length);
					cubeGrid [x, y].GetComponent<Renderer> ().material.color = colors [cubeGrid [x, y].GetComponent<CubeController> ().myColorIndex];
					cubeGrid [x, y].GetComponent<CubeController> ().myGridX = x;
					cubeGrid [x, y].GetComponent<CubeController> ().myGridY = y;

				}

			}
		}

	}

	void SpawnPusherCubes () {
		cubeGrid [pusherCube1SpawnX, pusherCube1SpawnY] = Instantiate (cubePreFab, gridPositions [pusherCube1SpawnX, pusherCube1SpawnY], Quaternion.identity);
		cubeGrid [pusherCube1SpawnX, pusherCube1SpawnY].GetComponent<Renderer> ().material.color = colors [Random.Range (0, colors.Length)];
		cubeGrid [pusherCube1SpawnX, pusherCube1SpawnY].GetComponent<CubeController> ().myGridX = pusherCube1SpawnX;
		cubeGrid [pusherCube1SpawnX, pusherCube1SpawnY].GetComponent<CubeController> ().myGridY = pusherCube1SpawnY;
		cubeGrid [pusherCube1SpawnX, pusherCube1SpawnY].GetComponent<CubeController> ().pusherCube1 = true;

		cubeGrid [pusherCube2SpawnX, pusherCube2SpawnY] = Instantiate (cubePreFab, gridPositions [pusherCube2SpawnX, pusherCube2SpawnY], Quaternion.identity);
		cubeGrid [pusherCube2SpawnX, pusherCube2SpawnY].GetComponent<Renderer> ().material.color = colors [Random.Range (0, colors.Length)];
		cubeGrid [pusherCube2SpawnX, pusherCube2SpawnY].GetComponent<CubeController> ().myGridX = pusherCube2SpawnX;
		cubeGrid [pusherCube2SpawnX, pusherCube2SpawnY].GetComponent<CubeController> ().myGridY = pusherCube2SpawnY;
		cubeGrid [pusherCube2SpawnX, pusherCube2SpawnY].GetComponent<CubeController> ().pusherCube2 = true;
	}
		

	public void StartActionPhase () {
		actionPhaseActivated = true;
		directionsText.text = ("You may click cubes to destroy them.");
	}

	void RunActionTimer () {
		actionTimerLength -= Time.deltaTime;
		actionButtonText.text = actionTimerLength.ToString ("00");

		if (actionTimerLength <= 0f) {
			actionPhaseActivated = false;
			RunResolutionPhase ();
		}

	}

	void RunResolutionPhase () {
		resolutionPhaseActivated = true;
		actionButtonText.text = (" ");
		directionsText.text = (" ");
		//scoring
		//cubes fall
		//new cubes
		//repeat if needed
	}
		
}
