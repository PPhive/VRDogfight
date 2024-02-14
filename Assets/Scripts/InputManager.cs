using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public UnityEvent TriggerDown;

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Debug.Log("Multiple Inputs detected");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TriggerDown.Invoke();
        }
    }

    public void TestMethod() 
    {
        //Debug.Log("Clicked!");
    }
}
