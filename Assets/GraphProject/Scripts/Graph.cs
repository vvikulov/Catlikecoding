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
        [SerializeField, Range(10, 100)]
        protected int m_resolution = 10;

        private Transform[] m_points;
        #endregion

        #region Unity Events
        private void Awake ()
        {
            m_points = new Transform[m_resolution];

            float step = 2f / m_resolution;
            Vector3 position = Vector3.zero;
            var scale = Vector3.one * step;
            for (int i = 0; i < m_points.Length; i++)
            {
                Transform point = Instantiate(m_pointPrefab);
                position.x = (i + 0.5f) * step - 1f;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);
                m_points[i] = point;
            }
        }

        private void Update ()
        {
            float time = Time.time;
            for (int i = 0; i < m_points.Length; i++)
            {
                Transform point = m_points[i];
                Vector3 position = point.localPosition;
                position.y = Mathf.Sin(Mathf.PI * (position.x + time));
                point.localPosition = position;
            }
        }
        #endregion
    }
}