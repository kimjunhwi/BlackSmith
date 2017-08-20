using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

    public GameObject[] avaliableObjects;
    public List<GameObject> objects;

    public float objectsMinDistance = 1.0f;

    private float objectsMinY = 1.2f;
    private float objectsMaxY = 3.4f;
    private float fCreateTime = 3.0f;
    public float fCreatePlusTime = 0;

    void Update()
    {
        fCreatePlusTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        GenerateObjectsIfRequired();
    }

    void AddObject(float lastObjectX)
    {
        //1
        int randomIndex = Random.Range(0, avaliableObjects.Length);

        //2
        GameObject obj = (GameObject)Instantiate(avaliableObjects[randomIndex]);

        //3
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectsMinDistance, randomY, 0);

        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        //1
        float playerX = transform.position.x;
        float removeObjectsX = playerX;
        float farthestObjectX = 0;

        //2
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;

            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            //1
            if (objX < removeObjectsX)
                objectsToRemove.Add(obj);
        }

        //6
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        //7
        if (farthestObjectX < 1 && fCreatePlusTime >= fCreateTime)
        {
            AddObject(farthestObjectX);
            fCreatePlusTime = 0f;
        }
    }

}
