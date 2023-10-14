using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.UIElements;

public enum DungeonState { inactive, generatingMain, generatingBranches, cleanup, completed}
public class ProceduralDungeonGenerator : MonoBehaviour
{


    [Header("Generation options")]
     public int seed;
    [SerializeField] public bool useSeed = false;
    [SerializeField] public GameObject[] startPrefabs; //Playerspawn room
    [SerializeField] public GameObject[] tilePrefabs;
    [SerializeField] public GameObject[] hallwayPrefabs;
    [SerializeField] public GameObject[] bossRoomPrefabs;
    [SerializeField] public GameObject[] blockedPrefabs;
    [SerializeField] public GameObject[] doorPrefabs;
    [SerializeField] public GameObject[] exitPrefabs;
    [SerializeField][Range(10,100)] public int roomCount = 10;
    [SerializeField][Range(0,50)] public int branchCount = 10;
    [SerializeField][Range(0,50)] public int branchLength = 5;
    [SerializeField][Range(0, 100)] public int doorPercent = 25;
    [SerializeField][Range(0, 1f)] public float constructionDelay = 0.1f;


    public DungeonState dungeonState = DungeonState.inactive;
    public List<Tile> generatedTiles = new List<Tile>();
    public List<Connector> availableConnectors = new List<Connector>(); 
    
    [Header("Debugging Options")]
    [SerializeField] public bool debugGenerate = false;
    [SerializeField] public KeyCode reloadKey = KeyCode.Space;
    [SerializeField] public KeyCode toggleMapKey = KeyCode.M;
    [SerializeField] public bool useBoxColliders = false;
    [SerializeField] public bool useLights = false;
    [SerializeField] public bool restoreLights = false;

    public FloorTextOverlay counter;

    public BakeNavMesh navMesh;
    GameObject playerCam;
    Color startLightColor = Color.white;

    Transform tileFrom, tileTo, tileRoot;
    Transform container;
    int attempts = 0;
    public int maxAttempts = 50;


    public UnityAction OnFinishBuilding;
  
    void Start()
    {
        GameObject floorText = GameObject.Find("Floor Counter");
        counter = floorText.GetComponent<FloorTextOverlay>();
        playerCam = GameObject.FindWithTag("Player");
        if (useSeed)
        {
            //seed = System.DateTime.Now.Millisecond;
            Debug.Log(seed);
            Random.InitState(seed);
        }
        
        StartCoroutine(DungeonGenerator());

       
    }

    private void Update()
    {
        if (Input.GetKeyDown(reloadKey) && debugGenerate)
        {
            SceneManager.LoadScene("MainGame");
        }
        
    }
    IEnumerator DungeonGenerator()
    {
        GameObject goContainer = new GameObject("Main Path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = CreateStartTile();
        DebugRoomLighting(tileRoot, Color.blue);
        tileTo = tileRoot;
        dungeonState = DungeonState.generatingMain;

        while(generatedTiles.Count < roomCount)
        {
            yield return new WaitForSeconds(constructionDelay);
            tileFrom = tileTo;
         

            if (generatedTiles.Count == roomCount-1) {
                //create exit room as the last room in main branch
                tileTo = CreateExitTile();
                DebugRoomLighting(tileTo, Color.magenta);
            }else if (generatedTiles.Count == roomCount - 2)
            {
                //create exit room as the last room in main branch
                tileTo = CreateBossRoomTile();
                DebugRoomLighting(tileTo, Color.magenta);
            }
            else
            {
                tileTo = CreateTile();
                DebugRoomLighting(tileTo, Color.yellow);
            }
          

            
            ConnectTiles();
            CollisionCheck();
           
        }

      

        //get all connector within container that are not connected //open rooms

        foreach(Connector connector in container.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                if (!availableConnectors.Contains(connector))
                {
                    availableConnectors.Add(connector);
                }
            }
        }


        //branching
        dungeonState = DungeonState.generatingBranches;
        for (int b = 0; b < branchCount; b++)
        {   if (availableConnectors.Count > 0)
            {
                goContainer = new GameObject("Branch " + (b + 1));
                container = goContainer.transform;
                container.SetParent(transform);
                int availIndex = Random.Range(0, availableConnectors.Count);
                tileRoot = availableConnectors[availIndex].transform.parent.parent;
                availableConnectors.RemoveAt(availIndex);
                tileTo = tileRoot;
                for (int i = 0; i < branchLength - 1; i++)
                {
                  
                    yield return new WaitForSeconds(constructionDelay);
                    tileFrom = tileTo;
                    tileTo = CreateTile();
                    DebugRoomLighting(tileTo, Color.blue);
                    ConnectTiles();
                    CollisionCheck();
                    if (attempts >= maxAttempts) { break; }
                }
            }
            else
            {
                break; //try another branch
            }

        }

        dungeonState = DungeonState.cleanup;
        LightReset();
        DestroyBoxColliders();
        BlockPassages();
        SpawnDoor();

        yield return null; //wait 1 frame
        dungeonState = DungeonState.completed;

        navMesh.BuildNavMesh();
        OnFinishBuilding?.Invoke();
        counter.SetFloorText();
        navMesh.BuildNavMesh();

    }

    void SpawnDoor()
    {
        if(doorPercent > 0)
        {
            Connector[] allConnectors = transform.GetComponentsInChildren<Connector>();
            for(int i = 0; i < allConnectors.Length; i++)
            {
                Connector myConnector = allConnectors[i];
                //Spawn door by change if tiles are connected
                if(myConnector.isConnected)
                {
                    int roll = Random.Range(1, 101);
                    if(roll <= doorPercent)
                    {
                        Vector3 halfExtents = new Vector3(myConnector.size.x, 1f, myConnector.size.x);
                        Vector3 pos = myConnector.transform.position;
                        Vector3 offset = Vector3.up * 0.5f;
                        Collider[] hits = Physics.OverlapBox(pos+offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Door"));
                        if (hits.Length == 0)
                        {
                            int doorIndex = Random.Range(0, doorPrefabs.Length);
                            GameObject door= Instantiate(doorPrefabs[doorIndex], pos, myConnector.transform.rotation, myConnector.transform);
                            door.name = doorPrefabs[doorIndex].name;
                        }
                    }

                }

            }
        }
    }

    void BlockPassages()
    {

     
    

       
        foreach(Connector connector in transform.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {


                
                Vector3 pos = connector.transform.position;
                int wallIndex = Random.Range(0, blockedPrefabs.Length);
                GameObject wall = Instantiate(blockedPrefabs[wallIndex], pos, connector.transform.rotation, connector.transform) as GameObject;
                wall.name = blockedPrefabs[wallIndex].name;
                
         
               
            }

        }

    }

    void LightReset()
    {
        if(useLights && restoreLights && Application.isEditor)
        {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach(Light light in lights) {
                light.color = startLightColor;
            
            }
        }
        
    }

    void DestroyBoxColliders()
    {
        if(!useBoxColliders)
        {
            foreach(Tile myTile in generatedTiles)
            {

                BoxCollider box = myTile.tile.GetComponent<BoxCollider>();  
                if(box != null)
                {
                    Destroy(box);
                }
            }
        }
    }

    void DebugRoomLighting(Transform tile, Color lightColor)
    {
        if(useLights && Application.isEditor) {
            Light[] lights = tile.GetComponentsInChildren<Light>();  
            if(lights.Length > 0)
            {
                if (startLightColor == Color.white)
                {
                    startLightColor = lights[0].color;
                }
                foreach(Light light in lights)
                {
                    light.color = lightColor;
                }
            }
            
        }

    }


    void CollisionCheck()
    {
        //BoxCollider box = tileTo.GetComponent<BoxCollider>();
        Collider collider = tileTo.GetComponent<Collider>();

        /*
        if (box == null)
        {
            box = tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }
        

        //offset = position in space. halfExtents = volumes of space from the center of that point
       
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Tiles")).ToList();
        */
        Vector3 offset = (tileTo.right * collider.bounds.center.x) + (tileTo.up * collider.bounds.center.y) + (tileTo.forward * collider.bounds.center.z);
        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, collider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Tiles")).ToList();
        hits.AddRange(Physics.OverlapBox(tileTo.position + offset, collider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Boundary")).ToList());
        if (hits.Count > 0)
        {
            //hit something other than tielFrom and tileTo
            if(hits.Exists(x => x.transform != tileFrom && x.transform != tileTo))
            {
                attempts++;

                int index = generatedTiles.FindIndex(x => x.tile == tileTo);
                if (generatedTiles[index].connector != null)
                {
                    generatedTiles[index].connector.isConnected = false;
                }

                generatedTiles.RemoveAt(index);
                DestroyImmediate(tileTo.gameObject);
                //backtracking

                if (attempts >= maxAttempts)
                {
                    int fromIndex = generatedTiles.FindIndex(x => x.tile == tileFrom);
                    Tile myTileFrom = generatedTiles[fromIndex];
                    if (tileFrom != tileRoot)
                    {
                        //at the root
                        if (myTileFrom.connector != null)
                        {
                            myTileFrom.connector.isConnected = false;
                        }
                        availableConnectors.RemoveAll(x => x.transform.parent.parent == tileFrom); //remove connectors 
                        generatedTiles.RemoveAt(fromIndex);
                        DestroyImmediate(tileFrom.gameObject);
                        if (myTileFrom.origin != tileRoot)
                        {
                            tileFrom = myTileFrom.origin;
                        }
                        else if (container.name.Contains("Main"))
                        {
                            if (myTileFrom.origin != null)
                            {
                                tileRoot = myTileFrom.origin;
                                tileFrom = tileRoot;
                            }
                        }
                    else if (availableConnectors.Count > 0)
                    {
                        int availIndex = Random.Range(0, availableConnectors.Count);
                        tileRoot = availableConnectors[availIndex].transform.parent.parent;
                        availableConnectors.RemoveAt(availIndex);
                        tileFrom = tileRoot;
                    }  else { return;}




                }else if (container.name.Contains("Main"))
                {
                        if(myTileFrom.origin != null)
                        {
                            tileRoot = myTileFrom.origin;
                            tileFrom = tileRoot;
                        }



                }else if(availableConnectors.Count > 0)
                {
                        int availIndex = Random.Range(0, availableConnectors.Count);
                        tileRoot = availableConnectors[availIndex].transform.parent.parent;
                        availableConnectors.RemoveAt(availIndex);
                        tileFrom = tileRoot;
                 }

                      else { return;}
                }
              







                //retry placing tiles
                if (tileFrom != null)
                {



                    if (generatedTiles.Count == roomCount - 1)
                    {

                        //create exit room as the last room in main branch
                        tileTo = CreateExitTile();
                        DebugRoomLighting(tileTo, Color.magenta);
                    }
                    else if (generatedTiles.Count == roomCount - 2)
                    {
                        //create exit room as the last room in main branch
                        tileTo = CreateBossRoomTile();
                        DebugRoomLighting(tileTo, Color.red);
                    }
                    else
                    {
                        tileTo = CreateTile();
                        Color retryColor = container.name.Contains("Branch") ? Color.green : Color.yellow;
                        DebugRoomLighting(tileTo, retryColor * 2f);
                    }
            
                    ConnectTiles();
                    CollisionCheck();
                }
            }else { attempts = 0;} //nothing was hit
        }
    }

    Transform CreateStartTile()
    {
        int index = Random.Range(0, startPrefabs.Length);
        GameObject startTile = Instantiate(startPrefabs[index], Vector3.zero, Quaternion.identity,container) as GameObject;
        startTile.name = "Player Start Room";
        float yRotation = Random.Range(0, 4) * 90f;
        startTile.transform.Rotate(0, yRotation, 0);

        
        playerCam.transform.LookAt(startTile.GetComponentInChildren<Connector>().transform.position); //player looks at opening
        generatedTiles.Add(new Tile(startTile.transform, null));
        return startTile.transform;
    }


    Transform CreateHallwayTile()
    {

        int index = Random.Range(0, hallwayPrefabs.Length);

        GameObject tile = Instantiate(hallwayPrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        tile.name = hallwayPrefabs[index].name;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile; //set the origin to the previous tile

        generatedTiles.Add(new Tile(tile.transform, origin));
        return tile.transform;
    }

    Transform CreateTile()
    {
        
        int index = Random.Range(0, tilePrefabs.Length);
       
        GameObject tile = Instantiate(tilePrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        tile.name = tilePrefabs[index].name;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile; //set the origin to the previous tile
       
        generatedTiles.Add(new Tile(tile.transform, origin));
        return tile.transform;
    }

    Transform CreateBossRoomTile()
    {
        int index = Random.Range(0, bossRoomPrefabs.Length);

        GameObject tile = Instantiate(bossRoomPrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        Debug.Log(tile.name);
        tile.name = "Boss Room";
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile; //set the origin to the previous tile

        generatedTiles.Add(new Tile(tile.transform, origin));
        return tile.transform;
    }

    Transform CreateExitTile()
    {

        int index = Random.Range(0, exitPrefabs.Length);

        GameObject tile = Instantiate(exitPrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        tile.name = "Exit Room";
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile; //set the origin to the previous tile

        generatedTiles.Add(new Tile(tile.transform, origin));
        return tile.transform;
    }

    void ConnectTiles()
    {
        Transform connectFrom = GetRandomConnector(tileFrom);
        if(connectFrom == null) { return; }

        Transform connectTo = GetRandomConnector(tileTo);
        if(connectTo == null) { return; }


        connectTo.SetParent(connectFrom);
        tileTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero; 
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0, 180f, 0);
        tileTo.SetParent(container);
        connectTo.SetParent(tileTo.Find("Connectors"));



        generatedTiles.Last().connector = connectFrom.GetComponent<Connector>();
    }

    private Transform GetRandomConnector(Transform tile)
    {
       
        if(tile == null) { return null; }
        List<Connector> connectors = tile.GetComponentsInChildren<Connector>().ToList().FindAll(x => x.isConnected == false);

        if(connectors.Count > 0)
        {
            int connectorIndex = Random.Range(0, connectors.Count);
            connectors[connectorIndex].isConnected = true;

            //Add a boxcollider to room objects, used for checking collisings with other tiles/rooms
            if(tile==tileFrom)
            {
                BoxCollider box = tile.GetComponent<BoxCollider>(); 
                if(box == null)
                {
                    box = tile.gameObject.AddComponent<BoxCollider>();
                    box.isTrigger = true;
                }
            }
            return connectors[connectorIndex].transform;
        }


        return null;
    }
}
