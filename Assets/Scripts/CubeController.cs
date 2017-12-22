using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour {
	public int myGridX, myGridY;
	public static int pusher1X, pusher1Y, pusher2X, pusher2Y;
	public bool pusherCube1, pusherCube2;
	public bool moveAllowed, moveUp, moveDown, moveRight, moveLeft;
	GameController myGameController;
	int myStartGridX, myStartGridY; 
	public int myColorIndex;
	float pushTime, lootTime;


	// Use this for initialization
	void Start () {
		myGameController = GameObject.Find ("GameControllerObject").GetComponent<GameController> ();
		pushTime = 1.5f;
		if (pusherCube1 == false && pusherCube2 == false) {
			CheckSpawnColor ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		MovePusherCubes ();

		if (pusherCube1) {
			pusher1X = myGridX;
			pusher1Y = myGridY;
		}

		if (pusherCube2) {
			pusher2X = myGridX;
			pusher2Y = myGridY;
		}

		if (myGameController.resolutionPhaseActivated) {
			StartCoroutine (ProcessResolutionPhase ());
		}

		if (moveAllowed) {
			PushCubes ();
		}
			
	}

	void CheckSpawnColor () {
		if (myGameController.cubeGrid [myGridX - 1, myGridY] != null && myGameController.cubeGrid [myGridX + 1, myGridY] != null) {
			if (myColorIndex == myGameController.cubeGrid [myGridX - 1, myGridY].GetComponent <CubeController> ().myColorIndex
				&& myColorIndex == myGameController.cubeGrid [myGridX + 1, myGridY].GetComponent <CubeController> ().myColorIndex) {
				myColorIndex = Random.Range (0, myGameController.colors.Length);
			}
		}
		if (myGameController.cubeGrid [myGridX, myGridY - 1] != null && myGameController.cubeGrid [myGridX, myGridY + 1] != null) {	
			if (myColorIndex == myGameController.cubeGrid [myGridX, myGridY - 1].GetComponent <CubeController> ().myColorIndex
			    && myColorIndex == myGameController.cubeGrid [myGridX, myGridY + 1].GetComponent <CubeController> ().myColorIndex) {
				myColorIndex = Random.Range (0, myGameController.colors.Length);
			}
		}

		gameObject.GetComponent <Renderer> ().material.color = myGameController.colors [myColorIndex];
	}

	void OnMouseDown () {
		print (myGridX + ".." + myGridY);
		if (myGameController.actionPhaseActivated) {
			myGameController.cubeGrid [myGridX, myGridY] = null;
			Destroy (gameObject);

		}

	}

	public void MoveCounterClockwise () {

		if (myGridY == 0) {
			if (myGridX < myGameController.maxCubeSpawnX) {
				myGridX++;
			} else {
				myGridX++;
				myGridY++;
			}
		}
		else if (myGridY == myGameController.playGridY - 1) {
			if (myGridX > 1) {
				myGridX--;
			} else {
				myGridX--;
				myGridY--;
			}
		}
		else if (myGridX == 0) {
			if (myGridY > 1) {
				myGridY--;
			} else {
				myGridX++;
				myGridY--;
			}
		}
		else if (myGridX == myGameController.playGridX - 1) {
			if (myGridY < myGameController.maxCubeSpawnY) {
				myGridY++;
			} else {
				myGridX--;
				myGridY++;
			}
		}
	}

	public void MoveClockwise () {

		if (myGridY == 0) {
			if (myGridX > 1) {
				myGridX--;
			} else {
				myGridX--;
				myGridY++;
			}
		}
		else if (myGridY == myGameController.playGridY - 1) {
			if (myGridX < myGameController.maxCubeSpawnX) {
				myGridX++;
			} else {
				myGridX++;
				myGridY--;
			}
		}
		else if (myGridX == 0) {
			if (myGridY < myGameController.maxCubeSpawnY) {
				myGridY++;
				print (myGridY);
			} else {
				myGridX++;
				myGridY++;
			}
		}
		else if (myGridX == myGameController.playGridX - 1) {
			if (myGridY > 0) {
				myGridY--;
			} else {
				myGridX--;
				myGridY--;
			}
		}
	}

	void MovePusherCubes () {
		myStartGridX = myGridX;
		myStartGridY = myGridY;

		if (pusherCube1) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveClockwise ();
				if (myGameController.cubeGrid [myGridX, myGridY] != null) {
					MoveClockwise ();
				}

			}

			else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveCounterClockwise ();
				if (myGameController.cubeGrid [myGridX, myGridY] != null) {
					MoveCounterClockwise ();
				}
			}

			pusher1X = myGridX;
			pusher1Y = myGridY;
		}

		if (pusherCube2) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				MoveClockwise ();
				if (myGameController.cubeGrid [myGridX, myGridY] != null) {
					MoveClockwise ();
				}
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				MoveCounterClockwise ();
				if (myGameController.cubeGrid [myGridX, myGridY] != null) {
					MoveCounterClockwise ();
				}
			}

			pusher2X = myGridX;
			pusher2Y = myGridY;
		}

		myGameController.cubeGrid [myStartGridX, myStartGridY] = null;
		myGameController.cubeGrid [myGridX, myGridY] = gameObject;
		gameObject.transform.position = myGameController.gridPositions [myGridX, myGridY];


	}

	IEnumerator ProcessResolutionPhase () {
		if (pusherCube1) {
			moveAllowed = true;
		}
		yield return new WaitForSeconds (pushTime);

		if (pusherCube2) {
			moveAllowed = true;
		}
		yield return new WaitForSeconds (pushTime);
		myGameController.resolutionPhaseActivated = false;
		CollectLoot ();
		yield return new WaitForSeconds (lootTime);

	
			
	}

	void ScoreCubes () {

	}

	void PushCubes () {
		myStartGridX = myGridX;
		myStartGridY = myGridY;

		if (pusherCube1 || pusherCube2) {
			if (myGridY == 0) {
				moveUp = true;
			} else if (myGridY == myGameController.playGridY - 1) {
				moveDown = true;
			} else if (myGridX == 0) {
				moveRight = true;
			} else if (myGridX == myGameController.playGridX - 1) {
				moveLeft = true;
			}
		}

		if (moveUp) {
			if (myGameController.cubeGrid [myGridX, myGridY + 1] != null) {
				myGameController.cubeGrid [myGridX, myGridY + 1].GetComponent<CubeController> ().moveUp = true;
				myGameController.cubeGrid [myGridX, myGridY + 1].GetComponent<CubeController> ().moveAllowed = true;
			}
			myGridY++;
			moveUp = false;
		} else if (moveDown) {
			if (myGameController.cubeGrid [myGridX, myGridY - 1] != null) {
				myGameController.cubeGrid [myGridX, myGridY - 1].GetComponent<CubeController> ().moveDown = true;
				myGameController.cubeGrid [myGridX, myGridY - 1].GetComponent<CubeController> ().moveAllowed = true;
			}
			myGridY--;
			moveDown = false;
		} else if (moveRight) {
			if (myGameController.cubeGrid [myGridX + 1, myGridY] != null) {
				myGameController.cubeGrid [myGridX + 1, myGridY].GetComponent<CubeController> ().moveRight = true;
				myGameController.cubeGrid [myGridX + 1, myGridY].GetComponent<CubeController> ().moveAllowed = true;
			} 
			myGridX++;
			moveRight = false;
		} else if (moveLeft) {
			if (myGameController.cubeGrid [myGridX - 1, myGridY] != null) {
				myGameController.cubeGrid [myGridX - 1, myGridY].GetComponent<CubeController> ().moveLeft = true;
				myGameController.cubeGrid [myGridX - 1, myGridY].GetComponent<CubeController> ().moveAllowed = true;
			}
			myGridX--;
			moveLeft = false;
		}


		myGameController.cubeGrid [myStartGridX, myStartGridY] = null;
		myGameController.cubeGrid [myGridX, myGridY] = gameObject;
		StartCoroutine (ResolutionCubeMovement(myGameController.gridPositions [myGridX, myGridY]));
		moveAllowed = false;
		if (pusherCube1) {
			pusherCube1 = false;
		} 
		if (pusherCube2) {
			pusherCube2 = false;
		}
	}

	IEnumerator ResolutionCubeMovement (Vector3 end){
		float elapsedTime = 0;
		Vector3 startingPos = myGameController.gridPositions [myStartGridX, myStartGridY];
		while (elapsedTime < pushTime)
			
		{
			gameObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / pushTime));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		gameObject.transform.position = end;
	}

	void CollectLoot () {
		if (myGridX == 0 || myGridX == myGameController.playGridX - 1 || myGridY == 0 || myGridY == myGameController.playGridY - 1) {
			Destroy (gameObject);
			myGameController.cubeGrid [myGridX, myGridY] = null;
			myGameController.loot++;
		}
	}
}
 