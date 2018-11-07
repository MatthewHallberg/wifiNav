using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReadMap : MonoBehaviour {

    const float MAX_NODE_DISTANCE = 1.1f;

    public JsonFileWriter jsonFileWriter;
    public GameObject nodePrefab;
    public Transform map;
    public GameObject lineRendererPrefab;

    private LineRenderer currLineRenderer;
    private List<GridData> allNodes = new List<GridData>();

    private void Start() {
        LoadMap();
    }

    public void LoadMap() {
        jsonFileWriter.LoadMap();
    }

    public void DisplayMap(List<GridData> nodes) {

        allNodes = nodes;
        allNodes = allNodes.OrderBy(x => Vector2.SqrMagnitude(x.pos)).ToList();
        List<Vector3> nodePositions = new List<Vector3>();
        Vector3 lastPos = Vector3.zero;
        int currNodeCount = 0;
        currLineRenderer = Instantiate(lineRendererPrefab, map).GetComponent<LineRenderer>();

        //draw map with line renderer
        foreach (GridData node in allNodes) {
            if (Vector3.Distance(node.pos,lastPos) > MAX_NODE_DISTANCE) {
                //create new line segment
                currLineRenderer = Instantiate(lineRendererPrefab, map).GetComponent<LineRenderer>();
                currNodeCount = 0;
            }
            currLineRenderer.positionCount = currNodeCount + 1;
            Vector3 nodePosition = new Vector3(node.pos.x, .1f, node.pos.y);
            currLineRenderer.SetPosition(currNodeCount, nodePosition);
            nodePositions.Add(node.pos);
            lastPos = node.pos;
            currNodeCount++;
        }

        //position camera
        StartCoroutine(PositionCamRoutine());
    }

    IEnumerator PositionCamRoutine() {
        //position camera at center between max and min points
        Vector3 minPoint = allNodes[0].pos;
        Vector3 maxPoint = allNodes[allNodes.Count - 1].pos;
        Vector3 middlePoint = Vector3.Lerp(minPoint, maxPoint, .5f);
        Camera.main.transform.position = new Vector3(middlePoint.x, 10, middlePoint.z);

        //move camera up until max and min of map is visible
        GameObject min = GameObject.CreatePrimitive(PrimitiveType.Cube);
        min.name = "min";
        min.transform.localScale = new Vector3(.01f, .01f, .01f);
        Renderer minRenderer = min.GetComponent<Renderer>();
        min.transform.SetParent(map);
        min.transform.position = minPoint;
        GameObject max = GameObject.CreatePrimitive(PrimitiveType.Cube);
        max.name = "max";
        max.transform.localScale = new Vector3(.01f, .01f, .01f);
        Renderer maxRenderer = min.GetComponent<Renderer>();
        max.transform.SetParent(map);
        max.transform.position = maxPoint;
        if (maxRenderer != null && minRenderer != null) {
            while (!maxRenderer.isVisible && !maxRenderer.isVisible) {
                Camera.main.transform.position += new Vector3(0, 1f, 0);
                yield return null;
            }
            Camera.main.transform.position += new Vector3(0, 1f, 0);
        }
    }
}
