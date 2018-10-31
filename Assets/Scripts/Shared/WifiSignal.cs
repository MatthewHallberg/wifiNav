using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WifiSignal : MonoBehaviour{

	AndroidJavaClass pluginClass;
	
	private void Start () {
		//initialize plugin
		pluginClass = new AndroidJavaClass ("com.example.matthew.plugin.WifiBridge");
		pluginClass.CallStatic ("Init");
	}

    public string GetMacAddress() {
        return pluginClass.CallStatic<string>("GetMac");
    }

    public int GetCurrSignal () {
		return pluginClass.CallStatic<int> ("GetRSSI");
	}

	public int GetFrequency () {
		return pluginClass.CallStatic<int> ("GetFrequency");
	}

	public int GetLinkSpeed() {
		return pluginClass.CallStatic<int> ("GetLinkSpeed");
	}
}
