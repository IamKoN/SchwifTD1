using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for recycling objects
public class ObjectPool : MonoBehaviour 
{
    //List of objects in pool
    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField]
    private GameObject[] objectPrefabs;

    //Gets objects from the pool
    public GameObject GetObject(string type)
    {
        //check poop for object
        foreach (GameObject go in pooledObjects)
        {
            //if pool contains object we need, re use it
            if(go.name == type && !go.activeInHierarchy)
            {
                //sets object to active
                go.SetActive(true);
                return go;
            }
        }
        //if pool does not contain object, create the object
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }
        //If everything fails
        return null;
    }

    //Release object in the pool
    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

}
