using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TerrainChecker : MonoBehaviour
{
    //init
    [SerializeField] Text[] layerTexts = null;
    [SerializeField] Text mousePositionText = null;
    [SerializeField] Text foundPhysicsObstacleText = null;

    public int layerMask = 0;

    Vector3 mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("agent id is" + );

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        NavMeshHit hit;
        NavMeshQueryFilter filter = new NavMeshQueryFilter();
        filter.areaMask = NavMesh.AllAreas;
        filter.agentTypeID = GameObject.FindGameObjectWithTag("NavMeshSurface").GetComponent<NavMeshSurface2d>().agentTypeID; //-1372625422;
        NavMesh.SamplePosition(mousePos, out hit, 0.1f, filter);
        bool[] layersFound = LayerMaskExtensions.HasLayers(hit.mask);

        mousePositionText.text = mousePos.ToString();
        for (int i = 0; i < layerTexts.Length; i++)
        {
            layerTexts[i].enabled = layersFound[i];
        }

        Debug.DrawLine(mousePos, mousePos + Vector3.one, Color.yellow);
        Debug.DrawLine(mousePos, mousePos - Vector3.left, Color.yellow);

        int layerMask_calc = 1 << layerMask;
        Collider2D rchit = Physics2D.OverlapCircle(mousePos, 0.3f, 1<<8);
        if (rchit)
        {
            foundPhysicsObstacleText.text = rchit.transform.gameObject.name + " at " + rchit.transform.position;
            Debug.DrawLine(mousePos, rchit.transform.position, Color.yellow);
        }
        if (!rchit)
        {
            foundPhysicsObstacleText.text = "no obstacle here";
        }


    }
}
