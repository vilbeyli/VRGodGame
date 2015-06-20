using UnityEngine;
using System.Collections;

public class sculpt : MonoBehaviour {
//brush parameters:
public Transform brush;
private float pull= .40f;
private float cur_pull= 0f;

private  float radius = .3f;
private  float cur_radius = 0f;

public Texture red;
public Texture green;
//
//shaders and planet parameters:
public Shader with_light;
public Shader without_light;
public Shader water_cartoon;

//

//gui changeable values:
private float min_mesh_flatness=2.5f;
private float max_mesh_flatness=8f;
private float max_scale=10f;
private float min_scale=.25f;

private float extra_height=1f;//Start gui values
private float scale_factor=6f;
private float height_factor=1f;
private float current_height_factor=1f;	

private float flatness=2.5f;
private float current_flatness=0f;
private float mesh_flatness=4f;
private float current_mesh_flatness=1f;
//


private MeshFilter unappliedMesh;
private Mesh Collider_Mesh;
private Collider collider;
public Transform planet;//planet
private Transform water;//water
private Transform clouds;//clouds
private Transform glow;	//glow

private Vector3 water_size_initial;
private float pl_rad=0f;//planet basic mesh radius
private bool first_time=false;//is it first click on planet or not
private Vector3 pl_center;//planet center

private float timer=0f;
private Vector3[] array;
private Vector3[] array_n;
//private bool generate=false;
/*private float seedx=0f;//random seeds
private float seedy=0f;
private float seedz=0f;*/

private bool first_time_planet_check=false;
private Vector4 blend1;//planet shader parameters
private Vector4 blend2;
private Vector4 blend3;
private bool mesh_changed=false;
private bool paint =false;
private bool creating_collidrs=false;//while creating colliders	this is true
private float max_height=30f;// checking max height of the mesh(max distance from the center)
private float checking_height=0f;
private float cur_dist=0f;
private float current_max_height=0f;
	
private float smooth_counter=100f;	
	
private Rect windowRect1 = new Rect(10, 10, 200, 300);
private Rect windowRect0 = new Rect(10, 320, 200, 150);
private Rect windowRect2 = new Rect(Screen.width-210, 10, 200, 150);
private Rect windowRect3 = new Rect(Screen.width-210, 200, 200, 150);

//falloff
float  falloff ( float distance  ,   float inRadius  ){
return Mathf.Clamp01(1.0f - distance / inRadius);
}




void Start(){
	/*seedx=Random.Range(1f,1000f);//random seed
	seedy=Random.Range(1f,1000f);
	seedz=Random.Range(1f,1000f);*/
	
	pl_rad=25f;//25 is the basic size of imported mesh. If you are using included planets don't change it. Otherwise, change to your value.
	
	blend1= planet.transform.GetComponent<Renderer>().material.GetVector("_Blend0to1and1to2");//saving current blending settings
	blend2 = planet.transform.GetComponent<Renderer>().material.GetVector("_Blend2to3and3to4");
	blend3 = planet.transform.GetComponent<Renderer>().material.GetVector("_Blend4to5and5to6");
	water =planet.transform.FindChild("water_sphere");//water
	water_size_initial	=water.transform.localScale;

    PlanetParameters();
		

}	



void OnGUI(){
	//gui windows creation
	windowRect0 = GUI.Window(0, windowRect0, DoMyWindow0, "Brush Window");	
//	windowRect1 = GUI.Window(1, windowRect1, DoMyWindow1, "Generate Window");	
	windowRect2 = GUI.Window(2, windowRect2, DoMyWindow2, "Textures Window");	
	//windowRect3 = GUI.Window(3, windowRect3, DoMyWindow3, "Planet Parameters Window");
	
}	

void DoMyWindow0(int windowID) {
	GUI.Label(new Rect(10,20,100,20),"brush force "+(Mathf.Floor(pull*10f)/10f).ToString());		
	pull = GUI.HorizontalSlider(new Rect(10,40,100,20),pull,-.8f,.8f);	
	GUI.Label(new Rect(10,60,100,20),"brush radius "+(Mathf.Floor(radius*10f)/10f).ToString());	
	radius = GUI.HorizontalSlider(new Rect(10,80,100,20),radius,0.05f,.75f);
	paint =GUI.Toggle(new Rect(10,100,150,20),paint,"Toggle to sculpt");	
	GUI.DragWindow(new Rect(0, 0, 10000, 10000)); 
}

void DoMyWindow2(int windowID) {
	
	
	GUI.Label(new Rect(10,20,200,20),"Water_Level "+(Mathf.Floor(height_factor*100f)/100f).ToString());	
	height_factor = GUI.HorizontalSlider(new Rect(10,40,150,20),height_factor,.9f,1.1f);	
	
	GUI.Label(new Rect(10,60,200,20),"Textures Junction "+(Mathf.Floor(flatness*10f)/10f).ToString());	
	flatness = GUI.HorizontalSlider(new Rect(10,80,150,20),flatness,.5f,10f);	

}
	//gui windows creation ends
	

void PlanetParameters(){
		//setting up planet parameters based on chosen shader
			planet.GetComponent<Renderer>().material.shader=without_light;
			water.GetComponent<Renderer>().material.shader=water_cartoon;	
			mesh_flatness=1f;
			min_mesh_flatness=.5f;
			max_mesh_flatness=2f;
			scale_factor=1f;
			min_scale=.25f;
			max_scale=4f;
}
	
//Main mesh deforming function	
void  DeformMesh ( Mesh mesh ,   Vector3 position ,   float power ,   float inRadius  ){

	Vector3[] vertices= mesh.vertices;
	Vector3[] normals= mesh.normals;
	float sqrRadius= inRadius * inRadius;
	if (!first_time){
		first_time=true;
		pl_rad=(pl_center -vertices[0]).magnitude;//checking actual planet radius
		max_height=pl_rad;
		array = new Vector3[vertices.Length];//saving sphere vertices positions
		array_n = new Vector3[vertices.Length];
		for (int i=0;i<vertices.Length;i++)
		{
			array[i]=vertices[i];
		}
		for (int i=0;i<vertices.Length;i++)
		{
			array_n[i]=normals[i];
		}
	}
	// Calculate averaged normal of all surrounding vertices	
	Vector3 averageNormal= Vector3.zero;
	for (int i=0;i<vertices.Length;i++)
	{
		
		float sqrMagnitude= (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;
		
		if ((pl_center -vertices[i]).sqrMagnitude>(checking_height)){
			checking_height=(pl_center -vertices[i]).sqrMagnitude;
		}	
		float distance= Mathf.Sqrt(sqrMagnitude);
		
		
		
		averageNormal += falloff(distance, inRadius) * normals[i];
	}
	averageNormal = averageNormal.normalized;
	
	// Deform vertices along averaged normal
	for (int i=0;i<vertices.Length;i++)
	{
	
		float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
		// Early out if too far away
		if (sqrMagnitude > sqrRadius)
			continue;
			
			float	distance = Mathf.Sqrt(sqrMagnitude);
		if (((array[i]-vertices[i]).sqrMagnitude<(pl_rad*pl_rad)*.035f/(mesh_flatness*mesh_flatness) || (array[i].sqrMagnitude-vertices[i].sqrMagnitude)>0f && pull>0f)|| (pull<0f) ){//limiting max height of mountains
			vertices[i] += averageNormal * falloff(distance, inRadius)* power/(mesh_flatness);
		}
	}
	if (checking_height>Mathf.Sqrt(current_max_height)){
		max_height=Mathf.Sqrt(checking_height);//new max height
	}
	mesh.vertices = vertices;
	
	StopCoroutine("recalculate");//stoping all recalculating coroutines if any are in process
	StartCoroutine("recalculate" , mesh);//starting a new corouting (recalculating and smoothing normals)
	mesh.RecalculateBounds();
	
}

void Planet_data(RaycastHit hit){
//getting data after hitting the planet. You can use it if you want to have multiple planets in editor window 
	planet=hit.transform;
	if (!first_time_planet_check){
		first_time_planet_check=true;
		clouds =planet.transform.FindChild("clouds_sphere").transform;
		glow =planet.transform.FindChild("Glow").transform;
		blend1= planet.transform.GetComponent<Renderer>().material.GetVector("_Blend0to1and1to2");
		blend2 = planet.transform.GetComponent<Renderer>().material.GetVector("_Blend2to3and3to4");
		blend3 = planet.transform.GetComponent<Renderer>().material.GetVector("_Blend4to5and5to6");
		water =hit.transform.FindChild("water_sphere");
		water_size_initial	=water.transform.localScale;
	}
}


void height_texture_set(){//setting shader parameters. (Basically applying texture junction, mesh flatness and water height values)
	if (planet!=null && (current_height_factor!=height_factor || current_flatness!=flatness)){
		current_height_factor=height_factor;
		current_flatness=flatness;
		
		Vector4 pl_rad4= new Vector4(pl_rad,pl_rad,pl_rad,pl_rad);
		planet.transform.GetComponent<Renderer>().material.SetVector("_Blend0to1and1to2",(pl_rad4-(pl_rad4-blend1)/flatness)*height_factor);
		planet.transform.GetComponent<Renderer>().material.SetVector("_Blend2to3and3to4",(pl_rad4-(pl_rad4-blend2)/flatness)*height_factor);	
		//planet.transform.GetComponent<Renderer>().material.SetVector("_Blend4to5and5to6",(pl_rad4-(pl_rad4-blend3)/flatness)*height_factor);	
		water.transform.localScale=(new Vector3(1f,1f,1f)-(new Vector3(1f,1f,1f)-water_size_initial)/flatness)*height_factor;
		
	}
	
	}	
	
	
void brush_set(){//setting the distance from brush projector to planet based on current planet radius
	if (cur_radius!=radius){
		cur_radius=radius;
		brush.GetComponent<Projector>().orthographicSize = .83f*cur_radius*planet_control.size_public*20f;
	}	
	
	if (cur_pull!=pull){
		cur_pull=pull;
		if (cur_pull>=0f){
			brush.GetComponent<Projector>().material.SetTexture("_ShadowTex",green);
		
		} else {
		
			brush.GetComponent<Projector>().material.SetTexture("_ShadowTex",red);
		}
		
	}	
}	

void  Update (){
	height_texture_set();	
	brush_set();
	
	RaycastHit hit_brush;
	Ray ray_brush= Camera.main.ScreenPointToRay(Input.mousePosition);
	
	if (Physics.Raycast (ray_brush, out hit_brush))// positioning the brush based on a raycast hit
	{
		brush.transform.position = hit_brush.point;
		brush.transform.position += -(planet.position-hit_brush.point).normalized*pl_rad*planet_control.size_public;	
		brush.LookAt(hit_brush.point);
	}
	// When no button is pressed and paint and generate toggle btns are not active we update the mesh collider if the mesh was changed
	if (!Input.GetMouseButton (0))
	{		
		// Apply collision mesh when we let go of button
		if (mesh_changed){
		mesh_changed=false;
		StartCoroutine(ApplyMeshCollider());
		}	
	
		return;
	}
	
	// Planet deforming raycast. Checking did we hit the surface ot not
	RaycastHit hit;
	Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
	
	
	if (Physics.Raycast (ray, out hit))
	{
		MeshFilter filter = hit.collider.GetComponent<MeshFilter>();
		collider = hit.collider;	
		
		if (filter)
		{
			if (filter!= unappliedMesh)
			{
			unappliedMesh = filter;
			}
			
			mesh_changed=true;
			    //deform the planet with brush
				Vector3 relativePoint= filter.transform.InverseTransformPoint(hit.point);
				if (Time.deltaTime<.1f)
                {//used to prevent framerate drop on old devices
					DeformMesh(filter.mesh, relativePoint, pull*pl_rad * Time.deltaTime, radius*pl_rad);
				}
		}
	}

}


IEnumerator   ApplyMeshCollider (){
		this.transform.FindChild("Notification").GetComponent<Renderer>().enabled=true;//showing notification
		yield return .1f;
	unappliedMesh.GetComponent<MeshCollider>().sharedMesh = null;
		unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.mesh;
		this.transform.FindChild("Notification").GetComponent<Renderer>().enabled=false;
	yield return 0;

}



IEnumerator recalculate(Mesh mesh){
		
	mesh.RecalculateNormals();
	Vector3[] vertices=mesh.vertices;
	Vector3[] normals = mesh.normals;
	
	int count=0;
		// normals smoothing. Used to make seams(planet is a deformed cube) invisible->
	for (int i = 0; i < vertices.Length; i++){
		Vector3 averageNormal= Vector3.zero;
		float inRadius=.1f;
		float sqrRadius=inRadius*inRadius;
		count++;
		smooth_counter=(float)i/(float)vertices.Length;
			
		if (count>20){
			count=0;
			
			yield return 0;
		}
		
		for (int j=0;j<vertices.Length;j++)
		{
		
			float sqrMagnitude= (vertices[j] - vertices[i]).sqrMagnitude;
			// Early out if too far away
			if (sqrMagnitude > sqrRadius){
			continue;
			}
			
			
			float distance= Mathf.Sqrt(sqrMagnitude);
			
			averageNormal += falloff(distance, inRadius) * normals[j];
			
			
		}
		averageNormal = averageNormal.normalized;
		
		normals[i] = averageNormal;
	}
	mesh.normals = normals;

}

/*void Generate(Mesh mesh){//generating mesh 
	
	checking_height=0f;
	max_height=0f;
	int cur_scale_size=planet_control.size_public;	
	Vector3[] vertices= mesh.vertices;
	Vector3[] normals= mesh.normals;
	
	
	
	
	
	generate=false;
	seedx=Random.Range(1f,1000f);
	seedy=Random.Range(1f,1000f);
	seedz=Random.Range(1f,1000f);
	
	
	if (!first_time){
		first_time=true;
		pl_rad=(pl_center -vertices[0]).magnitude;
		max_height=pl_rad;
		array = new Vector3[vertices.Length];
		array_n = new Vector3[vertices.Length];
			for (int i=0;i<vertices.Length;i++)
			{
				array[i]=vertices[i];
			}
			for (int i=0;i<vertices.Length;i++)
			{
				array_n[i]=normals[i];
			}
	
	}
	
	
	for (int i=0;i<vertices.Length;i++)
	{
	
		vertices[i]= array[i];
		//calculating point heights based on scale and vertex position using perlin noise. Each height var represents an octave.
		float height =Mathf.PerlinNoise(vertices[i].x*scale_factor/(pl_rad)+seedx,vertices[i].y*scale_factor/(pl_rad)+seedy)+Mathf.PerlinNoise(vertices[i].y*scale_factor/(pl_rad)+seedy,vertices[i].z*scale_factor/(pl_rad)+seedz)+Mathf.PerlinNoise(vertices[i].x*scale_factor/(pl_rad)+seedx,vertices[i].z*scale_factor/(pl_rad)+seedz);
		float height2 =Mathf.PerlinNoise(vertices[i].x*2f*scale_factor/(pl_rad)+seedx,vertices[i].y*2f*scale_factor/(pl_rad)+seedy)+Mathf.PerlinNoise(vertices[i].y*2f*scale_factor/(pl_rad)+seedy,vertices[i].z*2f*scale_factor/(pl_rad)+seedz)+Mathf.PerlinNoise(vertices[i].x*2f*scale_factor/(pl_rad)+seedx,vertices[i].z*2f*scale_factor/(pl_rad)+seedz);
		float height3 =Mathf.PerlinNoise(vertices[i].x*4f*scale_factor/(pl_rad)+seedx,vertices[i].y*4f*scale_factor/(pl_rad)+seedy)+Mathf.PerlinNoise(vertices[i].y*4f*scale_factor/(pl_rad)+seedy,vertices[i].z*4f*scale_factor/(pl_rad)+seedz)+Mathf.PerlinNoise(vertices[i].x*4f*scale_factor/(pl_rad)+seedx,vertices[i].z*4f*scale_factor/(pl_rad)+seedz);
		float height4 =Mathf.PerlinNoise(vertices[i].x*8f*scale_factor/(pl_rad)+seedx,vertices[i].y*8f*scale_factor/(pl_rad)+seedy)+Mathf.PerlinNoise(vertices[i].y*8f*scale_factor/(pl_rad)+seedy,vertices[i].z*8f*scale_factor/(pl_rad)+seedz)+Mathf.PerlinNoise(vertices[i].x*8f*scale_factor/(pl_rad)+seedx,vertices[i].z*8f*scale_factor/(pl_rad)+seedz);
		float height5 =Mathf.PerlinNoise(vertices[i].x*16f*scale_factor/(pl_rad)+seedx,vertices[i].y*16f*scale_factor/(pl_rad)+seedy)+Mathf.PerlinNoise(vertices[i].y*16f*scale_factor/(pl_rad)+seedy,vertices[i].z*16f*scale_factor/(pl_rad)+seedz)+Mathf.PerlinNoise(vertices[i].x*16f*scale_factor/(pl_rad)+seedx,vertices[i].z*16f*scale_factor/(pl_rad)+seedz);
		
		//combining all the heights
		vertices[i] += (array_n[i]*(.15f*extra_height-height/10f)+array_n[i]*(.15f*extra_height-height2/10f)/2f+array_n[i]*(.15f*extra_height-height3/10f)/(4f)+array_n[i]*(.15f*extra_height-height4*height4/10f)/(8f)+array_n[i]*(.15f*extra_height-height5*height5/10f)/(16f))*pl_rad/mesh_flatness;
		
		if ((pl_center -vertices[i]).sqrMagnitude>(checking_height)){
			checking_height=(pl_center -vertices[i]).sqrMagnitude;
		}	
		
	
	}
	max_height=Mathf.Sqrt(checking_height);

	
	mesh.vertices = vertices;
	StopCoroutine("recalculate");//stoping all coroutines if there are any coroutines active
	StartCoroutine("recalculate" , mesh); // start a new corouting
	mesh.RecalculateBounds();

}	*/





}