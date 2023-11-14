using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private GameObject currentBuildPrefab = null;
    private int maxBuildingSchemats = 1;
    private int currentBuildingSchemats = 0;

    public GameObject[] buildPrefabs = new GameObject[9];
   
    public void PickNewPrefab(int num, scaler info)
    {
        RemoveCurrentBuild();

        currentBuildPrefab = Instantiate(buildPrefabs[num], transform.position, Quaternion.identity);
        currentBuildPrefab.GetComponent<BuildingInfo>().SetBuildingInfo(info);
    }

    private void Update()
    {
        if(currentBuildPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float Xoffset = currentBuildPrefab.transform.localScale.x / 2;
                float Zoffset = currentBuildPrefab.transform.localScale.z / 2;

                Vector3 newPos = new Vector3(Mathf.RoundToInt(hit.point.x) + Xoffset, 0.75f, Mathf.RoundToInt(hit.point.z) + Zoffset);

                currentBuildPrefab.transform.position = newPos;
            }
        }

        if(Input.GetMouseButtonDown(1) && currentBuildPrefab != null)
        {
            RemoveCurrentBuild();
        }

        /*if(Input.GetKeyDown(KeyCode.E) && currentBuildPrefab != null)
        {
            currentBuildPrefab.transform.localEulerAngles += new Vector3(0, 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentBuildPrefab != null)
        {
            currentBuildPrefab.transform.localEulerAngles -= new Vector3(0, 90, 0);
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && currentBuildPrefab != null)
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
