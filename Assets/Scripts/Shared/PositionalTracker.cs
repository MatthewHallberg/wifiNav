using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionalTracker : MonoBehaviour {

	public Transform player;

	private Vector3 playerPos = Vector3.zero;

	void Update () {
		if (player.position != Vector3.zero) {
			playerPos = player.position;
		}
	}

	public Vector3 GetPosition () {
		return playerPos;
	}
}
