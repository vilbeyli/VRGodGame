using UnityEditor;
using UnityEngine;
  

// Creates a prefab from a selected game object.

class prefab_saver 
{
    const string menuName = "GameObject/Create Prefab From Selected";
	public static int number =0;
   
    // Adds a menu named "Create Prefab From Selected" to the GameObject menu.
   
    [MenuItem(menuName)]
    static void CreatePrefabMenu ()
    {
        var go = Selection.activeGameObject;
  

		Mesh m1 = go.GetComponent<MeshFilter>().mesh;//getting planet mesh
		Mesh new_Mesh = new Mesh();
		new_Mesh.vertices = m1.vertices;
		new_Mesh.uv = m1.uv;
		new_Mesh.triangles = m1.triangles;
		new_Mesh.normals = m1.normals;
		new_Mesh.tangents =	m1.tangents;
		Debug.Log(m1+prefab_saver.number.ToString());
		AssetDatabase.CreateAsset(new_Mesh, "Assets/Resources/savedMesh/" + go.name + prefab_saver.number.ToString() +"_M" + ".asset"); // saving mesh as asset
  
  
		var prefab =PrefabUtility.CreateEmptyPrefab("Assets/Resources/savedMesh/" + go.name + prefab_saver.number.ToString()+ ".prefab");//creating empty prefab
		var material_pl = go.GetComponent<Renderer>().material;
		var material_gl = go.transform.FindChild("Glow").GetComponent<Renderer>().material;
		var material_cl = go.transform.FindChild("clouds_sphere").GetComponent<Renderer>().material;
		var material_w = go.transform.FindChild("water_sphere").GetComponent<Renderer>().material;
		
		
		AssetDatabase.CreateAsset(material_pl, "Assets/Resources/savedMesh/mat_pl_"+go.name + prefab_saver.number.ToString()+".mat");//saving materials
		AssetDatabase.CreateAsset(material_gl, "Assets/Resources/savedMesh/mat_gl_"+go.name + prefab_saver.number.ToString()+".mat");//saving materials
		AssetDatabase.CreateAsset(material_cl, "Assets/Resources/savedMesh/mat_cl_"+go.name + prefab_saver.number.ToString()+".mat");//saving materials
		AssetDatabase.CreateAsset(material_w, "Assets/Resources/savedMesh/mat_w_"+go.name + prefab_saver.number.ToString()+".mat");//saving materials
		go.GetComponent<MeshFilter>().mesh = new_Mesh;
		go.GetComponent<Renderer>().material=Resources.Load("savedMesh/mat_pl_"+go.name + prefab_saver.number.ToString()) as Material;//loading materials and assigning them to a prefab
		go.transform.FindChild("Glow").GetComponent<Renderer>().material=Resources.Load("savedMesh/mat_gl_"+go.name + prefab_saver.number.ToString()) as Material;//loading materials and assigning them to a prefab
		go.transform.FindChild("clouds_sphere").GetComponent<Renderer>().material=Resources.Load("savedMesh/mat_cl_"+go.name + prefab_saver.number.ToString()) as Material;//loading materials and assigning them to a prefab
		go.transform.FindChild("water_sphere").GetComponent<Renderer>().material=Resources.Load("savedMesh/mat_w_"+go.name + prefab_saver.number.ToString()) as Material;//loading materials and assigning them to a prefab
		
		PrefabUtility.ReplacePrefab(go, prefab);
		
	


        AssetDatabase.Refresh();
	
		prefab_saver.number++;
    }
  

    // Validates the menu.
    // The item will be disabled if no game object is selected.

    // <returns>True if the menu item is valid.</returns>
    [MenuItem(menuName, true)]
    static bool ValidateCreatePrefabMenu ()
    {
        return Selection.activeGameObject != null;
    }
	
}