using System.Collections;
using System.Collections.Generic;
using TransformGizmos;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject selectedObject = null;
    public GameObject Gizmo=null;
    bool locked=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (!locked)
                {
                    locked = true;
                    selectedObject = hit.transform.gameObject;
                    Gizmo.SetActive(true);
                    Gizmo.GetComponent<GizmoController>().m_targetObject = selectedObject;
                    Gizmo.GetComponent<GizmoController>().m_translation.gameObject.SetActive(false);
                    Gizmo.GetComponent<GizmoController>().m_rotation.gameObject.SetActive(false);
                    Gizmo.GetComponent<GizmoController>().m_scaling.gameObject.SetActive(false);
                    Gizmo.GetComponent<GizmoController>().m_translation.gameObject.SetActive(true);
                  //  Gizmo.transform.localScale = new Vector3(selectedObject.transform.localScale.y, selectedObject.transform.localScale.y, selectedObject.transform.localScale.y);
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                }
            }
            else
            {
                locked=false;
                Gizmo.GetComponent<GizmoController>().m_translation.gameObject.SetActive(false);
                Gizmo.GetComponent<GizmoController>().m_rotation.gameObject.SetActive(false);
                Gizmo.GetComponent<GizmoController>().m_scaling.gameObject.SetActive(false);
                Gizmo.SetActive(false);
                selectedObject= null;
            }
        }
    }
}
