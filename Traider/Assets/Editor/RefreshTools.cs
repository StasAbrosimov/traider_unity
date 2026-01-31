using UnityEditor;
using UnityEngine;

public class RefreshTools : MonoBehaviour
{
    [MenuItem("AssetRefresh/Force Refresh Asset Database %#&D")]
    public static void RefreshAssetsManually()
    {

        Debug.Log("Refreshing the Asset Database...");
        AssetDatabase.Refresh();
        Debug.Log("Asset Database refresh complete.");

        EditorUtility.RequestScriptReload();
    }

    [MenuItem("AssetRefresh/Force ScriptReload %#&S")]
    public static void RequestScriptReloadManually()
    {

        Debug.Log("Refreshing RequestScriptReload...");
        EditorUtility.RequestScriptReload();
        Debug.Log("Refreshing RequestScriptReload complete...");
    }
}
