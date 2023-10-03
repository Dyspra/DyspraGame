using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Uitility.VittorCloud
{
    public class Shortcut : Editor
    {

        [MenuItem("Tools/ActiveToggle _`")]
        static void ToggleActivationSelection()
        {
            try
            {
                var go = Selection.activeGameObject;
                go.SetActive(!go.activeSelf);
            }
            catch (System.Exception asd) { }
        }


        [MenuItem("Tools/Clear Console _]")]
        static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }


        [MenuItem("Tools/Clear PlayerFrebs _#]")]
        static void ClearPlayerFrebs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
