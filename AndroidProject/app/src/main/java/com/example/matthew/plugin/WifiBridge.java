package com.example.matthew.plugin;

import android.net.wifi.WifiManager;
import android.content.Context;

import com.unity3d.player.UnityPlayer;
import android.util.Log;

public class WifiBridge {

    static WifiManager wifiManager;

    public static void Init(){
        Log.d("TAG","Init FROM ANDROID");
        wifiManager = (WifiManager) UnityPlayer.currentActivity.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
    }

    public static int GetRSSI(){
        return wifiManager.getConnectionInfo().getRssi();
    }

    public static int GetFrequency(){
        return wifiManager.getConnectionInfo().getFrequency();
    }

    public static int GetLinkSpeed(){
        return wifiManager.getConnectionInfo().getLinkSpeed();
    }

    public static String GetMac(){
        return wifiManager.getConnectionInfo().getBSSID();
    }
}
