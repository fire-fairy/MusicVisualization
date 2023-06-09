using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    private int m_initailSize = 5;

    private List<GameObject> m_availableObjects = new List<GameObject>();

    public static ObjectPooling Instance;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < m_initailSize; i++)
        {
            GameObject go = Instantiate<GameObject>(m_prefab, this.transform);
            m_availableObjects.Add(go);
            go.SetActive(false);
        }
    }

    public GameObject GetPooledInstance(Transform parent)
    {
        lock (m_availableObjects)
        {
            int lastIndex = m_availableObjects.Count - 1;
            if (lastIndex >= 0)
            {
                GameObject go = m_availableObjects[lastIndex];
                m_availableObjects.RemoveAt(lastIndex);
                go.SetActive(true);
                if (go.transform.parent != parent)
                {
                    go.transform.SetParent(parent);
                }
                return go;
            }
            else
            {
                GameObject go = Instantiate<GameObject>(m_prefab, parent);
                return go;
            }
        }
    }

    public void BackToPool(GameObject go)
    {
        lock (m_availableObjects)
        {
            m_availableObjects.Add(go);
            go.SetActive(false);
        }
    }
}
