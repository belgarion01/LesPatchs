using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

public class Personne : SerializedMonoBehaviour
{
    #region Connection

    public List<Personne> connectedBuddy = new List<Personne>();

    [SerializeField] int maxNumberOfConnection = 2;
    private int numberOfConnection { get { return connectedBuddy.Count; } }

    public GameObject linePrefab;

    #endregion

    public List<PassionObject> Attributes;

    [ReadOnly]
    public Dictionary<Passion, bool> activatedAttributes = new Dictionary<Passion, bool> 
    {
        { Passion.Lecture, false },
        { Passion.Jeux, false },
        { Passion.Magie, false },
        { Passion.Chien, false },
        { Passion.Jardinerie, false },
        { Passion.Peinture, false }
    };

    public List<PassionObject> Envies;

    [ReadOnly]
    public Dictionary<Passion, bool> activatedEnvies = new Dictionary<Passion, bool>
    {
        { Passion.Lecture, false },
        { Passion.Jeux, false },
        { Passion.Magie, false },
        { Passion.Chien, false },
        { Passion.Jardinerie, false },
        { Passion.Peinture, false }
    };

    public void UpdateAttributes()
    {
        foreach(Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            activatedAttributes[passion] = false;
        }

        foreach(PassionObject PO in Attributes)
        {
            activatedAttributes[PO.passion] = true;
        }

        UpdateEnvies();
    }

    public void UpdateEnvies()
    {
        foreach (Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            activatedEnvies[passion] = false;
        }

        List<Passion> connectedAttributes = new List<Passion>();

        foreach(Personne buddy in connectedBuddy)
        {
            foreach(PassionObject PO in buddy.Attributes)
            {
                connectedAttributes.Add(PO.passion);
            }
        }

        foreach(Passion passion in connectedAttributes)
        {
            activatedEnvies[passion] = true;
        }
    }

    private void OnValidate()
    {
        UpdateAttributes();
    }

    private void OnMouseDown()
    {
        if (ConnectionsAvailable())
        {
            Line line = Instantiate(linePrefab, transform.position, Quaternion.identity).GetComponent<Line>();
            line.originPersonne = this;
        }
    }

    public void AddConnectedBuddy(Personne buddy)
    {
        connectedBuddy.Add(buddy);
        UpdateEnvies();
    }

    public void RemoveConnectedBuddy(Personne buddy)
    {
        connectedBuddy.Remove(buddy);
        UpdateEnvies();
    }

    public bool ConnectionsAvailable() 
    {
        return numberOfConnection < maxNumberOfConnection ? true : false;
    }
}
