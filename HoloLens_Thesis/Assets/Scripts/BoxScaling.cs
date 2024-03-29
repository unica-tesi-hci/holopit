﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScaling : MonoBehaviour
{
	
	[Tooltip("The material displayed on the object during the placement.")]
    public Material PlacementMaterial;
	private GameObject[] selectedObjects = null;
	private GameObject[] box = null;
	private ContainerBox contBox;
    private string currentSequence;
    private float minX, maxX, minY, maxY, minZ, maxZ;

	// Use this for initialization
	void Awake () {
		contBox = GameObject.FindWithTag("ContainerBox").GetComponent<ContainerBox>();
	}
	
	public void ChooseSequence(string seq)
	{
        CockpitFeedback[] oi;
        bool[] in_box;
        int i;

        if (MenuManager.Instance.isMenuOpen())
        {
            MenuManager.Instance.CloseMenu();
        }

		DestroyAllObjects();

        currentSequence = seq;

        GameObject.FindWithTag("Cockpit").transform.GetChild(0).gameObject.SetActive(false);
		
		selectedObjects = InputSequence.Instance.getObjectsFromSequence(currentSequence);
		for(i = 0; i < selectedObjects.Length; i++)
		{
			selectedObjects[i].GetComponent<MeshRenderer>().enabled = true;
		}

        oi = InputSequence.Instance.GetSequence(currentSequence).objectIndicated.ToArray();
        in_box = new bool[oi.Length];
        for (i = 0; i < oi.Length; i++)
        {
            in_box[i] = oi[i].insideBox;
        }

        setIndicationObjects(selectedObjects, in_box);
		
		box[0].gameObject.tag = "Left";
		box[1].gameObject.tag = "Up";
		box[2].gameObject.tag = "Right";
		box[3].gameObject.tag = "Down";
		
		for(i = 0; i < box.Length; i++){
			box[i].AddComponent<BoxCollider>();
            box[i].GetComponent<BoxCollider>().isTrigger = true;
			box[i].AddComponent<HoldToScale>();
			box[i].GetComponent<HoldToScale>().Initialize(PlacementMaterial, box, minX, maxX, minY, maxY, minZ, maxZ);
		}
	}
	
	public void DestroyAllObjects()
	{	
		if(box != null)
		{
            contBox.UpdateBoxFromFile(box, currentSequence);

            int i;
			
			for(i = 0; i < box.Length; i++)
			{
				Destroy(box[i]);
			}
			
			box = null;
			
			for(i = 0; i < selectedObjects.Length; i++)
		    {
			    selectedObjects[i].GetComponent<MeshRenderer>().enabled = false;
		    }
		}
	}
	
	private void setIndicationObjects(GameObject[] obj, bool[] in_box)
    {
		if(!contBox.BoxFromFileExists())
		{
			contBox.InitializeBoxFromFile();
		}
		
        box = contBox.GetFromObjects(obj, in_box, currentSequence);
        contBox.getCurrentMinMax(out minX, out maxX, out minY, out maxY, out minZ, out maxZ);
    }

    public void SetBox()
    {
        if(box == null)
        {
            return;
        }

        bool value;

        if (MenuManager.Instance.isMenuOpen())
        {
            value = false;
        }
        else
        {
            value = true;
        }

        for (int i = 0; i < box.Length; i++)
        {
            box[i].SetActive(value);
        }
    }
}
