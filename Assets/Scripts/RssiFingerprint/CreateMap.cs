using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour {

    public JsonFileWriter jsonFileWriter;
    public PositionalTracker positionalTracker;
    public WifiSignal wifiSignal;

    public GameObject nodePrefab;

    private bool isMapping = false;
    private Vector3 lastPos = Vector3.zero;
    private int numNodes = 0;

    // Update is called once per frame
    void Update() {
        if (isMapping) {
            if (Vector3.Distance(positionalTracker.GetPosition(), lastPos) > .3f) {
                Collider[] hitColliders = Physics.OverlapSphere(positionalTracker.GetPosition(), .4f);
                if (hitColliders.Length == 0) {
                    //record new node
                    CreateNode();
                    lastPos = positionalTracker.GetPosition();
                }
            }
        }
    }

    public void StartMapping() {
        isMapping = true;
    }

    public void SaveMap() {
        isMapping = false;
        jsonFileWriter.SaveMap();
    }

    void CreateNode() {
        numNodes++;
        Debug.Log("Node: " + numNodes);
        //get node info
        Vector3 pos = positionalTracker.GetPosition();
        int rssi = wifiSignal.GetCurrSignal();
        string mac = wifiSignal.GetMacAddress();
        //add node info to json string
        jsonFileWriter.AddNode(mac, rssi, pos);
        //instantiate node
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.transform.position -= new Vector3(0, .1f, 0);
        string nodeText = "Mac: " + mac + "\n" + "RSSI: " + rssi + "dB";
        node.GetComponent<NodeBehavior>().Init(nodeText, numNodes);
    }
}
