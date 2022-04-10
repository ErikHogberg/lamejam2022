using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInfo : MonoBehaviour
{
	public Text thisObjectsText;
	public Text otherObjectsText;

	// Start is called before the first frame update
	void Start()
    {
		thisObjectsText.text = otherObjectsText.text;
	}
}
