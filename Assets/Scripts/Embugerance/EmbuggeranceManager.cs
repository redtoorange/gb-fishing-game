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
            }
            else
            {
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        [SerializeField] private List<EmbuggeranceType> embuggerances;

        public bool HasEmbuggerance(EmbuggeranceType key)
        {
            return embuggerances.Contains(key);
        }
    }
}