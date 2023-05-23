using System.Collections.Generic;
using UnityEngine;

namespace Embugerance
{
    public class EmbuggeranceManager : MonoBehaviour
    {
        public static EmbuggeranceManager S;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("Detected existing EmbuggeranceManager, destroying new one.");
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        [SerializeField] private List<EmbuggeranceType> embuggerances;

        public bool HasEmbuggerance(EmbuggeranceType key)
        {
            return embuggerances.Contains(key);
        }

        public void AddEmbuggerance(EmbuggeranceType newEmbuggerance)
        {
            if (!embuggerances.Contains(newEmbuggerance))
            {
                embuggerances.Add(newEmbuggerance);
            }
        }

        public void RemoveEmbuggerance(EmbuggeranceType which)
        {
            if (embuggerances.Contains(which))
            {
                embuggerances.Remove(which);
            }
        }
    }
}