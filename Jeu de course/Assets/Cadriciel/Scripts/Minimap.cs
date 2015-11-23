using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{

    public CustomTerrainScriptAtsV3Snow Terrain;
    public GameObject Cars;
    public int MinicarSize;

    private GUITexture _minimap;
    private Dictionary<CarController, GameObject> _miniCars; 
	
    // Use this for initialization
	void Start ()
	{
	    _minimap = this.GetComponentInChildren<GUITexture>();
        _miniCars = new Dictionary<CarController, GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

	    foreach (var car in Cars.GetComponentsInChildren<CarController>())
	    {
	        if (!_miniCars.ContainsKey(car))
	        {
	            var colorObj = car.gameObject.transform.FindChild("SkyCar").FindChild("vehicle_skyCar_body_paintwork").gameObject;
	            var carColor = colorObj.GetComponent<MeshRenderer>().sharedMaterials[1].color;

                var minicar = new GameObject(car.name);
	            minicar.transform.parent = this.transform;
                minicar.transform.localPosition = new Vector3(0, 0, 2);

                minicar.AddComponent<GUITexture>();
	            minicar.guiTexture.texture = Resources.Load("Textures/minicar") as Texture2D;
	            minicar.guiTexture.color = carColor;
                
                minicar.guiTexture.pixelInset = new Rect(0, 0, MinicarSize, MinicarSize);

                _miniCars[car] = minicar;
            }

            var x = _minimap.pixelInset.x + (car.transform.position.x - Terrain.transform.position.x) / Terrain.terrainSizeX * _minimap.pixelInset.width - MinicarSize / 2;
            var y = _minimap.pixelInset.y + (car.transform.position.z - Terrain.transform.position.z) / Terrain.terrainSizeZ * _minimap.pixelInset.height - MinicarSize / 2;
            _miniCars[car].guiTexture.pixelInset = new Rect(x, y, MinicarSize, MinicarSize);
            
	    }

	}


}
