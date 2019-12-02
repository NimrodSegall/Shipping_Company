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

    [SerializeField]
    private GameObject dataPrefab = null;
    [SerializeField]
    private Transform roadRoot = null;


    private float gridSize = GameParameters.gridSize;
    private GameObject roadPrefab, roadPrefabCorner_R, roadPrefabCorner_L, roadPrefab_T, roadPrefab_X;

    private Texture2D roadButtonTexture, roadButtonRTexture, roadButtonLTexture, roadButtonTTexture, roadButtonXTexture;
    private Texture2D arrowUpTexture, arrowRightTexture, arrowDownTexture, arrowLeftTexture;
    private Texture2D arrowUpTexture_G, arrowRightTexture_G, arrowDownTexture_G, arrowLeftTexture_G;
    private Texture2D clockwiseText, counterClockwiseText;

    private GameObject currentRoadPrefab = null;

    private float rayLength = 15f;

    private int roadLayerMask = 1 << 10;

    private int tab = 1;
    private bool selectAndBuild = true;

    private string currentOrientation = "forward";

    private void OnGUI()
    {
        LoadDataFromPrefab();
        string[] tabNames = { "Input", "Editor" };
        tab = GUILayout.Toolbar(tab, tabNames);
        switch(tab)
        {
            case 0:
                SerializedObject obj = new SerializedObject(this);
                EditorGUILayout.PropertyField(obj.FindProperty("roadRoot"));
                EditorGUILayout.PropertyField(obj.FindProperty("dataPrefab"));
                obj.ApplyModifiedProperties();

                EditorGUILayout.BeginHorizontal("box");
                DrawEditorOption();
                EditorGUILayout.EndHorizontal();
                break;

            case 1:
                if (roadRoot == null)
                {
                    EditorGUILayout.HelpBox("Root transform must be selected. Please assign root transform", MessageType.Warning);
                }
                else if(dataPrefab == null)
                {
                    EditorGUILayout.HelpBox("Data prefab is missing, please assign data prefab", MessageType.Warning);
                }
                else
                {
                    
                    EditorGUILayout.BeginHorizontal("box");
                    DrawRoadSelectionButtons();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginVertical("box");
                    DrawRoadContinueButtons();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginHorizontal();
                    DrawRotationButtons();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginVertical("box");
                    DrawRoadCreationButtons();
                    EditorGUILayout.EndVertical();
                }
                break;

        }



    }

    private void DrawEditorOption()
    {
        selectAndBuild = GUILayout.Toggle(selectAndBuild, "Select & Build");
    }

    private void DrawRoadSelectionButtons()
    {
        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonTexture, RoadBase.DirToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab;
            if(selectAndBuild)
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonRTexture, RoadBase.DirToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefabCorner_R;
            if (selectAndBuild)
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonLTexture, RoadBase.DirToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefabCorner_L;
            if (selectAndBuild)
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonTTexture, RoadBase.DirToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab_T;
            if (selectAndBuild)
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonXTexture, RoadBase.DirToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab_X;
            if (selectAndBuild)
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

    }

    private void DrawRoadContinueButtons()
    {
        Texture2D upButton = arrowUpTexture;
        Texture2D rightButton = arrowRightTexture;
        Texture2D downButton = arrowDownTexture;
        Texture2D leftButton = arrowLeftTexture;
        if (Selection.activeGameObject?.GetComponent<IRoadInterface>() != null)
        {
            IRoadInterface currentRoad = Selection.activeGameObject?.GetComponent<IRoadInterface>();
            string dir;
            bool isDisabled = false;

            GUILayout.BeginHorizontal();
            dir = "forward";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if(Selection.activeGameObject?.GetComponent<RoadBase>() != null)
            {
                if(Selection.activeGameObject.GetComponent<RoadBase>().createDirection == dir)
                {
                    upButton = arrowUpTexture_G; 
                }
            }
            else
            {
                upButton = arrowUpTexture;
            }
            if (GUILayout.Button(upButton))
            {
                currentRoad.SetCreateDirection(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            dir = "left";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (Selection.activeGameObject?.GetComponent<RoadBase>() != null)
            {
                if (Selection.activeGameObject.GetComponent<RoadBase>().createDirection == dir)
                {
                    leftButton = arrowLeftTexture_G;
                }
            }
            else
            {
                leftButton = arrowLeftTexture;
            }
            if (GUILayout.Button(leftButton))
            {
                currentRoad.SetCreateDirection(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();

            dir = "right";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (Selection.activeGameObject?.GetComponent<RoadBase>() != null)
            {
                if (Selection.activeGameObject.GetComponent<RoadBase>().createDirection == dir)
                {
                    rightButton = arrowRightTexture_G;
                }
            }
            else
            {
                rightButton = arrowRightTexture;
            }
            if (GUILayout.Button(rightButton))
            {
                currentRoad.SetCreateDirection(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            dir = "backward";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (Selection.activeGameObject?.GetComponent<RoadBase>() != null)
            {
                if (Selection.activeGameObject.GetComponent<RoadBase>().createDirection == dir)
                {
                    downButton = arrowDownTexture_G;
                }
            }
            else
            {
                downButton = arrowDownTexture;
            }
            if (GUILayout.Button(downButton))
            {
                currentRoad.SetCreateDirection(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button("Reset Direction"))
            {
                currentOrientation = "forward";
            }
        }
    }

    private void DrawRoadCreationButtons()
    {
        if (!selectAndBuild)
        {
            if (GUILayout.Button("Create Road"))
            {
                NewRoadButtonCallback(currentRoadPrefab, null, null, null);
            }
        }

        if(Selection.activeGameObject?.GetComponent<RoadBase>() != null)
        {
            if (GUILayout.Button("Delete Road"))
            {
                DestroyImmediate(Selection.activeGameObject);
            }
        }
    }

    private void DrawRotationButtons()
    {
        EditorGUI.BeginDisabledGroup(Selection.activeGameObject?.GetComponent<RoadT>() == null);
        if (GUILayout.Button(counterClockwiseText))
        {
            RoadBase currentlySelectedRoad = Selection.activeGameObject.GetComponent<RoadBase>();
            string newRotation = RoadBase.RotateClockwise(currentlySelectedRoad.orientation, 3);
            Vector3[] currentPos = { currentlySelectedRoad.transform.position };
            string currentName = currentlySelectedRoad.name;
            DestroyImmediate(currentlySelectedRoad.gameObject);
            NewRoadButtonCallback(roadPrefab_T, newRotation, currentPos, currentName);
        }

        if (GUILayout.Button(clockwiseText))
        {
            RoadBase currentlySelectedRoad = Selection.activeGameObject.GetComponent<RoadBase>();
            string newRotation = RoadBase.RotateClockwise(currentlySelectedRoad.orientation, 1);
            Vector3[] currentPos = { currentlySelectedRoad.transform.position };
            string currentName = currentlySelectedRoad.name;
            DestroyImmediate(currentlySelectedRoad.gameObject);
            NewRoadButtonCallback(roadPrefab_T, newRotation, currentPos, currentName);
        }
        EditorGUI.EndDisabledGroup();
    }

    // Using Vector3[] array since Vector3 is not nullable
    private void NewRoadButtonCallback(GameObject currentRoadPrefab, string newOrientation, Vector3[] newPosVec, string newName)
    {
        RoadBase currentRoad = NewRoad(currentRoadPrefab, newOrientation, newPosVec, newName);
        RaycastHit[] nearbyRoadHits = FindNearbyRoads(currentRoad);
        for(int i = 0; i< nearbyRoadHits.Length; i++)
        {
            if (nearbyRoadHits[i].collider?.GetComponent<RoadBase>() != null)
            {
                Waypoint[] thisObjectWaypoint = FindWaypointsInRoadClosestToPoint(currentRoad, nearbyRoadHits[i].point);
                Waypoint[] otherObjectWaypoints = FindWaypointsInRoadClosestToPoint(nearbyRoadHits[i].collider.GetComponent<RoadBase>(), nearbyRoadHits[i].point);
                Waypoint[][] orderedForConnection = OrderWaypointsForConnection(thisObjectWaypoint, otherObjectWaypoints);
                RoadBase.ConnectRoads(orderedForConnection);
            }
        }
        currentOrientation = currentRoad.createDirection;
    }
    
    private RoadBase NewRoad(GameObject roadPrefab, string newOrientation, Vector3[] newPositionTransform, string newName)
    {
        GameObject roadObject = Instantiate(roadPrefab, roadRoot);
        if (newName == null)
        {
            roadObject.name = "Road " + roadRoot.childCount;
        }
        else
        { 
            roadObject.name = newName;
        }
        RoadBase prevRoad = GetPrevRoad();
        if (prevRoad != null)
        {
            roadObject.GetComponent<IRoadInterface>().CreateRoad(prevRoad, gridSize, newOrientation, newPositionTransform);
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

    private RaycastHit[] FindNearbyRoads(RoadBase currentRoad)
    {
        RaycastHit[] objectHits = new RaycastHit[RoadBase.directions.Length];
        for (int i = 0; i < RoadBase.directions.Length; i++)
        {
            string dir = RoadBase.directions[i];
            // If can connect towards dir
            if (currentRoad.GetComponent<IRoadInterface>().IsDirectionConnectable(dir))
            {
                RaycastHit[] hits = Physics.RaycastAll(currentRoad.transform.position, RoadBase.DirToVec(dir), rayLength, roadLayerMask);
                if (hits.Length > 0)
                {

                    if (hits[0].collider.gameObject != currentRoad.gameObject)
                    {
                        objectHits[i] = hits[0];
                    }
                    else if (hits.Length > 1)
                    {
                        if (hits[1].collider.gameObject != currentRoad.gameObject)
                        {
                            objectHits[i] = hits[1];
                        }
                    }

                }
            }
        }
        return objectHits;
    }

    private Waypoint[] FindWaypointsInRoadClosestToPoint(RoadBase road, Vector3 pointHit)
    {
        float[] closestDistances = { Mathf.Infinity, Mathf.Infinity };
        GameObject[] closestWaypointObjects = { null, null };
        foreach (GameObject laneObject in road.lanes)
        {
            float distanceToPoint = (laneObject.transform.position - pointHit).magnitude;
            if (distanceToPoint < closestDistances[0] && laneObject != closestWaypointObjects[1])
            {
                closestWaypointObjects[1] = closestWaypointObjects[0];
                closestWaypointObjects[0] = laneObject;
                closestDistances[1] = closestDistances[0];
                closestDistances[0] = distanceToPoint;
            }
            else if (distanceToPoint < closestDistances[1] && laneObject != closestWaypointObjects[0])
            {
                closestWaypointObjects[1] = laneObject;
                closestDistances[1] = distanceToPoint;
            }
        }
        Waypoint[] closestWaypoints = {closestWaypointObjects[0].GetComponent<Waypoint>(),
        closestWaypointObjects[1].GetComponent<Waypoint>()};
        return closestWaypoints;
    }

    private Waypoint[][] OrderWaypointsForConnection(Waypoint[] waypoints0, Waypoint[] waypoints1)
    {
        Waypoint[] fromWaypoints = new Waypoint[2];
        Waypoint[] toWaypoints = new Waypoint[2];
        float xDist = Mathf.Abs(waypoints0[0].transform.position.x - waypoints0[1].transform.position.x);
        float zDist = Mathf.Abs(waypoints0[0].transform.position.z - waypoints0[1].transform.position.z);

        // Roads orient along the forward / backward direction
        if(xDist > zDist)
        {
            float dz = waypoints1[0].transform.position.z - waypoints0[0].transform.position.z;
            // Rightmost waypoint connects forward (waypoint.next = forwardWaypoint)
            if (dz > 0)
            {
                fromWaypoints[0] = MoreRight(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
                fromWaypoints[1] = MoreLeft(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();

                toWaypoints[0] = MoreRight(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();
                toWaypoints[1] = MoreLeft(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
            }
            else
            {
                fromWaypoints[0] = MoreLeft(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
                fromWaypoints[1] = MoreRight(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();

                toWaypoints[0] = MoreLeft(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();
                toWaypoints[1] = MoreRight(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
            }
        }
        else
        {
            float dx = waypoints1[0].transform.position.x - waypoints0[0].transform.position.x;
            // Backward most waypoint connects right (waypoint.next = rightWaypoint)
            if (dx > 0)
            {
                fromWaypoints[0] = MoreBackward(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
                fromWaypoints[1] = MoreForward(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();

                toWaypoints[0] = MoreBackward(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();
                toWaypoints[1] = MoreForward(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
            }
            else
            {
                fromWaypoints[0] = MoreForward(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
                fromWaypoints[1] = MoreBackward(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();

                toWaypoints[0] = MoreForward(waypoints1[0].transform, waypoints1[1].transform).GetComponent<Waypoint>();
                toWaypoints[1] = MoreBackward(waypoints0[0].transform, waypoints0[1].transform).GetComponent<Waypoint>();
            }
        }
        Waypoint[][] outArray = { fromWaypoints, toWaypoints };
        return outArray;


    }

    private Transform MoreRight(Transform t1, Transform t2)
    {
        return (t1.position.x > t2.position.x) ? t1 : t2;
    }

    private Transform MoreLeft(Transform t1, Transform t2)
    {
        return (t1.position.x < t2.position.x) ? t1 : t2;
    }

    private Transform MoreForward(Transform t1, Transform t2)
    {
        return (t1.position.z > t2.position.z) ? t1 : t2;
    }

    private Transform MoreBackward(Transform t1, Transform t2)
    {
        return (t1.position.z < t2.position.z) ? t1 : t2;
    }

    private void LoadDataFromPrefab()
    {
        if (dataPrefab != null)
        {
            RoadEditorData data = dataPrefab.GetComponent<RoadEditorData>();

            gridSize = data.gridSize;

            roadPrefab = data.prefabs[0];
            roadPrefabCorner_R = data.prefabs[1];
            roadPrefabCorner_L = data.prefabs[2];
            roadPrefab_T = data.prefabs[3];
            roadPrefab_X = data.prefabs[4];

            roadButtonTexture = data.textures[0];
            roadButtonRTexture = data.textures[1];
            roadButtonLTexture = data.textures[2];
            roadButtonTTexture = data.textures[3];
            roadButtonXTexture = data.textures[4];

            arrowUpTexture = data.textures[5];
            arrowRightTexture = data.textures[6];
            arrowDownTexture = data.textures[7];
            arrowLeftTexture = data.textures[8];

            arrowUpTexture_G = data.textures[9];
            arrowRightTexture_G = data.textures[10];
            arrowDownTexture_G = data.textures[11];
            arrowLeftTexture_G = data.textures[12];

            clockwiseText = data.textures[13];
            counterClockwiseText = data.textures[14];
        }
    }

    private bool IsDirectionButtonDisabled(IRoadInterface currentRoad, GameObject currentlySelected, string dir)
    {
        return !(currentRoad.IsDirectionConnectable(dir));
    //&& currentlySelected?.GetComponent<RoadBase>()?.createDirection != dir);
    }
}

