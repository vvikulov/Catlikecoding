using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphG
{
    public class Graph2 : MonoBehaviour
    {
        public enum TransitionMode { Cycle, Random }

        #region fields
        [SerializeField]
        protected Transform m_pointPrefab;
        [SerializeField, Range(10, 100)]
        protected int m_resolution = 10;
        [SerializeField]
        protected FunctionLibrary.FunctionName m_function;
        [SerializeField, Min(0f)]
        protected float m_functionDuration = 1f, m_transitionDuration = 1f;
        [SerializeField]
        private TransitionMode m_transitionMode = TransitionMode.Cycle;
        [SerializeField]
        private bool _isChange = true;

        private Transform[] m_points;
        private float m_duration;

        private bool m_transitioning;
        private FunctionLibrary.FunctionName m_transitionFunction;

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
            if(!_isChange)
            {
                UpdateFunction();
            }
            else
            {
                m_duration += Time.deltaTime;
                if (m_transitioning)
                {
                    if (m_duration >= m_transitionDuration)
                    {
                        m_duration -= m_transitionDuration;
                        m_transitioning = false;
                    }
                }
                else if (m_duration >= m_functionDuration)
                {
                    m_duration -= m_functionDuration;
                    m_transitioning = true;
                    m_transitionFunction = m_function;
                    PickNextFunction();
                }
                if (m_transitioning)
                {
                    UpdateFunctionTransition();
                }
                else
                {
                    UpdateFunction();
                }
            }
        }
        #endregion

        private void UpdateFunction()
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

        void UpdateFunctionTransition()
        {
            FunctionLibrary.Function
                from = FunctionLibrary.GetFunction(m_transitionFunction),
                to = FunctionLibrary.GetFunction(m_function);
            float progress = m_duration / m_transitionDuration;
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
                m_points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
            }
        }

        private void PickNextFunction()
        {
            m_function = m_transitionMode == TransitionMode.Cycle ?
                FunctionLibrary.GetNextFunctionName(m_function) :
                FunctionLibrary.GetRandomFunctionNameOtherThan(m_function);
        }
    }
}