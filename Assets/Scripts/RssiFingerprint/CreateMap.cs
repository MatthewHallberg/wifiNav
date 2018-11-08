using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour {

    public JsonFileWriter jsonFileWriter;
    public PositionalTracker positionalTracker;
    public WifiSignal wifiSignal;

    public GameObject nodePrefab;

    private bool isMapping = false;
    private Vector2 lastPos = Vector2.zero;
    private int numNodes = 0;
    private Vector3 tempPos = Vector3.zero;

    // Update is called once per frame
    void Update() {
        if (isMapping) {
            if (Vector2.Distance(positionalTracker.GetPosition(), lastPos) > .3f) {
                tempPos.x = positionalTracker.GetPosition().x;
                tempPos.y = Camera.main.transform.position.y;
                tempPos.z = positionalTracker.GetPosition().y;
                Collider[] hitColliders = Physics.OverlapSphere(tempPos, .4f);
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
        Vector2 pos = positionalTracker.GetPosition();
        int rssi = wifiSignal.GetCurrSignal();
        string mac = wifiSignal.GetMacAddress();
        //add node info to json string
        jsonFileWriter.AddNode(mac, rssi, pos);
        //instantiate node
        Vector3 worldNodePos = new Vector3(pos.x, Camera.main.transform.position.y, pos.y);
        GameObject node = Instantiate(nodePrefab, worldNodePos, Quaternion.identity);
        node.transform.position -= new Vector3(0, .1f, 0);
        string nodeText = "Mac: " + mac + "\n" + "RSSI: " + rssi + "dB";
        node.GetComponent<NodeBehavior>().Init(nodeText, numNodes);
    }
}
