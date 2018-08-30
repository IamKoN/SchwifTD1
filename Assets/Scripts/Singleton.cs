using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generic class for creating a Singleton
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Instance of a singleton
    private static T instance;

    //Accessing the singleton
    public static T Instance
    {
        get
        { 
            if (instance == null)
            {
                //Finds object
                instance = FindObjectOfType<T>(); 
            }
            return instance; 
        }
    }
}
