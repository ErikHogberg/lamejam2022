using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		Time.timeScale = 0;
	}

	public void Unpause()
	{
		Time.timeScale = 1;
	}

	public void Pause() {
		Time.timeScale = 0;
	}

}
