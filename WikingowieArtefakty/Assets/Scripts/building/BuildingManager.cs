using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingState
{
    PlacingValid,
    PlacingInvalid,
    Erecting,
    Erected,
}
public class BuildingManager : MonoBehaviour
{
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    public Material ErectingMaterial;

    public MeshRenderer[] meshComponents;
    private Dictionary<MeshRenderer, List<Material>> initialMaterials;

    public bool hasValicPlacement;
    public bool isFixed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #if UNITY_EDITOR
        private void OnValidate()
    {
        InitializeMaterials();
    }
    #endif

    public void SetPlacementMode(BuildingState Mode);
    public void SetMaterial(BuildingState Mode);
    private void InitializeMaterials();
}
