using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class menuStart : MonoBehaviour
{

	public static menuStart MainInstance = null;

	public GameObject startMenu;
	public GameObject gameOverMenu;

	public UnityEvent StartEvent;
	public UnityEvent GameOverEvent;

	void Start()
    {		
		MainInstance = this;
		startMenu.SetActive(true);
		StartEvent.Invoke();
    }

	public void RestartGame() 
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
	}

	public void GameOverScreen() 
	{
		gameOverMenu.SetActive(true);
		GameOverEvent.Invoke();
	}
}
