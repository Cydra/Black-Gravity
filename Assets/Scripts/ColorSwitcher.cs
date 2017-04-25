using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour {
	private Light light;


	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		GameObject obj = col.gameObject;
		if(obj.tag == "environment"){
			Material mat = obj.GetComponent<Renderer> ().material;
			Color color = mat.color;

			// Increase the saturation
			float H, S, V;
			Color.RGBToHSV (color, out H, out S, out V);
			S *= 3.0f;
			color = Color.HSVToRGB (H, S, V);

			light.color = color;
		}
	}
}
