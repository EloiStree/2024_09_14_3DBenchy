using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class BenchyLodMono : MonoBehaviour
{

    public Transform m_pointOfView;

    public LOD[] m_lodToApply;
    public bool m_useAwake = true;
    public bool m_useUpdate = true;

    [Header("Debug")]
    public float m_currentDistance;

    [System.Serializable]
    public class LOD
    {
        public float m_distanceMin=0.1f;
        public float m_distanceMax=0.8f;
        public GameObject [] m_gameObject;
        public bool m_isInRange;
        public UnityEvent<bool> m_onInRangeChanged;

        public void ApplyRangeChange()
        {
            ApplyRangeChange(m_isInRange);
        }
        public void ApplyRangeChange(bool inRange)
        {
            m_isInRange = inRange;
            m_onInRangeChanged.Invoke(inRange);
            for (int j = 0; j < m_gameObject.Length; j++)
            {
                if (m_gameObject[j] != null)
                    m_gameObject[j].SetActive(inRange);
            }
        }

        public void SetToHide()
        {
            m_isInRange = false;
            m_onInRangeChanged.Invoke(false);
            for (int j = 0; j < m_gameObject.Length; j++)
            {
                if(m_gameObject[j]!=null)
                    m_gameObject[j].SetActive(false);
            }
        }
    }
    private void Awake()
    {

        if (m_useAwake) { 
            RefreshWithDistanceAndNotify();
        }
    }

    public void Update()
    {
        if (m_useUpdate)
        {
            RefreshWithDistanceOnChanged();
        }
    }
    private bool m_firstTime = true;
    [ContextMenu("Refresh With Distance")]
    public void RefreshWithDistanceAndNotify() { RefreshWithDistanceOnChanged(false);}
    public void RefreshWithDistanceOnChanged(bool checkChanged=true)
    {
        if (m_pointOfView == null && Camera.main!=null)
        {
            m_pointOfView = Camera.main.transform;
        }
        if (m_pointOfView == null)
        {
            return;
        }

        float distance = Vector3.Distance(m_pointOfView.position, transform.position);
        m_currentDistance = distance;
        for (int i = 0; i < m_lodToApply.Length; i++)
        {
            bool previous = m_lodToApply[i].m_isInRange;
            bool newInRange = distance >= m_lodToApply[i].m_distanceMin && distance < m_lodToApply[i].m_distanceMax;

            if (checkChanged==false)
            {
                m_lodToApply[i].ApplyRangeChange(newInRange);
            }
            else if (previous != newInRange)
            {
                m_lodToApply[i].ApplyRangeChange(newInRange);
            }
        }
    }
    [ContextMenu("Refresh To Hide")]
    public void RefreshToHide()
    {
        if (m_pointOfView == null && Camera.main != null)
        {
            m_pointOfView = Camera.main.transform;
        }
        if (m_pointOfView == null)
        {
            return;
        }

        float distance = Vector3.Distance(m_pointOfView.position, transform.position);
        m_currentDistance = distance;
        for (int i = 0; i < m_lodToApply.Length; i++)
        {
            m_lodToApply[i].SetToHide();
        }
    }
}
