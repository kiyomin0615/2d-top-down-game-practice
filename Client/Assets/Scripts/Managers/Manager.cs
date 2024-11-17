using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Manager Singleton
    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    // Network Manager
    private NetworkManager network = new NetworkManager();
    public static NetworkManager Network
    {
        get
        {
            return Instance.network;
        }
    }

    // Object Manager
    private ObjectManager objectManager = new ObjectManager();
    public static ObjectManager Object
    {
        get
        {
            return Instance.objectManager;
        }
    }

    // Map Manager
    private MapManager map = new MapManager();
    public static MapManager Map
    {
        get
        {
            return Instance.map;
        }
    }

    // Resource Manager
    private ResourceManager resource = new ResourceManager();
    public static ResourceManager Resource
    {
        get
        {
            return Instance.resource;
        }
    }

    // UI Manager
    private UIManager ui = new UIManager();
    public static UIManager UI
    {
        get
        {
            return Instance.ui;
        }
    }

    // Scene Manager Extended
    private SceneManagerEx scene = new SceneManagerEx();
    public static SceneManagerEx Scene
    {
        get
        {
            return Instance.scene;
        }
    }

    // Audio Manager
    private AudioManager audioManager = new AudioManager();
    public static AudioManager Audio
    {
        get
        {
            return Instance.audioManager;
        }
    }

    // Pool Manager
    private PoolManager pool = new PoolManager();
    public static PoolManager Pool
    {
        get
        {
            return Instance.pool;
        }
    }

    // Data Manager
    private DataManager data = new DataManager();
    public static DataManager Data
    {
        get
        {
            return Instance.data;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        network.Update();
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject manager = GameObject.Find("@Manager");
            if (manager == null)
            {
                manager = new GameObject("@Manager");
                manager.AddComponent<Manager>();
            }
            DontDestroyOnLoad(manager);
            instance = manager.GetComponent<Manager>();

            instance.network.Init();
            instance.audioManager.Init();
            instance.pool.Init();
            instance.data.Init();
        }
    }

    public static void Clear()
    {
        Audio.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear();
    }
}
