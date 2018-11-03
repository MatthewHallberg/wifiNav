using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerprintController : MonoBehaviour {

    public JsonFileWriter jsonFileWriter;
    public PositionalTracker positionalTracker;
    public WifiSignal wifiSignal;

    private bool isMapping = false;
    private Vector3 lastPos = Vector3.zero;


    // Update is called once per frame
    void Update() {
        if (isMapping) {
            if (Vector3.Distance(positionalTracker.GetPosition(), lastPos) > 1) {
                //record new node
                CreateNode();
                lastPos = positionalTracker.GetPosition();
            }
        }
    }

    public void ReadMap() {
        isMapping = false;
        jsonFileWriter.LoadMap();
    }

    public void CreateMap() {
        CreateNode();
        isMapping = true;
    }

    public void SaveMap() {
        isMapping = false;
        jsonFileWriter.SaveMap();
    }

    void CreateNode() {
        Debug.Log("Recording new Node!");
        jsonFileWriter.AddNode(wifiSignal.GetMacAddress(), wifiSignal.GetCurrSignal(), positionalTracker.GetPosition());
    }
}
