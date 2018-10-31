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

	public int GetCurrSignal () {
		return pluginClass.CallStatic<int> ("GetRSSI");
	}

	public int GetFrequency () {
		return pluginClass.CallStatic<int> ("GetFrequency");
	}

	public int GetLinkSpeed() {
		return pluginClass.CallStatic<int> ("GetLinkSpeed");
	}

	//https://stackoverflow.com/questions/11217674/how-to-calculate-distance-from-wifi-router-using-signal-strength
}
