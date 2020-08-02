using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Winch;
using Winch.Update;

public class MovementTest : MonoBehaviour
{
    [SerializeField]
    private float m_TimeDelta;
    [SerializeField]
    private Vector3 m_Force;
    [SerializeField]
    private Vector3 m_Velocity;
    [SerializeField]
    private KeyCode m_FireKey;

    [SerializeField]
    private GameObject m_VisualObject;

    private PhysicsBody m_Round;
    private GameObject m_CurrentView;

    private void Update()
    {
        Updater.Instance.TimeScale.Value = m_TimeDelta;

        if (Input.GetKeyDown(m_FireKey))
        {
            Fire();
        }

        if (Input.GetKey(KeyCode.W))
        {
            m_Round.Force.Value += m_CurrentView.transform.forward * 0.0001f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_Round.Force.Value -= m_CurrentView.transform.forward * 0.0001f;
        }
    }

    public void Fire()
    {
        m_Round = new PhysicsBody();
        m_Round.BeginUpdate();

        m_CurrentView = Instantiate(m_VisualObject);
        m_CurrentView.SetActive(true);
        m_Round.Position.Subscribe((position) => m_CurrentView.transform.position = position);
    }
}

