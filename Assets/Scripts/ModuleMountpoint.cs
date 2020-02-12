using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(ModuleMountpoint))]
[CanEditMultipleObjects]
public class ModuleMountpointEditor : Editor
{
    SerializedProperty transform;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        if (GUILayout.Button("Align"))
        {
            ModuleMountpoint mp = target as ModuleMountpoint;
            mp.SnapToClosestSurface();
        }
        //EditorGUILayout.Toggle("Align", false);
        //EditorGUILayout.PropertyField(lookAtPoint);
        //serializedObject.ApplyModifiedProperties();
    }
}

public class ModuleMountpoint : MonoBehaviour
{
    public string model;
    protected void Start()
    {
        UpdateModel(model, true);
    }

    public void UpdateModel(string newModelPath, bool force = false)
    {
        if (!force && model == newModelPath)
            return;

        GameObject prefab = (GameObject)Resources.Load("Prefabs/Modules/" + newModelPath);
        if (prefab == null)
        {
            return;
        }

        model = newModelPath;
        Transform child = transform.Find("model");
        if (child != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }

        if (prefab != null)
        {
            GameObject model = Instantiate(prefab, transform);
            model.transform.name = "model";
        }
    }

    public void SnapToClosestSurface()
    {

        List<Vector3> directions = new List<Vector3> { transform.up, -transform.up, transform.right, -transform.right, transform.forward, -transform.forward };
        List<RaycastHit> hits = directions.ConvertAll<RaycastHit>(direction =>
        {
            RaycastHit hit;
            Physics.Raycast(transform.position - direction * 0.01f, direction, out hit, 1f);
            return hit;
        });

        hits.Sort((a, b) =>
        {
            if (a.normal == Vector3.zero)
            {
                return 1;
            }
            if (b.normal == Vector3.zero)
            {
                return -1;
            }
            return b.distance.CompareTo(a.distance);
        });
        if(hits[0].normal != Vector3.zero)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hits[0].normal);
        }

    }
}
