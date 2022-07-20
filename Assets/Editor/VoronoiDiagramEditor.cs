using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiDiagram))]
public class VoronoiDiagramEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VoronoiDiagram vd = (VoronoiDiagram)target;
        if (GUILayout.Button("Generate"))
        {
            vd.GenerateVoronoiDiagram();
        }
    }
}
