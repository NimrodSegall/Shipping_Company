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
    private GameObject dataPrefab;
    [SerializeField]
    private Transform roadRoot;


    private float gridSize = 15f;
    private GameObject roadPrefab, roadPrefabCorner_R, roadPrefabCorner_L, roadPrefab_T, roadPrefab_X;

    private Texture2D roadButtonTexture, roadButtonRTexture, roadButtonLTexture, roadButtonTTexture, roadButtonXTexture;
    private Texture2D arrowUpTexture, arrowRightTexture, arrowDownTexture, arrowLeftTexture;

    private GameObject currentRoadPrefab = null;

    private float rayLength = 15f;

    private int roadLayerMask = 1 << 10;

    private string[] directions = { "forward", "right", "backward", "left" };

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

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonTexture, RoadBase.DirectionToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab;
            if(selectAndBuild)
            {
                NewRoadButtonCallback();
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonRTexture, RoadBase.DirectionToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefabCorner_R;
            if (selectAndBuild)
            {
                NewRoadButtonCallback();
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonLTexture, RoadBase.DirectionToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefabCorner_L;
            if (selectAndBuild)
            {
                NewRoadButtonCallback();
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonTTexture, RoadBase.DirectionToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab_T;
            if (selectAndBuild)
            {
                NewRoadButtonCallback();
            }
        }

        if (GUILayout.Button(Utilities.RotateTextureClockwise(roadButtonXTexture, RoadBase.DirectionToInd(currentOrientation))))
        {
            currentRoadPrefab = roadPrefab_X;
            if (selectAndBuild)
            {
                NewRoadButtonCallback();
            }
        }

    }

    private void DrawRoadContinueButtons()
    {
        if (Selection.activeGameObject?.GetComponent<IRoadInterface>() != null)
        {
            IRoadInterface currentRoad = Selection.activeGameObject?.GetComponent<IRoadInterface>();
            string dir;
            bool isDisabled = false;

            GUILayout.BeginHorizontal();
            dir = "forward";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (GUILayout.Button(arrowUpTexture))
            {
                currentRoad.SetCreateDirAndLanesOut(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            dir = "left";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (GUILayout.Button(arrowLeftTexture))
            {
                currentRoad.SetCreateDirAndLanesOut(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();

            dir = "right";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (GUILayout.Button(arrowRightTexture))
            {
                currentRoad.SetCreateDirAndLanesOut(dir);
                currentOrientation = dir;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            dir = "backward";
            isDisabled = IsDirectionButtonDisabled(currentRoad, Selection.activeGameObject, dir);
            EditorGUI.BeginDisabledGroup(isDisabled);
            if (GUILayout.Button(arrowDownTexture))
            {
                currentRoad.SetCreateDirAndLanesOut(dir);
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
                NewRoadButtonCallback();
            }
        }
    }

    private void NewRoadButtonCallback()
    {
        RoadBase currentRoad = NewRoad(currentRoadPrefab);
        RoadBase nextRoad = FindNextRoadAndSetInLanes(currentRoad);
        currentOrientation = currentRoad.createDirection;
        if (nextRoad != null)
        {
            currentRoad.ConnectToNextRoad(nextRoad);
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
        }
    }

    private bool IsDirectionButtonDisabled(IRoadInterface currentRoad, GameObject currentlySelected, string dir)
    {
        return !(currentRoad.IsDirectionConnectable(dir));
    //&& currentlySelected?.GetComponent<RoadBase>()?.createDirection != dir);
    }
}

