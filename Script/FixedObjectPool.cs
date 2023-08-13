using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    /// <summary>
    /// Manages a fixed pool of GameObjects. 
    /// Objects are stored as inactive when not in use and can be activated when requested. 
    /// Supports a maximum lifetime for objects, automatically deactivating them when 
    /// they reach the given lifetime.
    /// </summary>
    public class FixedObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectPool;
        [SerializeField] private float maximumLifetime;

        private Dictionary<GameObject, float> activeObjects = new();
        private List<GameObject> inactiveObjects = new();

        private void Start()
        {
            for (int i = 0; i < objectPool.Length; i++)
            {
                objectPool[i].SetActive(false);
                inactiveObjects.Add(objectPool[i]);
            }

            if (maximumLifetime > 0)
            {
                StartCoroutine(DeactivationRoutine());
            }
        }

        public GameObject GetObject()
        {
            if (inactiveObjects.Count > 0)
            {
                int randomIndex = Random.Range(0, inactiveObjects.Count);
                GameObject selectedObject = inactiveObjects[randomIndex];
                inactiveObjects.RemoveAt(randomIndex);
                Activate(selectedObject);

                return selectedObject;
            }

            GameObject oldestActiveObject = GetOldestActiveObject();
            Deactivate(oldestActiveObject);
            Activate(oldestActiveObject);

            return oldestActiveObject;
        }

        private void Activate(GameObject obj)
        {
            obj.SetActive(true);
            activeObjects[obj] = Time.time;
        }

        private void Deactivate(GameObject obj)
        {
            obj.SetActive(false);
            activeObjects.Remove(obj);
            inactiveObjects.Add(obj);
        }

        private GameObject GetOldestActiveObject()
        {
            GameObject oldestObject = null;
            float oldestActivationTime = float.PositiveInfinity;

            foreach (KeyValuePair<GameObject, float> kvp in activeObjects)
            {
                if (kvp.Value < oldestActivationTime)
                {
                    oldestObject = kvp.Key;
                    oldestActivationTime = kvp.Value;
                }
            }

            return oldestObject;
        }

        private IEnumerator DeactivationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(maximumLifetime);

                List<GameObject> toRemove = new();
                foreach (KeyValuePair<GameObject, float> kvp in activeObjects)
                {
                    if (!kvp.Key.activeSelf || Time.time - kvp.Value >= maximumLifetime)
                    {
                        toRemove.Add(kvp.Key);
                    }
                }

                foreach (GameObject obj in toRemove)
                {
                    Deactivate(obj);
                }
            }
        }
    }
}

