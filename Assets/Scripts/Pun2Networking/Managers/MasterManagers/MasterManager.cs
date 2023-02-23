using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableObjSingleton<MasterManager>
{
    //VARIABLES
    [SerializeField] private GameSettings _gSettings;
    public static GameSettings GSettings { get { return Instance._gSettings; } }

    [SerializeField] private List<NetworkedPrefab> _networkedPrefabs = new List<NetworkedPrefab>();


    //METHODS
    public static GameObject NetworkInstantiate(GameObject obj, Vector3 position, Quaternion rotation)
    {

        foreach (NetworkedPrefab netPrefab in Instance._networkedPrefabs)
        {
            if(netPrefab.Prefab == obj)
            {
                if (netPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(netPrefab.Path, position, rotation);
                    return result;
                }
                else
                {
                    Debug.LogError("Path is Empty for gameobject - " + netPrefab.Prefab);
                    return null;
                }
            }
        }

        return null;
    }



    //ONLY FOR EDITOR PLAY [One time "Instantiation" (adds all of "Resources" to "MasterManager")]
    //DEPRECATED: If add something new to "Resources" folder "Play in Editor" at least Once
    //*UPDATE: "ResourcesAssetsPathBuilder" helps and "Play in Editor" once no longer needs.

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void PopulateNetworkedPrefabs()
    {
#if UNITY_EDITOR

        Instance._networkedPrefabs.Clear();

        GameObject[] results = Resources.LoadAll<GameObject>("");
        for(int i = 0; i<results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance._networkedPrefabs.Add(new NetworkedPrefab(results[i], path));
            }
        }

#endif
    }


}
