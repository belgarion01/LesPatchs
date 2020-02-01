using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MeshAttributes : SerializedMonoBehaviour
{
    public bool canBeDropped = false;

    [SerializeField] PassionObject passion;

    public Dictionary<Passion, GameObject> dragableAttributesPrefabs;

    [SerializeField] Transform dropPosition;

    private void OnMouseDown()
    {
        if (!canBeDropped) return;

        GetComponentInParent<Personne>().RemoveAttributes(passion);

        Instantiate(dragableAttributesPrefabs[passion.passion], dropPosition.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
