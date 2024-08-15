using UnityEngine;

namespace Utils {
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Load the instance from the Resources folder
                    _instance = Resources.Load<T>(typeof(T).Name);

                    // If the instance is not found in Resources, create a new instance
                    if (_instance == null)
                    {
                        _instance = CreateInstance<T>();
                        Debug.LogWarning(typeof(T).Name + " not found in Resources. A new instance was created.");
                    }
                }

                return _instance;
            }
        }
    }
}