using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

public class ProceduralDungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public Transform tile;
        public Transform origin;
        public Connector connector;

        public Tile(Transform _tile, Transform _origin)
        {
            tile = _tile;
            origin = _origin;
        }
    }


    [Header("Generation options")]
    public GameObject[] startPrefabs; //Playerspawn room
    public GameObject[] tilePrefabs;
    public GameObject[] blockedPrefabs;
    public GameObject[] doorPrefabs;
    public GameObject[] exitPrefabs;
    [Range(10,100)] public int roomCount = 10;
    [Range(0,50)] public int branchCount = 10;
    [Range(0,50)] public int branchLength = 5;
    [Range(0, 100)] public int doorPercent = 25;
    [Range(0, 1f)] public float constructionDelay = 0.1f;

    public List<Tile> generatedTiles = new List<Tile>();
    public List<Connector> availableConnectors = new List<Connector>(); 

    [Header("Debugging Options")]
    public bool debugGenerate = false;
    public KeyCode reloadKey = KeyCode.Space;
    public KeyCode toggleMapKey = KeyCode.M;
    public bool useBoxColliders = false;
    public bool useLights = false;
    public bool restoreLights = false;

    GameObject overheadCamera, playerCam;
    Color startLightColor = Color.white;

    Transform tileFrom, tileTo, tileRoot;
    Transform container;
    int attempts = 0;
    public int maxAttempts = 50;

  
    void Start()
    {
        //overheadCamera = GameObject.Find("OverheadCamera");
        playerCam = GameObject.FindWithTag("Player");
        StartCoroutine(DungeonGenerator());
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(reloadKey) && debugGenerate)
        {
            SceneManager.LoadScene("MainGame");
        }
        if (Input.GetKeyDown(toggleMapKey))
        {
            overheadCamera.SetActive(!overheadCamera.activeInHierarchy);
            playerCam.SetActive(!playerCam.activeInHierarchy);  
        }
    }
    IEnumerator DungeonGenerator()
    {
        //playerCam.SetActive(false);
        //overheadCamera.SetActive(true);
        GameObject goContainer = new GameObject("Main Path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = CreateStartTile();
        DebugRoomLighting(tileRoot, Color.blue);
        tileTo = tileRoot;
        for (int i = 0; i < roomCount-1; i++)
        {
            yield return new WaitForSeconds(constructionDelay);
            tileFrom = tileTo;
            tileTo = CreateTile();

            DebugRoomLighting(tileTo, Color.yellow);
            ConnectTiles();
            CollisionCheck();
            if (attempts >= maxAttempts) { break; }
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
        Debug.Log(availableConnectors.Count);

        //branching
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

        //overheadCamera.SetActive(false);
        
        LightReset();
        DestroyBoxColliders();
        playerCam.SetActive(true);

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
        BoxCollider box = tileTo.GetComponent<BoxCollider>();

        if (box == null)
        {
            box = tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        //offset = position in space. halfExtents = volumes of space from the center of that point
        Vector3 offset = (tileTo.right * box.center.x) + (tileTo.up * box.center.y) + (tileTo.forward * box.center.z);
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Tiles")).ToList();
       
        if(hits.Count > 0)
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
              







                //retry
                if (tileFrom != null)
                {

                    tileTo = CreateTile();
                    Color retryColor = container.name.Contains("Branch") ? Color.green : Color.yellow;
                    DebugRoomLighting(tileTo, retryColor * 2f);
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


    Transform CreateTile()
    {
        
        int index = Random.Range(0, tilePrefabs.Length);
        Debug.Log(index);
        GameObject tile = Instantiate(tilePrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        tile.name = tilePrefabs[index].name;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile; //set the origin to the previous tile
        Debug.Log(origin);
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
