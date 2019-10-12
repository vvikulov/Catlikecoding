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
        [SerializeField]
        [Range(10, 100)]
        protected int m_resolution = 10;

        private Transform[] m_points;
        #endregion

        #region Unity Events
        private void Awake()
        {
            float step = 2f / m_resolution;
            Vector3 scale = Vector3.one * step;
            Vector3 position;
            position.y = 0.0f;
            position.z = 0.0f;

            m_points = new Transform[m_resolution];
            for (int i = 0; i < m_points.Length; i++)
            {
                Transform point = GameObject.Instantiate(m_pointPrefab, this.transform, false);
                position.x = (i + 0.5f) * step - 1.0f;
                point.localPosition = position;
                point.localScale = scale;
                m_points[i] = point;
            }
        }

        private void Update() {
            for (int i = 0; i < m_points.Length; i++) {
                Transform point = m_points[i];
                Vector3 position = point.localPosition;
                position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
                point.localPosition = position;
            }
        }
        #endregion
    }
}