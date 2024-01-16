using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class C_EditorWindows : Editor
{
	
	[MenuItem("Stunt Crasher/Edit Settings", false, -100)]
	public static void OpenSettings(){
		Selection.activeObject = C_Settings.Instance;
	}

	[MenuItem("Stunt Crasher/Player Vehicles", false, -95)]
	public static void OpenPlayerVehicles(){
		Selection.activeObject = C_PlayerVehicles.Instance;
	}

	[MenuItem("Stunt Crasher/Player Vehicles Folder", false, -90)]
	public static void OpenPlayerVehiclesFolder(){
		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath ("Assets/StuntCrasher/Resources/Vehicles");
	}

	[MenuItem("Stunt Crasher/Other Prefabs Folder", false, -80)]
	public static void OpenPrefabsFolder(){
		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath ("Assets/StuntCrasher/Resources/Prefabs/Props");
	}

	[MenuItem("Stunt Crasher/Documentation", false, 100)]
	public static void OpenDocumentation(){
		string url = "http://www.bonecrackergames.com/stunt-crasher/";
		Application.OpenURL(url);
	}

}
