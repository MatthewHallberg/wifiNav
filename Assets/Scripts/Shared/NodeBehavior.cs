using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehavior : MonoBehaviour {

    public List<Color> glowColors = new List<Color>();
    public TextMesh textMesh;
    public Transform textParent;

    private Vector3 turnAngle = new Vector3(0, .5f, 0);

    // Use this for initialization
    public void Init (string text, int nodeNum) {
        textMesh.text = text;
        int colorIndex = nodeNum % glowColors.Count;
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", glowColors[colorIndex]);
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles += turnAngle;
        textParent.LookAt(Camera.main.transform);
	}
}
