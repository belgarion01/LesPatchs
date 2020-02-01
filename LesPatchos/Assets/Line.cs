using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    Camera mainCamera;

    [HideInInspector]
    public Personne originPersonne;
    Personne endPersonne;

    LineRenderer lRenderer;

    public LayerMask raycastMask;

    bool connected = false;

    Ray debugRay;

    private void Start()
    {
        lRenderer = GetComponent<LineRenderer>();
        lRenderer.SetPosition(0, transform.position);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (connected) return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        debugRay = ray;
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity/*, raycastMask*/))
        {
            if(hit.collider.TryGetComponent(out Personne personne))
            {
                if(personne != originPersonne && !isAlreadyConnected(personne) && personne.ConnectionsAvailable())
                {
                    SetLineEndPosition(personne.transform.position);
                    endPersonne = personne;
                }

                else
                {
                    SetLineEndPosition(hit.point + Vector3.up * 0.2f);
                    endPersonne = null;
                }
            }

            else
            {
                SetLineEndPosition(hit.point + Vector3.up*0.2f);
                endPersonne = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (connected) return;

            if (endPersonne == null) Destroy(gameObject);

            else
            {
                connected = true;
                ConnectBuddy(originPersonne, endPersonne);
                transform.position = Vector3.zero;
                CreateLineRendererMesh();
  
            }
        }
    }

    private void OnMouseDown()
    {
        BreakBuddy(originPersonne, endPersonne);
        Destroy(gameObject);
    }

    private void SetLineEndPosition(Vector3 position)
    {
        lRenderer.SetPosition(1, position);
    }

    private void ConnectBuddy(Personne buddy1, Personne buddy2)
    {
        buddy1.AddConnectedBuddy(buddy2);
        buddy2.AddConnectedBuddy(buddy1);
    }

    private void BreakBuddy(Personne buddy1, Personne buddy2)
    {
        buddy1.RemoveConnectedBuddy(buddy2);
        buddy2.RemoveConnectedBuddy(buddy1);
    }

    bool isAlreadyConnected(Personne buddy)
    {
        if (buddy.connectedBuddy.Contains(originPersonne)) return true;
        else return false;
    }

    void CreateLineRendererMesh()
    {
        Mesh mesh = new Mesh();
        lRenderer.BakeMesh(mesh, mainCamera, true);
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
