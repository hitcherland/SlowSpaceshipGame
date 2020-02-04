using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class CreateProtobufs : MonoBehaviour
{
    [MenuItem("Assets/Create/Protobuf")]
    static void CreateProtobuf()
    {
        string basePath = AssetDatabase.GUIDToAssetPath(UnityEditor.Selection.assetGUIDs[0]);
        FileStream fs = File.Create(basePath + "/default.proto");
        byte[] bytes = Encoding.UTF8.GetBytes("syntax = \"proto3\";\npackage GameProtobufs;\n\nmessage DefaultMessage {\n}");
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }
}