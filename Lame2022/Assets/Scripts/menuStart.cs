using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuStart : MonoBehaviour
{
	public GameObject startMenu;
	public GameObject gameOverMenu;

	// Start is called before the first frame update
	void Start()
    {		
		startMenu.SetActive(true);
    }

	public void RestartGame() 
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
	}

	public void GameOverScreen() 
	{
		gameOverMenu.SetActive(true);
	}
}
