using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private GameObject currentBuildPrefab = null;
    private int maxBuildingSchemats = 1;
    private int currentBuildingSchemats = 0;

    public GameObject[] buildPrefabs = new GameObject[9];
   
    public void PickNewPrefab(int num)
    {
        RemoveCurrentBuild();

        currentBuildPrefab = Instantiate(buildPrefabs[num], transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if(currentBuildPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPos = new Vector3(Mathf.RoundToInt(hit.point.x), 0.75f, Mathf.RoundToInt(hit.point.z));

                currentBuildPrefab.transform.position = newPos;
            }
        }

        if(Input.GetMouseButtonDown(1) && currentBuildPrefab != null)
        {
            RemoveCurrentBuild();
        }

        if (Input.GetMouseButtonDown(0) && currentBuildPrefab != null)
        {
            SetNewBuild();
        }
    }

    private void RemoveCurrentBuild()
    {
        if (currentBuildPrefab != null)
        {
            Destroy(currentBuildPrefab);
            currentBuildPrefab = null;
        }
    }

    private void SetNewBuild()
    {
        /// WYWO£AJ FUNKCJE POSTAWIENIA NA SKRYPCIE BUDYNKU!!!!!
        currentBuildPrefab = null;
        currentBuildingSchemats++;
    }

    public void DecreseSchematsCount()
    {
        currentBuildingSchemats--;
    }

    public bool CheckForSchematPlace()
    {
        return (currentBuildingSchemats < maxBuildingSchemats);
    }
}
