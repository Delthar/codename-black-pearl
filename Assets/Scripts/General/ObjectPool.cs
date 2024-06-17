using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    public static ObjectPool Instance { get; private set;}

    [SerializeField] private GameObject objectPrefab;

    private List<GameObject> pool = new List<GameObject>();

    [Range(1, 30)]
    [SerializeField] private int poolStartSize = 30;

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
        GameObject item = pool.Where(item => item.activeSelf == false).FirstOrDefault() ?? GeneratePoolObject();
        item.SetActive(true);
        return item;
    }

    public void ReturnPoolObject(GameObject item)
    {
        if(pool.Contains(item))
        {
            item.SetActive(false);
        }
    }

    private GameObject GeneratePoolObject()
    {
        GameObject item = Instantiate(objectPrefab, transform.position, Quaternion.identity, transform);
        pool.Add(item);
        item.SetActive(false);
        return item;
    }
}
