using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;

public class AndroidPermissonChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Has Fine Location Permission");

        }
        else
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        
        }

        if (Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Debug.Log("Has Location Permission");

        }
        else
        {
            Permission.RequestUserPermission(Permission.CoarseLocation);

        }

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Debug.Log("Has External Storage Permission");

        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
