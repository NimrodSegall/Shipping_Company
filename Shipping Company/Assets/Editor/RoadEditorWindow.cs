using UnityEditor;
using UnityEngine;
using RoadTypes;

public class RoadEditorWindow : EditorWindow
{
    [MenuItem("Tools/Road Editor")]
    public static void Open()
    {
        GetWindow<RoadEditorWindow>();
    }

    public Transform roadRoot;
    public float gridSize = 15f;
    public GameObject roadPrefab;
    public GameObject roadPrefabCorner_R;
    public GameObject roadPrefabCorner_L;
    public GameObject roadPrefab_T;
    public GameObject roadPrefab_X;

    private GameObject currentRoadPrefab = null;

    private float rayLength = 15f;

    private int roadLayerMask = 1 << 10;

    private string[] directions = { "forward", "right", "backward", "left" };

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("roadRoot"));
        EditorGUILayout.PropertyField(obj.FindProperty("roadPrefab"));
        EditorGUILayout.PropertyField(obj.FindProperty("roadPrefabCorner_R"));
        EditorGUILayout.PropertyField(obj.FindProperty("roadPrefabCorner_L"));
        EditorGUILayout.PropertyField(obj.FindProperty("roadPrefab_T"));
        EditorGUILayout.PropertyField(obj.FindProperty("roadPrefab_X"));

        if (roadRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            DrawRoadButtons();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawRoadButtons()
    {

        if (GUILayout.Button("Select I Road"))
        {
            currentRoadPrefab = roadPrefab;
        }

        if (GUILayout.Button("Select Corner L"))
        {
            currentRoadPrefab = roadPrefabCorner_L;
        }

        if (GUILayout.Button("Select Corner R"))
        {
            currentRoadPrefab = roadPrefabCorner_R;
        }

        if (GUILayout.Button("Select T"))
        {
            currentRoadPrefab = roadPrefab_T;
        }

        if (GUILayout.Button("Select X"))
        {
            currentRoadPrefab = roadPrefab_X;
        }

        if (GUILayout.Button("Create Road"))
        {
            RoadBase currentRoad = NewRoad(currentRoadPrefab);
            RoadBase nextRoad = FindNextRoadAndSetInLanes(currentRoad);
            if (nextRoad != null)
            {
                currentRoad.ConnectToNextRoad(nextRoad);
            }
        }

        if (Selection.activeGameObject?.GetComponent<IRoadInterface>() != null)
        {
            IRoadInterface currentRoad = Selection.activeGameObject?.GetComponent<IRoadInterface>();
            foreach (string dir in directions)
            {
                if (currentRoad.IsDirectionConnectable(dir) && Selection.activeGameObject?.GetComponent<RoadBase>().createDirection != dir)
                {
                    if(GUILayout.Button("Continue " + dir))
                    {
                        currentRoad.SetCreateDirAndLanesOut(dir);
                    }
                }
            }
        }
    }
    
    private RoadBase NewRoad(GameObject roadPrefab)
    {
        GameObject roadObject = Instantiate(roadPrefab, roadRoot);
        roadObject.name = "Road " + roadRoot.childCount;
        RoadBase prevRoad = GetPrevRoad();
        if (prevRoad != null)
        {
            roadObject.GetComponent<IRoadInterface>().CreateRoad(prevRoad, gridSize);
        }
        Selection.activeGameObject = roadObject;
        return roadObject.GetComponent<RoadBase>();
    }

    private RoadBase GetPrevRoad()
    {
        RoadBase prevRoad = null;
        if (Selection.activeGameObject?.GetComponent<RoadBase>() != null)
        {
            prevRoad = Selection.activeGameObject.GetComponent<RoadBase>();
        }
        else if (roadRoot.childCount > 1)
        {
            prevRoad = roadRoot.GetChild(roadRoot.childCount - 2).gameObject.GetComponent<RoadBase>();
        }
        return prevRoad;
    }

    private RoadBase FindNextRoadAndSetInLanes(RoadBase currentRoad)
    {
        RoadBase nextRoad = null;
        int hitInd = 0;
        RaycastHit[] hits = Physics.RaycastAll(currentRoad.transform.position, RoadBase.DirToVec(currentRoad.createDirection), rayLength, roadLayerMask);
        if (hits.Length > 0)
        {

            if (hits[0].collider.gameObject != currentRoad.gameObject)
            {
                nextRoad = hits[0].collider.GetComponent<RoadBase>();
            }
            else if (hits.Length > 1)
            {
                if (hits[1].collider.gameObject != currentRoad.gameObject)
                {
                    nextRoad = hits[1].collider.GetComponent<RoadBase>();
                }
            }
            if (nextRoad != null)
            {
                Vector3 pointHit = hits[hitInd].point;
                GameObject[] inLanes = FindInLanes(nextRoad, pointHit, currentRoad.lanesOut);
                nextRoad.GetComponent<IRoadInterface>().SetInLanes(inLanes);
            }
        }

        return nextRoad;
    }

    private GameObject[] FindInLanes(RoadBase nextRoad, Vector3 pointHit, GameObject[] currentRoadOutLanes)
    {
        float[] closestDistances = { Mathf.Infinity, Mathf.Infinity };
        GameObject[] nextRoadInLanes = { null, null };
        foreach (GameObject laneObject in nextRoad.lanes)
        {
            float distanceToPoint = (laneObject.transform.position - pointHit).magnitude;
            if (distanceToPoint < closestDistances[0] && laneObject != nextRoadInLanes[1])
            {
                nextRoadInLanes[1] = nextRoad.lanesIn[0];
                nextRoadInLanes[0] = laneObject;
                closestDistances[1] = closestDistances[0];
                closestDistances[0] = distanceToPoint;
            }
            else if (distanceToPoint < closestDistances[1] && laneObject != nextRoadInLanes[0])
            {
                nextRoadInLanes[1] = laneObject;
                closestDistances[1] = distanceToPoint;
            }
        }
        Vector3 posIn0 = nextRoadInLanes[0].transform.position;
        Vector3 posIn1 = nextRoadInLanes[1].transform.position;
        Vector3 posOut0 = currentRoadOutLanes[0].transform.position;
        Vector3 posOut1 = currentRoadOutLanes[1].transform.position;
        // Connect waypoints so sum of distances is minimal
        if (!RoadBase.IsSumOfDistMin(posIn0, posIn1, posOut0, posOut1))
        {
            GameObject swap = nextRoadInLanes[0];
            nextRoadInLanes[0] = nextRoadInLanes[1];
            nextRoadInLanes[1] = swap;
        }
        return nextRoadInLanes;
    }
}

