using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    public static ObjectPool Instance { get; private set;}

    [SerializeField] private GameObject objectPrefab;

    private List<GameObject> pool;

    [Range(1, 30)]
    [SerializeField] private int poolStartSize = 10;

    private void Awake() 
    {
        Instance = this;    
    }

    private void Start() 
    {
        for (int i = 0; i < poolStartSize; i++) GeneratePoolObject();   
    }

    public GameObject GetPoolObject()
    {
        GameObject item = pool.Where(item => item.activeSelf == false).First() ?? GeneratePoolObject();
        item.SetActive(true);
        return item;
    }

    private GameObject GeneratePoolObject()
    {
        GameObject item = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        pool.Add(item);
        return item;
    }
}
