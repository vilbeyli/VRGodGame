using UnityEngine;
using System.Collections;

public class planet_control : MonoBehaviour {
	public Transform planet;
	public  int size=1;//planet scale
	public static int size_public=0;
	public static float glow_value_public=1f;//glow size value for public use
		private float rota_dir=0f;
	private Vector3 startPosCam;
	private Vector3 startPosBrush;
	private Transform glow;
	private Transform brush;
	// Use this for initialization
	void Awake () {
		size_public=size;
			brush = GameObject.Find("brush").transform;
	glow= planet.FindChild("Glow");
	startPosCam = this.transform.position;
	//startPosBrush = brush.transform.position;
			if (planet.transform.localScale.x!=size){
			
			planet.transform.localScale = new Vector3(size,size,size);
			this.transform.position = (startPosCam-planet.transform.position)*size;
			
			//brush.transform.position = (startPosBrush-planet.transform.position)*size;
			
			
			planet.GetComponent<Renderer>().material.SetFloat("_Radius",1f/size);
			glow.GetComponent<Renderer>().material.SetFloat("_Radius",1f/size);
			glow.localScale=new Vector3(1f/size,1f/size,1f/size);
			glow_value_public=25f*(size-1)+1f*size;
			glow.GetComponent<Renderer>().material.SetFloat("_Size",glow_value_public);
		}	
	
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		
		
	this.transform.position -= Input.GetAxis("Mouse ScrollWheel")*(this.transform.position-planet.transform.position).normalized*10f*planet_control.size_public;
	this.transform.RotateAround(planet.transform.position,Vector3.up,Input.GetAxis("Horizontal"));
	this.transform.RotateAround(planet.transform.position,Vector3.forward,-Input.GetAxis("Vertical"));
		
	}
}
