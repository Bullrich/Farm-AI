using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {


	public bool Exists() {
		return gameObject.activeSelf;
	}

	public void Eat() {
		gameObject.SetActive(false);
	}

	public void Grow() {
		gameObject.SetActive(true);
	}

}
