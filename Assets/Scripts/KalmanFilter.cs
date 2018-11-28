using UnityEngine;
using System.Collections;

public class KalmanFilter : MonoBehaviour {
    public double Q = 0.000001;
    public double R = 0.01;
    private double P = 1, X = 0, K;

    void Start() {
         //PerfomKalmanTest();
    }

    void Update() {
        // Example use

        //double psudoVar = Random.Range(0, 100);
        //double kalmanVar = (double)Mathf.Round((float)KalmanUpdate(psudoVar));
        //print("Psudo: " + psudoVar + " , " + kalmanVar);
    }

    void measurementUpdate() {
        K = (P + Q) / (P + Q + R);
        P = R * (P + Q) / (R + P + Q);
    }

    public double KalmanUpdate(double measurement) {
        measurementUpdate();

        double result = X + (measurement - X) * K;
        X = result;
        return result;
    }

    void PerfomKalmanTest() {
        int[] DATA = new int[16] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 100, 10, 2, 3, 3, 1, 0 };

        for (int i = 0; i < DATA.Length; i++) {
            print(Mathf.Round((float)KalmanUpdate(DATA[i])) + ",");
        }
    }
}

// <copyright file="SimpleKalmanFilter.cs" company="dyadica.co.uk">
// Copyright (c) 2010, 2014 All Right Reserved, http://www.dyadica.co.uk

// This source is subject to the dyadica.co.uk Permissive License.
// Please see the http://www.dyadica.co.uk/permissive-license file for more information.
// All other rights reserved.

// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

// </copyright>

// <author>SJB</author>
// <email>contact via facebook.com/adropinthedigitalocean</email>
// <date>29.10.2014</date>
// <summary>A Unity MonoBehaviour implimentation of my Very Simple Kalman in C# posting:
// http://www.dyadica.co.uk/journal/very-simple-kalman-in-c/
// </summary