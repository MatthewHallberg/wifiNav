using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateMap : MonoBehaviour {

    public NodeController nodeController;
    public PositionalTracker positionalTracker;
    public WifiSignal wifiSignal;

    public GameObject nodePrefab;

    private bool isMapping = false;
    private int numNodes = 0;
    private Vector3 tempPos = Vector3.zero;

    // Update is called once per frame
    void Update() {
        if (isMapping) {
            tempPos.x = positionalTracker.GetPosition().x;
            tempPos.y = Camera.main.transform.position.y;
            tempPos.z = positionalTracker.GetPosition().y;
            Collider[] hitColliders = Physics.OverlapSphere(tempPos, .4f);
            if (hitColliders.Length == 0) {
                //record new node
                CreateNode();
            }
        }
    }

    public void AddNewLabel(string label){
        //find closest node
        Vector3 camVector3 = Camera.main.transform.position;
        Vector2 camVector2 = new Vector2(camVector3.x, camVector3.z);
        List<GridData> orderedNodes = nodeController.GetNodes().OrderBy(
            x => Vector2.Distance(camVector2, x.pos)).ToList();
        GridData closestNode = orderedNodes[0];
        //add label to closest node
        closestNode.label = label;
    }

    public void StartMapping() {
        isMapping = true;
    }

    public void SaveMap() {
        isMapping = false;
        nodeController.SaveMap();
    }

    void CreateNode() {
        numNodes++;
        Debug.Log("Node: " + numNodes);
        //get node info
        Vector2 pos = positionalTracker.GetPosition();
        int rssi = wifiSignal.GetCurrSignal();
        string mac = wifiSignal.GetMacAddress();
        //add node info to json string
        nodeController.AddNode(mac, rssi, pos);
        //instantiate node
        Vector3 worldNodePos = new Vector3(pos.x, Camera.main.transform.position.y, pos.y);
        GameObject node = Instantiate(nodePrefab, worldNodePos, Quaternion.identity);
        node.transform.position -= new Vector3(0, .1f, 0);
        string nodeText = "Mac: " + mac + "\n" + "RSSI: " + rssi + "dB";
        node.GetComponent<NodeBehavior>().Init(nodeText, numNodes);
    }
}
