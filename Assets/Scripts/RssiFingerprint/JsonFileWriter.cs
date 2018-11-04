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
    public Vector3 pos;

    public GridData(string mac, int strength, Vector3 pos) {
        this.mac = mac;
        this.strength = strength;
        this.pos = pos;
    }
}
[Serializable]
public class GridDataCollection {
    public List<GridData> nodes = new List<GridData>();
}

public class JsonFileWriter : MonoBehaviour {

    const string WRITE_DATABASE = "http://matthewhallberg.com/WriteMap.php";
    const string READ_DATABASE = "http://matthewhallberg.com/ReadMap.php";
    const string MAP_NAME = "mainMap";

    private string path;
    private GridDataCollection gridDataCollection = new GridDataCollection();

    public void Start() {
        //set path for local file
        path = Path.Combine(Application.persistentDataPath, "nodeData.json");
    }

    public void AddNode(string macAddress,int rssi,Vector3 position){
        gridDataCollection.nodes.Add(new GridData(macAddress, rssi, position));
    }

    public void SaveMap(){
        //serialize data
        string jsonDataString = JsonUtility.ToJson(gridDataCollection, true);

        //write to file locally
        //File.WriteAllText(path, jsonDataString);

        //write to database
        StartCoroutine(WriteToDatabase(jsonDataString));
    }

    IEnumerator WriteToDatabase(string json){
        WWWForm form = new WWWForm();
        form.AddField("mapInfo", json);
        form.AddField("mapName", MAP_NAME);
        WWW www = new WWW(WRITE_DATABASE, form);
        yield return www;
        Debug.Log(www.text);
    }

    public void LoadMap(){
        //load all json from file
        //string loadedJsonDataString = File.ReadAllText(path);

        //load all json from database
        StartCoroutine(ReadFromDatabase());
    }

    IEnumerator ReadFromDatabase() {
        WWWForm form = new WWWForm();
        form.AddField("mapName", MAP_NAME);
        WWW www = new WWW(READ_DATABASE, form);
        yield return www;
        string loadedJsonDataString = www.text;
        //deserialize json
        gridDataCollection = JsonUtility.FromJson<GridDataCollection>(loadedJsonDataString);
        LoopThroughAllNodes();
    }

    void LoopThroughAllNodes(){
        //loop though all nodes
        foreach (GridData item in gridDataCollection.nodes) {
            string debugText = "ITEM: " + gridDataCollection.nodes.IndexOf(item) + "\n"
                + "mac: " + item.mac + "\n"
                + "rssi: " + item.strength + "\n"
                + "position: " + item.pos;
            Debug.Log(debugText);
        }
    }
}
