using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableAttributes : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField] PassionObject passionObject;

    [SerializeField] LayerMask raycastMask;

    [SerializeField] float groundOffset = 0.2f;

    Personne targetBuddy;

    bool dragged = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void OnMouseDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask))
        {
            dragged = true;

            transform.position = hit.point + Vector3.up * groundOffset;

            if(hit.collider.TryGetComponent(out Personne buddy))
            {
                targetBuddy = buddy;
            }

            else
            {
                targetBuddy = null;
            }
        }
    }

    private void OnMouseUp()
    {
        if (targetBuddy == null) return;

        if (!dragged || targetBuddy.hasAttributes(passionObject.passion)) return;

        targetBuddy.AddAttributes(passionObject);

        targetBuddy.SetAttributeToRemovable(passionObject.passion, true);

        Destroy(gameObject);
    }
}
