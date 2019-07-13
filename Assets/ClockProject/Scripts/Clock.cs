using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ClockG
{
    public class Clock : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Transform m_hoursArm;
        [SerializeField]
        private Transform m_minutesArm;
        [SerializeField]
        private Transform m_secondsArm;

        [SerializeField]
        private bool m_isContinuous;

        private const float DEGREES_PER_HOUR = 30.0f;
        private const float DEGREES_PER_MINUTE = 6.0f;
        private const float DEGREES_PER_SECOND = 6.0f;
        #endregion

        #region Unity Events
        private void Awake()
        {
            DateTime time = DateTime.Now;
            m_hoursArm.localRotation = Quaternion.Euler(0.0f, time.Hour * DEGREES_PER_HOUR, 0.0f);
            m_minutesArm.localRotation = Quaternion.Euler(0.0f, time.Minute * DEGREES_PER_MINUTE, 0.0f);
            m_secondsArm.localRotation = Quaternion.Euler(0.0f, time.Second * DEGREES_PER_SECOND, 0.0f);
        }

        private void Update()
        {
            if(m_isContinuous)
            {
                UpdateContinuous();
            }
            else
            {
                UpdateDiscrete();
            }
        }
        #endregion

        #region Helpers
        private void UpdateContinuous()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            m_hoursArm.localRotation = Quaternion.Euler(0.0f, (float)time.TotalHours * DEGREES_PER_HOUR, 0.0f);
            m_minutesArm.localRotation = Quaternion.Euler(0.0f, (float)time.TotalMinutes * DEGREES_PER_MINUTE, 0.0f);
            m_secondsArm.localRotation = Quaternion.Euler(0.0f, (float)time.TotalSeconds * DEGREES_PER_SECOND, 0.0f);
        }

        private void UpdateDiscrete()
        {
            DateTime time = DateTime.Now;
            m_hoursArm.localRotation = Quaternion.Euler(0.0f, time.Hour * DEGREES_PER_HOUR, 0.0f);
            m_minutesArm.localRotation = Quaternion.Euler(0.0f, time.Minute * DEGREES_PER_MINUTE, 0.0f);
            m_secondsArm.localRotation = Quaternion.Euler(0.0f, time.Second * DEGREES_PER_SECOND, 0.0f);
        }
        #endregion
    }
}