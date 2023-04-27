using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportTexture : EditorWindow
{
    //目标texture
    private Texture2D _select;

    [MenuItem("Tools/导出Texture2D")]
    static void Export()
    {
        GetWindow(typeof(ExportTexture), false, "导出Texture2D");
    }

    private void OnGUI()
    {
        _select = EditorGUILayout.ObjectField(_select, typeof(Texture2D), true) as Texture2D;
        if (GUILayout.Button("导出"))
        {
            if (_select == null)
            {
                EditorUtility.DisplayDialog("导出Texture2D", "导出失败", "OK");
                return;
            }

            string path =
                EditorUtility.SaveFilePanel("Save Sample", "", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), "png");

            var tmp = RenderTexture.GetTemporary(_select.width, _select.height, 0, RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Linear);
            Graphics.Blit(_select, tmp);
            var preActive = RenderTexture.active;
            RenderTexture.active = tmp;
            var texture = new Texture2D(_select.width, _select.height);
            texture.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            texture.Apply();
            RenderTexture.active = preActive;
            RenderTexture.ReleaseTemporary(tmp);
            var bytes = texture.EncodeToPNG();
            var file = File.Open(path, FileMode.Create);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            file.Close();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("导出Texture2D","导出完成","OK");
        }
    }
}