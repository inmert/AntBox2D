using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickToAdd : MonoBehaviour
{

    public GameObject prefabObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure z position is appropriate for your scene

            // Instantiate prefab at mouse position
            Instantiate(prefabObject, mousePosition, Quaternion.identity);

        }
    }
}
