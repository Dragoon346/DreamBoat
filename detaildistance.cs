using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class detaildistance : MonoBehaviour
{

    public float distance = 1000; // 250 is max in terrain settings, but not here
    public float densityDistance = 1000; // 250 is max in settings.
    Terrain terrain;//referencing the terrain

    void Start()
    {
        terrain = GetComponent<Terrain>();//is object terrain
        if (terrain == null)//if object isnt terrain
        {
            Debug.LogError("This gameobject is not terrain, disabling forced details distance", gameObject);//warning message
            this.enabled = false;//disables script
            return;//reruns code
        }

    }

    // WARNING: this runs update loop inside editor, you dont need this if you dont change the value
    void Update()//loops script execution
    {
        terrain.detailObjectDistance = distance;//detaildistance is set equal to distance variable
        terrain.detailObjectDensity = densityDistance;//detail density is set to densityDistance variable.
    }
}