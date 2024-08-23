using UnityEngine;

namespace TransformGizmos
{
    public class GizmoController : MonoBehaviour
    {
        [SerializeField] public Rotation m_rotation;
        [SerializeField] public Translation m_translation;
        [SerializeField] public Scaling m_scaling;
        [SerializeField] GameObject m_rotationAppendix;


        [SerializeField] Material m_clickedMaterial;
        [SerializeField] Material m_transparentMaterial;
        [SerializeField] GameObject m_objectWithMeshes;
        [SerializeField] GameObject m_degreesText;

        [Header("Adjustable Variables")]
        [SerializeField] public GameObject m_targetObject;
        [SerializeField] public GameObject m_targetObject_old;
        [SerializeField] float m_gizmoSize = 1;

        Transformation m_transformation = Transformation.None;

        enum Transformation
        {
            None,
            Rotation,
            Translation,
            Scale
        }

        void Start()
        {
            m_targetObject_old = m_targetObject;
            transform.SetPositionAndRotation(m_targetObject.transform.position, m_targetObject.transform.rotation);
            transform.localScale = m_targetObject.transform.lossyScale;
            m_gizmoSize = (1 / m_targetObject.transform.localScale.y)*0.1f;
            if(m_gizmoSize < 0.1f) m_gizmoSize = 0.1f;
            m_rotation.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial, m_objectWithMeshes, m_degreesText, m_rotationAppendix);
            m_translation.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial);
            m_scaling.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial);

            ChangeTransformationState(Transformation.None);
        }

        void Update()
        {
            if(m_targetObject!=m_targetObject_old)
            {
         
                m_targetObject_old = m_targetObject;
                transform.SetPositionAndRotation(m_targetObject.transform.position, m_targetObject.transform.rotation);
                transform.localScale = m_targetObject.transform.lossyScale;
                m_gizmoSize = (1 / m_targetObject.transform.lossyScale.y)*0.1f;
                if (m_gizmoSize < 0.1f) m_gizmoSize = 0.1f;
                m_rotation.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial, m_objectWithMeshes, m_degreesText, m_rotationAppendix);
                m_translation.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial);
                m_scaling.Initialization(m_targetObject, m_clickedMaterial, m_transparentMaterial);
            }

            transform.SetPositionAndRotation(m_targetObject.transform.position, m_targetObject.transform.rotation);
            m_degreesText.transform.position = m_targetObject.transform.position;
            m_objectWithMeshes.transform.position = m_targetObject.transform.position;
            m_rotation.SetGizmoSize(m_gizmoSize);
            m_translation.SetGizmoSize(m_gizmoSize);
            m_scaling.SetGizmoSize(m_gizmoSize);

            if (Input.GetKeyDown(KeyCode.R))
                ChangeTransformationState(Transformation.Rotation);

            if (Input.GetKeyDown(KeyCode.T))
                ChangeTransformationState(Transformation.Translation);

            if (Input.GetKeyDown(KeyCode.Z))
                ChangeTransformationState(Transformation.Scale);
        }

        private void ChangeTransformationState(Transformation transformation)
        {
            m_rotation.gameObject.SetActive(false);
            m_translation.gameObject.SetActive(false);
            m_scaling.gameObject.SetActive(false);

            switch (transformation)
            {
                case Transformation.None:
                    break;

                case Transformation.Rotation:
                    if (m_transformation == Transformation.Rotation)
                    {
                        m_transformation = Transformation.None;
                    }
                    else
                    {
                        m_rotation.gameObject.SetActive(true);
                        m_transformation = transformation;
                    }
                    break;

                case Transformation.Translation:
                    if (m_transformation == Transformation.Translation)
                    {
                        m_transformation = Transformation.None;
                    }
                    else
                    {
                        m_translation.gameObject.SetActive(true);
                        m_transformation = transformation;
                    }
                    break;

                case Transformation.Scale:
                    if (m_transformation == Transformation.Scale)
                    {
                        m_transformation = Transformation.None;
                    }
                    else
                    {
                        m_scaling.gameObject.SetActive(true);
                        m_transformation = transformation;
                    }
                    break;
            }
        }

        public void ToggleRotation()
        {
            ChangeTransformationState(Transformation.Rotation);
        }

        public void ToggleMovement()
        {
            ChangeTransformationState(Transformation.Translation);
        }

        public void ToggleScale()
        {
            ChangeTransformationState(Transformation.Scale);
        }
    }
}
