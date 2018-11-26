using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReadMap : MonoBehaviour {

    const float MAX_NODE_DISTANCE = 1f; //RSSI in dBm

    public WifiSignal wifiSignal;
    public Transform user;
    public NodeController nodeController;
    public GameObject nodePrefab;
    public GameObject labelPrefab;
    public Transform map;
    public GameObject lineRendererPrefab;
    public Text debugText;

    private LineRenderer currLineRenderer;
    private List<GridData> allNodes = new List<GridData>();

    private bool initialized = false;
    private int lastRSSI = -100;
    private int rssiChangedCount = 0;
    private Vector3 desiredPosition = new Vector3(0, .2f, 0);

    private void Start() {
        LoadMap();
    }

    private void Update() {
        if (initialized){
            int currSignal = wifiSignal.GetCurrSignal();
            if (Mathf.Abs(lastRSSI - currSignal) > MAX_NODE_DISTANCE){
                rssiChangedCount++;
                if (rssiChangedCount > 2) {
                    lastRSSI = currSignal;
                    UpdateUserPosition(currSignal);
                }
            } else {
                rssiChangedCount = 0;
            }
            user.position = Vector3.Lerp(user.position, desiredPosition, Time.deltaTime * 6f);
        }
    }

    void UpdateUserPosition(int currRSSI){
        //get mac addres of currently connected access point
        string currMac = wifiSignal.GetMacAddress();
        //first get list of nodes with same mac address
        List<GridData> macNodes = nodeController.GetNodes().Where(x => x.mac == currMac).ToList();
        //find closest node by RSSI value from previous list
        GridData closestNode = macNodes.OrderBy(item => Mathf.Abs(currRSSI - item.strength)).First();
        //set user dot to move to this position
        desiredPosition = new Vector3(closestNode.pos.x, .2f, closestNode.pos.y);
        Debug.Log("User Position: " + desiredPosition);
    }

    public void LoadMap() {
        nodeController.LoadMap();
    }

    public void DisplayMap(List<GridData> nodes) {

        //order nodes by distance x and y to see which is greater
        List<GridData> yNodes = nodes.OrderBy(x => x.pos.y).ToList();
        float distanceY = Vector2.Distance(yNodes[0].pos, yNodes[yNodes.Count - 1].pos);
        List<GridData> xNodes = nodes.OrderBy(x => x.pos.x).ToList();
        float distanceX = Vector2.Distance(xNodes[0].pos, xNodes[xNodes.Count - 1].pos);
        if (distanceX > distanceY) allNodes = xNodes;
        else allNodes = yNodes;

        List<Vector2> nodePositions = new List<Vector2>();
        List<LineRenderer> currLines = new List<LineRenderer>();
        Vector2 lastPos = Vector2.zero;
        int currNodeCount = 0;
        currLineRenderer = Instantiate(lineRendererPrefab, map).GetComponent<LineRenderer>();

        //draw map with line renderer
        foreach (GridData node in allNodes) {

            if (Vector2.Distance(node.pos,lastPos) > MAX_NODE_DISTANCE) {
                //create new line segment
                currLineRenderer = Instantiate(lineRendererPrefab, map).GetComponent<LineRenderer>();
                currNodeCount = 0;
                currLines.Add(currLineRenderer);
            }
            currLineRenderer.positionCount = currNodeCount + 1;
            Vector3 nodePosition3D = new Vector3(node.pos.x, .1f, node.pos.y);
            currLineRenderer.SetPosition(currNodeCount, nodePosition3D);
            nodePositions.Add(node.pos);
            lastPos = node.pos;
            currNodeCount++;
        }

        //connect closest lines
        List<Vector3> endPoints = new List<Vector3>();
        foreach(LineRenderer line in currLines){
            endPoints.Add(line.GetPosition(0));
            endPoints.Add(line.GetPosition(line.positionCount - 1));
        }
        //this results in some duplicates and overkill but I am pretty much over this project so here we are.
        foreach(Vector3 point in endPoints){
            foreach(Vector3 newPoint in endPoints){
                if (newPoint != point && Vector3.Distance(point,newPoint) < MAX_NODE_DISTANCE){
                    currLineRenderer = Instantiate(lineRendererPrefab, map).GetComponent<LineRenderer>();
                    currLineRenderer.positionCount = 2;
                    currLineRenderer.SetPosition(0, newPoint);
                    currLineRenderer.SetPosition(1, point);
                }
            }
        }

        //position camera
        StartCoroutine(PositionCamRoutine());
    }

    IEnumerator PositionCamRoutine() {

        //create get max and min point on map
        Vector2 minPoint = allNodes[0].pos;
        Vector2 maxPoint = allNodes[allNodes.Count - 1].pos;
        GameObject min = GameObject.CreatePrimitive(PrimitiveType.Cube);
        min.name = "min";
        min.transform.localScale = Vector3.one * .1f;
        Renderer minRenderer = min.GetComponent<Renderer>();
        min.transform.SetParent(map);
        min.transform.position = new Vector3(minPoint.x, 0, minPoint.y);
        GameObject max = GameObject.CreatePrimitive(PrimitiveType.Cube);
        max.name = "max";
        max.transform.localScale = Vector3.one * .1f;
        Renderer maxRenderer = min.GetComponent<Renderer>();
        max.transform.SetParent(map);
        max.transform.position = new Vector3(maxPoint.x, 0, maxPoint.y);

        //set heading direction of camera
        Transform cam = Camera.main.transform;
        cam.position = min.transform.position;
        cam.LookAt(max.transform.position);
        Camera.main.transform.eulerAngles += new Vector3(90, -90, 0);

        //position camera at center between max and min points
        Vector2 middlePoint = Vector2.Lerp(minPoint, maxPoint, .5f);
        Camera.main.transform.position = new Vector3(middlePoint.x, 1, middlePoint.y);

        //move camera up until max and min of map is visible
        if (maxRenderer != null && minRenderer != null) {
            while (!maxRenderer.isVisible && !maxRenderer.isVisible) {
                Camera.main.transform.position += new Vector3(0, 2f, 0);
                yield return null;
            }
            Camera.main.transform.position += new Vector3(0, 2f, 0);
        }

        //add label and position towards camera
        foreach (GridData node in allNodes) {
            if (node.label != string.Empty) {
                GameObject label = Instantiate(labelPrefab, map);
                float yPos = label.transform.position.y;
                label.transform.position = new Vector3(node.pos.x, yPos, node.pos.y);
                Debug.Log("LABEL: " + node.label);
                label.GetComponent<TextMeshPro>().text = node.label;
                label.transform.rotation = Camera.main.transform.rotation;
            }
        }
        user.rotation = Camera.main.transform.rotation;
        initialized = true;
    }
}
