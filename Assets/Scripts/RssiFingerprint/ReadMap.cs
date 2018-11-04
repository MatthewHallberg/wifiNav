using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMap : MonoBehaviour {

    public JsonFileWriter jsonFileWriter;
    public GameObject nodePrefab;
    public Transform map;

    private int numNodes = 0;

    private void Start() {
        LoadMap();
    }

    public void LoadMap() {
        jsonFileWriter.LoadMap();
    }

    public void DisplayMap(List<GridData> nodes) {
        numNodes = 0;
        foreach (GridData node in nodes) {
            GameObject mapNode = Instantiate(nodePrefab);
            mapNode.transform.position = new Vector3(node.pos.x, 0, node.pos.z);
            mapNode.transform.SetParent(map);
            string nodeText = "Mac: " + node.mac + "\n" + "RSSI: " + node.strength + "dB";
            numNodes++;
            mapNode.GetComponent<NodeBehavior>().Init(nodeText, numNodes);
        }
    }
}
