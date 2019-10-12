using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphG {
    public class Graph2 : MonoBehaviour {
        #region fields
        [SerializeField]
        protected Transform m_pointPrefab;
        [SerializeField]
        [Range(10, 100)]
        protected int m_resolution = 10;
        [SerializeField]
        protected GraphFunctionName m_function;

        private Transform[] m_points;
        private static GraphFunction[] functions = {
            SineFunction, Sine2DFunction, MultiSineFunction,
            MultiSine2DFunction, Ripple, Cylinder,
            Sphere, Torus
        };
        #endregion

        #region Unity Events
        private void Awake() {
            float step = 2f / m_resolution;
            Vector3 scale = Vector3.one * step;

            m_points = new Transform[m_resolution * m_resolution];

            for (int i = 0; i < m_points.Length; i++) {
                Transform point = Instantiate(m_pointPrefab);
                point.localScale = scale;
                point.SetParent(transform, false);
                m_points[i] = point;
            }
        }

        private void Update() {
            float t = Time.time;
            GraphFunction f = functions[(int)m_function];
            float step = 2f / m_resolution;
            for (int i = 0, z = 0; z < m_resolution; z++) {
                float v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < m_resolution; x++, i++) {
                    float u = (x + 0.5f) * step - 1f;
                    m_points[i].localPosition = f(u, v, t);
                }
            }
        }
        #endregion

        private const float pi = Mathf.PI;

        private static Vector3 SineFunction(float x, float z, float t) {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin(pi * (x + t));
            p.z = z;
            return p;
        }

        private static Vector3 MultiSineFunction(float x, float z, float t) {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
            p.y *= 2f / 3f;
            p.z = z;
            return p;
        }

        private static Vector3 Sine2DFunction(float x, float z, float t) {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(pi * (z + t));
            p.y *= 0.5f;
            p.z = z;
            return p;
        }

        private static Vector3 MultiSine2DFunction(float x, float z, float t) {
            Vector3 p;
            p.x = x;
            p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
            p.y += Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
            p.y *= 1f / 5.5f;
            p.z = z;
            return p;
        }

        private static Vector3 Ripple(float x, float z, float t) {
            Vector3 p;
            float d = Mathf.Sqrt(x * x + z * z);
            p.x = x;
            p.y = Mathf.Sin(pi * (4f * d - t));
            p.y /= 1f + 10f * d;
            p.z = z;
            return p;
        }

        static Vector3 Cylinder(float u, float v, float t) {
            Vector3 p;
            float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
            p.x = r * Mathf.Sin(pi * u);
            p.y = v;
            p.z = r * Mathf.Cos(pi * u);
            return p;
        }

        private static Vector3 Sphere(float u, float v, float t) {
            Vector3 p;
            float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
            r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
            float s = r * Mathf.Cos(pi * 0.5f * v);
            p.x = s * Mathf.Sin(pi * u);
            p.y = r * Mathf.Sin(pi * 0.5f * v);
            p.z = s * Mathf.Cos(pi * u);
            return p;
        }

        private static Vector3 Torus(float u, float v, float t) {
            Vector3 p;
            float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
            float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
            float s = r2 * Mathf.Cos(pi * v) + r1;
            p.x = s * Mathf.Sin(pi * u);
            p.y = r2 * Mathf.Sin(pi * v);
            p.z = s * Mathf.Cos(pi * u);
            return p;
        }
    }
}