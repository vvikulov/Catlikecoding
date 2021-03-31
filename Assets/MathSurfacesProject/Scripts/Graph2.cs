using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphG
{
    public class Graph2 : MonoBehaviour
    {
        #region fields
        [SerializeField]
        protected Transform m_pointPrefab;
        [SerializeField, Range(10, 100)]
        protected int m_resolution = 10;
        [SerializeField]
        FunctionLibrary.FunctionName m_function;

        private Transform[] m_points;
        #endregion

        #region Unity Events
        private void Awake ()
        {
            m_points = new Transform[m_resolution * m_resolution];

            float step = 2f / m_resolution;
            var scale = Vector3.one * step;
            for (int i = 0; i < m_points.Length; i++)
            {
                Transform point = Instantiate(m_pointPrefab);
                point.localScale = scale;
                point.SetParent(transform, false);
                m_points[i] = point;
            }
        }

        private void Update ()
        {
            FunctionLibrary.Function f = FunctionLibrary.GetFunction(m_function);
            float time = Time.time;
            float step = 2f / m_resolution;
            float v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < m_points.Length; i++, x++)
            {
                if (x == m_resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;
                m_points[i].localPosition = f(u, v, time);
            }
        }
        #endregion
    }
}