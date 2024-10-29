using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerOutofBounds : MonoBehaviour
{
    [SerializeField]
    Player myPlayer;
    [SerializeField]
    TextMeshPro myText;

    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GetComponent<Player>();
        if (myPlayer == null)
        {
            Debug.Log("noplayer");
            Destroy(gameObject);
        }
        else 
        {
            if (myPlayer.AI || myText == null)
            {
                this.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer.myCurrentShip != null)
        {
            if (myPlayer.transform.position.magnitude > GameManager.instance.WorldRadius * 1.21)
            {
                myText.enabled = true;
                myText.fontSize = 20;
                myText.text = "Return to Battlefield";
                if (myPlayer.transform.position.magnitude > GameManager.instance.WorldRadius * 1.4)
                {
                    myPlayer.myShipUnit.Hit(99999);
                    myText.fontSize = 40;
                    myText.text = "Out of Bounds";
                }
            }
            else 
            {
                myText.enabled = false;
            }
        }
    }
}
