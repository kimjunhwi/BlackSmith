using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCashing : GenericMonoSingleton<ObjectCashing> {

	private Dictionary<string,GameObject> cachingObject = new Dictionary<string,GameObject>();
    private Dictionary<string, Sprite> cachingSprite = new Dictionary<string, Sprite>();


    public GameObject LoadObjectFromCache(string path)
    {
        GameObject objResource = null;

        cachingObject.TryGetValue(path, out objResource);


        if (objResource == null)
        {
            objResource = Resources.Load<GameObject>(path) as GameObject;

            if (objResource != null)
                cachingObject.Add(path, objResource);
        }
            

        return objResource;
    }

    public Sprite LoadSpriteFromCache(string path)
    {
        Sprite objResource = null;

        cachingSprite.TryGetValue(path, out objResource);


        if (objResource == null)
        {
            objResource = Resources.Load<Sprite>(path) as Sprite;

            if (objResource != null)
                cachingSprite.Add(path, objResource);
        }


        return objResource;
    }

    

    public void ClearCache()
    {
        cachingObject.Clear();
        cachingSprite.Clear();
        Resources.UnloadUnusedAssets();
    }
}
