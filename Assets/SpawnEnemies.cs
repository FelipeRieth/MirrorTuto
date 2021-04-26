using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnEnemies : NetworkBehaviour
{
  
    public GameObject enemy;
    void Start()
    {

    
       if(isServer && NetworkServer.active)
        {
            GameObject X = Instantiate(enemy) as GameObject;
            X.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            NetworkServer.Spawn(X);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   


   
}
