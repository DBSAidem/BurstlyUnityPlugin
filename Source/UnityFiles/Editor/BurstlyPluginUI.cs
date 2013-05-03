using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class BurstlyPluginUI : AssetPostprocessor {

	// The AndroidManifest.xml file in Assets/Plugins/Android *must* have the correct package ID otherwise the app will not run, so this
	// method checks to see if that file has been updated every time any assets are imported (changed or added). If the manifest has been
	// changed, then we automatically 'fix' it to make sure the app runs.
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
		foreach (string str in importedAssets) {
			if (str == "Assets/Plugins/Android/AndroidManifest.xml") {
				Debug.Log("Manifest changed! Ensuring that the package ID is correct.");
				
				/*
					Sample manifest element below. The problem with parsing this properly is that using System.Xml adds 1 MB to the game executable. 
					Instead, we're going to parse off of 'package="' and find the closing '"', and replace that package name with the appropriate
					package name from PlayerSettings.bundleIdentifier. This obviously has the danger that it could potentially break something with 
					that exact signature... but since it's XML, any double quotes have to be escaped, so the only danger is if another element has
					a package attribute (unlikely).
					
					<manifest
					    xmlns:android="http://schemas.android.com/apk/res/android"
					    package="com.iUnity.angryBots"
						android:installLocation="preferExternal"
					    android:versionCode="1"
					    android:versionName="1.0">
				 */
				 
				string manifestContents = System.IO.File.ReadAllText(Application.dataPath + "/Plugins/Android/AndroidManifest.xml");
				int iPackageStart = manifestContents.IndexOf("package=\"");
				string manifestBeginning = manifestContents.Substring(0, iPackageStart) + "package=\"";
				string manifestLeftovers = manifestContents.Substring(iPackageStart + 9);
				string manifestEnd = manifestLeftovers.Substring(manifestLeftovers.IndexOf("\""));
				manifestContents = manifestBeginning + PlayerSettings.bundleIdentifier + manifestEnd;
				System.IO.File.WriteAllText(Application.dataPath + "/Plugins/Android/AndroidManifest.xml", manifestContents);
				
				break;
			}
		}
	}

	// Runs the Burstly PostProcessBuildPlayer script - this is needed because existing projects may have a script already defined
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {	
		string deploymentPlatform = "";
		if (target == BuildTarget.Android) deploymentPlatform = "android";
		if (target == BuildTarget.iPhone) deploymentPlatform = "iPhone";
		
		if (deploymentPlatform == "") return;
		
		System.Diagnostics.Process p = new System.Diagnostics.Process();
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.FileName = Application.dataPath + "/Editor/PostProcessBuildPlayer-Burstly";
		//p.StartInfo.Arguments = "'" + pathToBuiltProject + "' '" + deploymentPlatform + "' '" + EditorPrefs.GetString("AndroidSdkRoot") + "' '" + PlayerSettings.Android.keystoreName + "' '" + PlayerSettings.Android.keystorePassword + "' '" + PlayerSettings.Android.keyaliasName + "' '" + PlayerSettings.Android.keyaliasPassword + "'";
		p.StartInfo.Arguments = "'" + pathToBuiltProject + "' '" + deploymentPlatform + "' '" + EditorPrefs.GetString("AndroidSdkRoot") + "'";
		p.Start();
		string output = p.StandardOutput.ReadToEnd();
		p.WaitForExit();
		Debug.Log(output);
	}
}
