using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HighlightInteractable : GameBehaviour
{
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] Material highlightMaterial;
    public GameObject highlightPrefab;



    public void PrepareHighlight(GameObject previewGameObject)
    {
        if(previewGameObject == highlightPrefab) return;
        highlightPrefab = previewGameObject;

        print($"Prepare Highlight for {previewGameObject.name}");

        Renderer[] renderers = previewGameObject.GetComponentsInChildren<Renderer>();
        MaterialPropertyBlock materialProperyBlock = new MaterialPropertyBlock();


        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            
            for (int i = 0; i < materials.Length; i++)
            {

                //get texture
                Texture2D texture = renderer.material.GetTexture("_MainTex") as Texture2D;
                if (texture == null) texture = renderer.material.GetTexture("_BaseMap") as Texture2D;

                materials[i] = highlightMaterial;
                //get material to property blocj
                renderer.GetPropertyBlock(materialProperyBlock,i);

                //set texture
                materialProperyBlock.SetTexture("_MainTex", texture);
                //set propery block
                renderer.SetPropertyBlock(materialProperyBlock,i);
            }

            renderer.materials = materials;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        print($"Trigger with {other.name}");

        if (other.gameObject.layer == LayerMask.NameToLayer("InteractableMask"))
            PrepareHighlight(other.gameObject);

    }

}

