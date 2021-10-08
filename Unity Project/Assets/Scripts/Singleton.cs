using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T Instance
    {
        get
        {
            if (instance == null)
                instance = (T)FindObjectsOfTypeAll(typeof(T))[0];

            return instance;
        }
    }

    private static T instance;
}
