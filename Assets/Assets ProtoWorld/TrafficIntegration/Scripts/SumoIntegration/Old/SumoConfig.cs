﻿/*
 * 
 * SUMO COMMUNICATION
 * SumoConfig.cs
 * Miguel Ramos Carretero
 * 
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

/// <summary>
/// Sets the configuration parameters to run SUMO in Unity. 
/// </summary> 
public class SumoConfig : MonoBehaviour 
{
    public bool useConfFile = false;
    public bool integrateSumo = false;
    public bool sumoIsRunningInLocalHost = true;
    public string remoteIp = "127.0.0.1";
    public int remotePortForTraci = 3456;
    public int remotePortForListener = 3654;
    public bool useFrustumForUpdate = true;
    public bool useCoordinateConversion = true;
    public bool vehicleBrakingActive = true;
    public int timeToBrakeInSeconds = 1;
    public int driversPatientInSeconds = 2;
    public int driversAngleOfView = 45;
    public bool simulateFromFCDFile = false;
    public string FCDFilePath = "";

    void Awake()
    {
        string path = Application.dataPath + "/confSUMO.cfg";
        Debug.Log("This is the path of the SUMO config file: " + path);

        //Use the confSUMO.cgf in order to make the project portable -- Miguel R. C.
        if (File.Exists(path) == true && useConfFile)
        {
            Debug.Log("Exists confSUMO.cfg");
            FileStream fs = File.OpenRead(path);
            StreamReader sr = new StreamReader(fs);
            string[] split;
            string line;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                split = line.Split(new Char[] { ' ' });

                if (split[0].Equals("integrateSUMO"))
                {
                    if (split[1].Equals("true"))
                        integrateSumo = true;
                }
                else if (split[0].Equals("tcpAddress"))
                {
                    if (!split[1].Equals("localhost"))
                    {
                        sumoIsRunningInLocalHost = false;
                        remoteIp = split[1];
                    }
                }
                else if (split[0].Equals("portTraCI"))
                {
                    if (!split[1].Equals(null))
                        remotePortForTraci = int.Parse(split[1]);
                }
                else if (split[0].Equals("portListener"))
                {
                    if (!split[1].Equals(null))
                        remotePortForListener = int.Parse(split[1]);
                }
                else if (split[0].Equals("vehicleBrakingActive"))
                {
                    if (split[1].Equals("false"))
                        vehicleBrakingActive = false;
                }
                else if (split[0].Equals("readFCDFile"))
                {
                    if (split[1].Equals("true"))
                        simulateFromFCDFile = true;
                }
                else if (split[0].Equals("FCDFilePath"))
                {
                    if (!split[1].Equals(null))
                        FCDFilePath = split[1];
                }
            }

            sr.Close();
        }
    }
}
