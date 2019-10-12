using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphG
{
    public class Graph : MonoBehaviour
    {
        #region fields
        [SerializeField]
        protected Transform m_pointPrefab;
        #endregion

        #region Unity Events
        private void Awake()
        {
            Vector3 scale = Vector3.one / 5.0f;
            Vector3 position;
            for(int i = 0; i < 10; i++)
            {
                Transform point = GameObject.Instantiate(m_pointPrefab, this.transform, false);
                point.localPosition = Vector3.right * ((i + 0.5f) / 5.0f - 1.0f);
                point.localScale = scale;
            }
        }
        #endregion
    }
}