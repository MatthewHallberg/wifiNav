using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehavior : MonoBehaviour {

	public WifiController wifiController;
	public bool isColliding;

	public List<GameObject> targetColliders = new List<GameObject> ();
	private List<GameObject> currColliders = new List<GameObject> ();

	public void MakeCollide () {
		StartCoroutine (MakeCollideRoutine ());
	}

	IEnumerator MakeCollideRoutine () {
		while (!isColliding) {
			transform.localScale += Vector3.one * .1f;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale += Vector3.one * .1f;
		wifiController.Trilaterate ();
	}

	private void OnCollisionEnter (Collision collision) {
		if (targetColliders.Contains (collision.gameObject) && !currColliders.Contains(collision.gameObject)) {
			currColliders.Add (collision.gameObject);
			if (targetColliders.Count == currColliders.Count) {
				isColliding = true;
			}
		}
	}

	private void OnCollisionExit (Collision collision) {
		if (targetColliders.Contains (collision.gameObject)) {
			currColliders.Remove (collision.gameObject);
			isColliding = false;
		}
	}
}
