using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionalTracker : MonoBehaviour {

	public Transform player;

	private Vector2 playerPos = Vector2.zero;

	public Vector2 GetPosition () {
        playerPos.x = player.position.x;
        playerPos.y = player.position.z;
        return playerPos;
	}
}
