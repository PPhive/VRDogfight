using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugNotes : MonoBehaviour
{
    [SerializeField]
    TextMeshPro myText;
    [SerializeField]
    List<GameObject> trackedObjects;
    [SerializeField]
    string message;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        message = "";
        message += "Tracked objects:\n";
        foreach (GameObject currentobject in trackedObjects) 
        {
            message += currentobject.name + ": " + currentobject.transform.localPosition + " " + currentobject.transform.localEulerAngles + "\n";
        }

        myText.text = (message);
    }
}
