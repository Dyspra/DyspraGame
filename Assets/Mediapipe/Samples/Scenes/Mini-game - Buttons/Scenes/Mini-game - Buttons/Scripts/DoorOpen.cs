using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{
    public PhysicsButton button1;
    public PhysicsButton button2;
    public GameObject door;
    public GameObject player;
    public Image fondu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (button1.isPressed && button2.isPressed) {
            door.transform.eulerAngles = Vector3.Lerp(door.transform.eulerAngles, new Vector3(0, -90f, 0), Time.deltaTime * 0.1f);
            Debug.Log(door.transform.rotation);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 3f * Time.deltaTime);
            fondu.color = new Color(fondu.color.r, fondu.color.g, fondu.color.b, fondu.color.a + 0.01f);
        }
    }
}
