using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WifiController : MonoBehaviour {

	const float METERS_TO_FEET = 3.28f;

	public WifiSignal wifiSignal;
	public PositionalTracker tracker;

	public List<Text> textList = new List<Text> ();
	public List<Transform> sphereList = new List<Transform> ();

	public Transform marker;

	List<SignalSnapShot> signals = new List<SignalSnapShot> ();

	List<float> rssiValues = new List<float> ();

	float currSignal = 0;
	int frameCount = 0;
	int signalCount = 0;

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			LoadSnapShot ();
		}

		//print current signal
		currSignal = Mathf.Lerp (currSignal, wifiSignal.GetCurrSignal (), Time.deltaTime * 10f);
		frameCount++;
		if (frameCount % 10 == 0) {
			frameCount = 0;
			textList [0].text = "Dist: " + CalcDistance () * METERS_TO_FEET;
		}
	}

	public void Trilaterate () {
		bool shouldFindSolution = true;
		foreach (Transform sphere in sphereList) {
			//make sure spheres are colliding...
			if (!sphere.GetComponent<SphereBehavior> ().isColliding) {
				sphere.GetComponent<SphereBehavior> ().MakeCollide ();
				return;
			}
		}
		if (shouldFindSolution) {
			foreach (Transform sphere in sphereList) {

				SignalSnapShot snap = new SignalSnapShot {
					pos = sphere.position,
					radius = sphere.localScale.x / 2
				};
				signals.Add (snap);
			}

			Vector3 newPos = Trilateration.Trilaterate (
				signals [0],
				signals [1],
				signals [2]
			);
			if (newPos != Vector3.one) {
				marker.position = newPos;
				//debug positions
				textList [2].text = "ROUTER: " + marker.position.ToString ();
			} else {
				textList [2].text = "NO Solution";
			}
			signals.Clear ();
		}
	}

	///returns distance in meters
	private float CalcDistance () {
		float signalLevelInDb = Mathf.RoundToInt (currSignal);
		int freqInMHz = wifiSignal.GetFrequency ();
		float exp = (27.55f - (20f * Mathf.Log10 (freqInMHz)) + Math.Abs (signalLevelInDb)) / 20.0f;
		return Mathf.Pow (10.0f, exp);
	}

	void LoadSnapShot () {
#if !UNITY_EDITOR
			int index = signalCount % sphereList.Count;
			textList [1].text = index.ToString();
			sphereList [index].position = tracker.GetPosition ();
			sphereList [index].localScale = Vector3.one * (currSignal * 2);
			signalCount++;
#endif
		Trilaterate ();
	}
}
