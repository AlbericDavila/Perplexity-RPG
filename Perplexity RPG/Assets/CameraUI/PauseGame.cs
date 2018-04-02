using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public Transform menuCanvas;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Pause();
		}	
	}

	public void Pause()
	{
		if (menuCanvas.gameObject.activeInHierarchy == false) 
		{
			menuCanvas.gameObject.SetActive (true);
			Time.timeScale = 0;
		} 
		else 
		{
			menuCanvas.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}

	public void Quit()
	{
		Debug.Log ("Player has quit the game.");
		Application.Quit();
	}
}
