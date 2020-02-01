using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine.UI;

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
    public bool isHappy = false;


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

    public Dictionary<Passion, Image> envieUI;
    public Dictionary<Passion, GameObject> attributeObject;

    GameManager gameManager;


    private void Start()
    {
        isHappy = false;

        gameManager = FindObjectOfType<GameManager>();
        gameManager.personnes.Add(this);

        foreach (Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            envieUI[passion].transform.gameObject.SetActive(false);
        }

        foreach (PassionObject PO in Envies)
        {
            envieUI[PO.passion].transform.gameObject.SetActive(true);
        }
    }

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

        foreach (Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            attributeObject[passion].transform.gameObject.SetActive(false);
        }

        foreach (PassionObject PO in Attributes)
        {
            attributeObject[PO.passion].SetActive(true);
        }

        UpdateEnvies();
    }

    public void UpdateEnvies()
    {
        //Reset the dictionnary
        foreach (Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            activatedEnvies[passion] = false;
        }

        List<Passion> connectedAttributes = new List<Passion>();

        //Add all passions from the connected buddy
        foreach(Personne buddy in connectedBuddy)
        {
            foreach(PassionObject PO in buddy.Attributes)
            {
                connectedAttributes.Add(PO.passion);
            }
        }

        //Set to true in the dictionnary
        foreach(Passion passion in connectedAttributes)
        {
            activatedEnvies[passion] = true;
        }

        UpdateEnviesFeedback();
        if(gameManager != null) gameManager.UpdateWinCondition();
    }

    public void UpdateEnviesFeedback()
    {
        foreach (Passion passion in Enum.GetValues(typeof(Passion)).Cast<Passion>())
        {
            envieUI[passion].transform.GetChild(0).gameObject.SetActive(false);
        }

        bool willBeHappy = true;

        foreach(PassionObject PO in Envies)
        {
            if (activatedEnvies[PO.passion])
            {
                //SetActiveUI
                envieUI.TryGetValue(PO.passion, out Image ui);
                ui.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                willBeHappy = false;
            }
        }

        if (willBeHappy) isHappy = true;
        else isHappy = false;
    }

    private void OnValidate()
    {
        UpdateAttributes();
    }

    public void AddAttributes(PassionObject passion)
    {
        Attributes.Add(passion);
        UpdateAttributes();
    }

    public void SetAttributeToRemovable(Passion passion, bool value)
    {
        attributeObject[passion].GetComponent<MeshAttributes>().canBeDropped = value;
    }

    public void RemoveAttributes(PassionObject passion)
    {
        Attributes.Add(passion);
        UpdateAttributes();
    }

    public bool hasAttributes(Passion passion)
    {
        bool toReturn = false;
        foreach(PassionObject PO in Attributes)
        {
            if (PO.passion == passion) toReturn = true;
        }

        return toReturn;
    }

    private void OnMouseDown()
    {
        if (ConnectionsAvailable() && !gameManager.gameEnded)
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
