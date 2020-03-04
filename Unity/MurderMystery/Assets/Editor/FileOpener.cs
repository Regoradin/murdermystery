using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Callbacks;

[InitializeOnLoad]
public class FileOpener : MonoBehaviour {

    static string _emacsPath = "/usr/bin/emacsclient";
    const string _fileExtensions = ".cs, .txt, .js, .javascript, .json, .html, .shader, .template";
    
    [OnOpenAssetAttribute()]
    public static bool OnOpenedAsset(int instanceID, int line)
    {
	UnityEngine.Object selected = EditorUtility.InstanceIDToObject(instanceID);
	    
	string selectedFilePath = AssetDatabase.GetAssetPath(selected);
	string selectedFileExt = Path.GetExtension(selectedFilePath);
	if (selectedFileExt == null) {
	    selectedFileExt = String.Empty;
	}
	if (!String.IsNullOrEmpty(selectedFileExt)) {
	    selectedFileExt = selectedFileExt.ToLower();
	}

	if (selected.GetType().ToString() == "UnityEditor.MonoScript" ||
	    selected.GetType().ToString() == "UnityEngine.Shader" ||
	    _fileExtensions.IndexOf(selectedFileExt, StringComparison.OrdinalIgnoreCase) >= 0 ) {
        
	    string ProjectPath = System.IO.Path.GetDirectoryName(UnityEngine.Application.dataPath);
	    string completeFilepath = ProjectPath + Path.DirectorySeparatorChar + AssetDatabase.GetAssetPath(selected);
	    string args = null;

	    args = '"' + completeFilepath + '"';
	    if (line != -1){
		args = "-n +" + line.ToString() + " " + args;
	    }

	    
	    System.Diagnostics.Process proc = new System.Diagnostics.Process();
	    proc.StartInfo.FileName = _emacsPath;
	    proc.StartInfo.Arguments = args;
	    proc.StartInfo.UseShellExecute = false;
	    proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
	    proc.StartInfo.CreateNoWindow = true;
	    proc.StartInfo.RedirectStandardOutput = true;
	    proc.Start();


	    return true;
	}else{

	Debug.LogWarning("Letting Unity handle opening of script.");
	return false;
	}
    }
}
