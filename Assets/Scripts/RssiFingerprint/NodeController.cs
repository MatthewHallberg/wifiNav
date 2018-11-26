using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

[Serializable]
public class GridData {
    public string mac;
    public int strength;
    public Vector2 pos;
    public string label;

    public GridData(string mac, int strength, Vector2 pos) {
        this.mac = mac;
        this.strength = strength;
        this.pos = pos;
        this.label = string.Empty;
    }
}
[Serializable]
public class GridDataCollection {
    public List<GridData> nodes = new List<GridData>();
}

public class NodeController : MonoBehaviour {

    const string WRITE_DATABASE = "http://matthewhallberg.com/WriteMap.php";
    const string READ_DATABASE = "http://matthewhallberg.com/ReadMap.php";
    const string MAP_NAME = "matt";

    public Text debugText;

    private GridDataCollection gridDataCollection = new GridDataCollection();

    public void AddNode(string macAddress,int rssi,Vector2 position){
        gridDataCollection.nodes.Add(new GridData(macAddress, rssi, position));
    }

    public List<GridData> GetNodes(){
        return gridDataCollection.nodes;
    }

    public void SaveMap(){
        //serialize data
        string jsonDataString = JsonUtility.ToJson(gridDataCollection, true);

        //write to database
       StartCoroutine(WriteToDatabase(jsonDataString));
    }

    IEnumerator WriteToDatabase(string json){
        debugText.text = "";
        WWWForm form = new WWWForm();
        form.AddField("mapInfo", json);
        form.AddField("mapName", MAP_NAME);
        WWW www = new WWW(WRITE_DATABASE, form);
        yield return www;
        Debug.Log(www.text);
        debugText.text = gridDataCollection.nodes.Count + " nodes uploaded.\nResult: " + www.text;
    }

    public void LoadMap(){
        //load all json from database
        StartCoroutine(ReadFromDatabase());
    }

    IEnumerator ReadFromDatabase() {
        WWWForm form = new WWWForm();
        form.AddField("mapName", MAP_NAME);
        WWW www = new WWW(READ_DATABASE, form);
        yield return www;
        string loadedJsonDataString = www.text;
        print(www.text);
        //deserialize json
        gridDataCollection = JsonUtility.FromJson<GridDataCollection>(loadedJsonDataString);
        //display map
        GetComponent<ReadMap>().DisplayMap(gridDataCollection.nodes);
    }
}
