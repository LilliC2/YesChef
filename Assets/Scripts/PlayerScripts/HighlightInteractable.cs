using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class HighlightInteractable : GameBehaviour
{
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] Material highlightMaterial;
    public GameObject highlightPrefab;
    [SerializeField]
    List<GameObject> onTriggerGOs = new();

    GameObject HighlightPriority()
    {
        bool foundPickUp = false;
        GameObject priortyItem =null;
        foreach (GameObject go in onTriggerGOs)
        {
            if (go.CompareTag("PickUp"))
            {
                priortyItem = go;
                foundPickUp = true;

            }
            if (!foundPickUp)
                priortyItem = onTriggerGOs[1];
        }
            return priortyItem;
    }
    void RemoveHighlight(GameObject go)
    {
        //Don't need to property block because its already the correct material
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;


            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains(highlightMaterial.name))
                {
                    materials[i].SetFloat("_LightenFactor", 0f);
                }

            }

            renderer.materials = materials;
        }
    }

    public void PrepareHighlight(GameObject previewGameObject)
    {
        if (previewGameObject == null) Debug.Log($"previewGameObject is null");
        else print($"Highlight {previewGameObject.name}");

        if (previewGameObject == highlightPrefab) return;
        highlightPrefab = previewGameObject;


        Renderer[] renderers = previewGameObject.GetComponentsInChildren<Renderer>();
        MaterialPropertyBlock materialProperyBlock = new MaterialPropertyBlock();


        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            
            for (int i = 0; i < materials.Length; i++)
            {
                if(materials[i].name.Contains(highlightMaterial.name)  )
                {
                    materials[i].SetFloat("_LightenFactor", 0.2f);
                }
                else
                {
                    //get texture
                    Texture2D texture = renderer.material.GetTexture("_MainTex") as Texture2D;
                    if (texture == null) texture = renderer.material.GetTexture("_BaseMap") as Texture2D;

                    materials[i] = highlightMaterial;
                    //get material to property blocj
                    renderer.GetPropertyBlock(materialProperyBlock, i);

                    //set texture
                    materialProperyBlock.SetTexture("_MainTex", texture);
                    //set propery block
                    renderer.SetPropertyBlock(materialProperyBlock, i);
                }
               
            }

            renderer.materials = materials;

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("InteractableMask"))
        {
            print($"Trigger with {other.name}");
            if(!onTriggerGOs.Contains(other.gameObject)) onTriggerGOs.Add(other.gameObject);
            //PrepareHighlight(other.gameObject);
            if (other.gameObject == null) return;
            if (onTriggerGOs.Count > 1) //if there are multiple items in trigger (e.g if order is on table) find priorty
            {
                print($"Multiple hightlight {onTriggerGOs.Count}");
                PrepareHighlight(HighlightPriority());

            }
            else PrepareHighlight(other.gameObject);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (onTriggerGOs.Contains(other.gameObject))
        {
            onTriggerGOs.Remove(other.gameObject);
            RemoveHighlight(other.gameObject);
            if (highlightPrefab = other.gameObject)
            {
                highlightPrefab = null;
                if(onTriggerGOs.Count == 1) highlightPrefab = onTriggerGOs[0];
            }
        }

    }
}

