using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

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

    private string path;
    private GridDataCollection gridDataCollection = new GridDataCollection();

    public void Start() {
        //set path
        path = Path.Combine(Application.persistentDataPath, "nodeData.json");
    }

    public void AddNode(string macAddress,int rssi,Vector3 position){
        gridDataCollection.nodes.Add(new GridData(macAddress, rssi, position));
    }

    public void SaveMap(){
        //write all current nodes to JSON file
        SerializeData();
    }

    public void LoadMap(){
        //load all nodes from file
        DeserializeData();
        foreach(GridData item in gridDataCollection.nodes){
            Debug.Log("ITEM: "+ gridDataCollection.nodes.IndexOf(item));
            Debug.Log("mac: "+ item.mac);
            Debug.Log("rssi: " + item.strength);
            Debug.Log("position: " + item.pos);
        }
    }

    void SerializeData() {
        string jsonDataString = JsonUtility.ToJson(gridDataCollection, true);
        File.WriteAllText(path, jsonDataString);
    }

    void DeserializeData() {
        string loadedJsonDataString = File.ReadAllText(path);
        gridDataCollection = JsonUtility.FromJson<GridDataCollection>(loadedJsonDataString);
     }
}
