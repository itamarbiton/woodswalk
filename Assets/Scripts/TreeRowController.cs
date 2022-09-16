using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRowController : MonoBehaviour
{
    public TreeThemeManager themeManager;
    public MIDIControls controls;

    public Transform tree;
    public Transform explosion;
    public Transform floor;
    public TreeLine[] lineConfigs;

    GameObject treeHolder;
    bool isSliding;

    bool shouldSpawnTree1;
    float tree1Time;

    bool shouldSpawnTree2;
    float tree2Time;

    void Awake()
    {
        controls = new MIDIControls();
        controls.Gameplay.Tree1.started += ctx => { shouldSpawnTree1 = true; };
        controls.Gameplay.Tree1.canceled += ctx => { shouldSpawnTree1 = false; };

        controls.Gameplay.Tree2.started += ctx => { shouldSpawnTree2 = true; };
        controls.Gameplay.Tree2.canceled += ctx => { shouldSpawnTree2 = false; };

        controls.Gameplay.Explosion.performed += ctx => { print(">>>>> explosion is being perfored"); };
    }

    void Start()
    {
        ConfigureTreeHolder();
    }

    void Update()
    {
        if (shouldSpawnTree1)
        {
            if (Time.time > tree1Time)
            {
                SpawnTree(lineConfigs[0]);
                float freq = lineConfigs[0].freq;
                float freqOffset = lineConfigs[0].freqOffset;
                tree1Time = Time.time + freq + Random.Range(freq - freqOffset, freq + freqOffset);
            }
        }

        if (shouldSpawnTree2)
        {
            if (Time.time > tree2Time)
            {
                SpawnTree(lineConfigs[1]);
                float freq = lineConfigs[1].freq;
                float freqOffset = lineConfigs[1].freqOffset;
                tree2Time = Time.time + freq + Random.Range(freq - freqOffset, freq + freqOffset);
            }
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void ConfigureTreeHolder()
    {
        GameObject treeHolder = new GameObject("Tree Holder");
        treeHolder.transform.parent = transform;

        this.treeHolder = treeHolder;
    }

    void SpawnTree(TreeLine line)
    {
        float treeX = floor.localScale.x * 10f / 2f;
        float treeZ = line.y + Random.Range(line.minOffset, line.maxOffset);
        Vector3 treePos = new Vector3(treeX, 0f, treeZ);

        Transform newTreeTransform = Instantiate(tree, treePos, Quaternion.identity);
        newTreeTransform.parent = treeHolder.transform;

        Tree newTree = newTreeTransform.GetComponent<Tree>();
        newTree.slideSpeed = line.speed;
        newTree.themeManager = themeManager;
        newTree.themeIndex = 1;
        newTree.ConfigureControls(controls);
    }

    void SpawnExplosion()
    {
        Vector3 explosionPos = new Vector3(floor.localScale.x * 10 / 2, 0, 0);
        Quaternion explosionRot = Quaternion.LookRotation(Vector3.left, Vector3.up);
        Transform newExplosion = Instantiate(explosion, explosionPos, explosionRot);
    }

    [System.Serializable]
    public struct TreeLine
    {
        public float y;
        public float speed;
        public float minOffset;
        public float maxOffset;
        public float freq;
        public float freqOffset;

        public TreeLine(float y, float speed, float minOffset, float maxOffset, float freq, float freqOffset)
        {
            this.y = y;
            this.speed = speed;
            this.minOffset = minOffset;
            this.maxOffset = maxOffset;
            this.freq = freq;
            this.freqOffset = freqOffset;
        }
    }
}
