using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolectableObjects : MonoBehaviour
{
    [SerializeField] Miscellaneous _resource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            InventoryManager.InventoryInstance.AddItem(_resource, _resource.maxValueAdded);
            Destroy(this.gameObject);
        }
    }
}
