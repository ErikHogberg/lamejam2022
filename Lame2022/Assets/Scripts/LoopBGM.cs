using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopBGM : MonoBehaviour
{
	public AudioClip StartClip;
	public AudioClip LoopClip;

	public GameObject StressedMusicPlayer;

	void Start() {
		StartCoroutine(playSound());
	}

	IEnumerator playSound() {
		GetComponent<AudioSource>().clip = StartClip;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(StartClip.length);
		GetComponent<AudioSource>().clip = LoopClip;
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().loop = true;
	}

	public void SwapMusic() {
		StressedMusicPlayer.SetActive(true);
		gameObject.SetActive(false);
	}
}
