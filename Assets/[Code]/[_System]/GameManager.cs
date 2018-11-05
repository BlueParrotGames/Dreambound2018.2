using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    private Camera cam;
    bool camset = false;

    public static bool useController;
    [SerializeField] Toggle toggle;

    [Header("Parents")]
    [SerializeField] Transform floraParent;


    private void Start()
    {
        if (!Physics.autoSimulation)
            Physics.autoSimulation = true;

        //if(toggle != null)
        //    useController = toggle.isOn;
        
    }

    public void CleanScene()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Flora"))
        {
            g.transform.parent = floraParent;
        }
    }

    public void SetCamera()
    {
        camset = true;
        cam = Camera.main;

        cam.fieldOfView = 65f;
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;
        //cam.gameObject.AddComponent<PostProcessVolume>();
    }

}
