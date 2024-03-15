using UnityEngine;
using System;

[ExecuteInEditMode]
public class Telemetry : MonoBehaviour
{
    public Vector3 PitchYawRoll;

    string apiMode = "api";  //constant to identify the package
    public string game = "Project Cars 2";  //constant to identify the game
    public string vehicle = "Lamborghini Huracan";  //constant to identify the vehicle
    public string location = "Circuit Gilles-Villeneuve";  //constant to identify the location
    uint apiVersion = 102;  //constant of the current version of the api

    //gets the vehicle body to send speed to SRS
	Rigidbody vehicleBody;

    void Start ()
    {
        vehicleBody = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {

        SimRacingStudio.SimRacingStudio_SendTelemetry(apiMode.PadRight(50).ToCharArray()
                                                     , apiVersion
                                                     , game.PadRight(50).ToCharArray()
                                                     , vehicle.PadRight(50).ToCharArray()
                                                     , location.PadRight(50).ToCharArray()
                                                     , Convert.ToSingle(vehicleBody.velocity.magnitude * 100)
                                                     , 7000
                                                     , 8000
                                                     , -1
                                                     , PitchYawRoll.x //pitch
                                                     , PitchYawRoll.z//Roll
                                                     , PitchYawRoll.y //Yaw
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0
                                                     , 0);
    }
}
