using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            ((FreeFlyCamera)this.GetComponent<FreeFlyCamera>())._active = true;
        }
        else
        {            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            this.GetComponent<FreeFlyCamera>()._active = false;
        }
    }
}
