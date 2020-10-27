using UnityEngine;

namespace TextMesh_Pro.Scripts
{
    
    public class ObjectSpin : MonoBehaviour
    {

#pragma warning disable 0414

        public float spinSpeed = 5;
        public int rotationRange = 15;
        private Transform mTransform;

        private float mTime;
        private Vector3 mPrevPos;
        private Vector3 mInitialRotation;
        private Vector3 mInitialPosition;
        private Color32 mLightColor;
        private int frames = 0;

        public enum MotionType { Rotation, BackAndForth, Translation };
        public MotionType motion;

        void Awake()
        {
            mTransform = transform;
            mInitialRotation = mTransform.rotation.eulerAngles;
            mInitialPosition = mTransform.position;

            Light light = GetComponent<Light>();
            mLightColor = light != null ? light.color : Color.black;
        }


        // Update is called once per frame
        void Update()
        {
            if (motion == MotionType.Rotation)
            {
                mTransform.Rotate(0, spinSpeed * Time.deltaTime, 0);
            }
            else if (motion == MotionType.BackAndForth)
            {
                mTime += spinSpeed * Time.deltaTime;
                mTransform.rotation = Quaternion.Euler(mInitialRotation.x, Mathf.Sin(mTime) * rotationRange + mInitialRotation.y, mInitialRotation.z);
            }
            else
            {
                mTime += spinSpeed * Time.deltaTime;

                float x = 15 * Mathf.Cos(mTime * .95f);
                float y = 10; // *Mathf.Sin(m_time * 1f) * Mathf.Cos(m_time * 1f);
                float z = 0f; // *Mathf.Sin(m_time * .9f);    

                mTransform.position = mInitialPosition + new Vector3(x, z, y);

                // Drawing light patterns because they can be cool looking.
                //if (frames > 2)
                //    Debug.DrawLine(m_transform.position, m_prevPOS, m_lightColor, 100f);

                mPrevPos = mTransform.position;
                frames += 1;
            }
        }
    }
}