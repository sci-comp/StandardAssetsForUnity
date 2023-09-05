using UnityEngine;

namespace Toolbox
{
    /// <summary>
    /// A generic Singleton implementation for MonoBehaviour classes that makes use of 
    /// DontDetroyOnLoad. Ensures that only one instance of the component is available 
    /// in the scene, creating a new instance if necessary and destroying duplicate instances.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).Name;
                        Debug.Log("Creating singleton: " + singletonObject.name);
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                enabled = true;
            }
            else
            {
                if (this != instance)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}

