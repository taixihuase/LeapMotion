using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;
using UnityEngine.Events;

namespace Leap.Unity
{
    public class ObjectProximityDetector : Detector
    {
        public ObjectProximityEvent OnProximity;

        [Units("seconds")]
        [MinValue(0)]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        public float Period = .1f; //seconds

        [Header("Detector Targets")]
        [Tooltip("The list of target objects.")]
        public GameObjectArray[] TargetObjects;

        [Header("Distance Settings")]
        [Tooltip("The target distance in meters to activate the detector.")]
        [MinValue(0)]
        public float OnDistance = .01f; //meters

        [Tooltip("The distance in meters at which to deactivate the detector.")]
        public float OffDistance = .015f; //meters

        public GameObject CurrentLhsObject { get { return _currentLhsObj; } }

        public GameObject CurrentRhsObject { get { return _currentRhsObj; } }

        /** Whether to draw the detector's Gizmos for debugging. (Not every detector provides gizmos.)
         * @since 4.1.2 
         */
        [Header("")]
        [Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
        public bool ShowGizmos = true;

        private IEnumerator proximityWatcherCoroutine;

        private GameObject _currentLhsObj = null;

        private GameObject _currentRhsObj = null;

        protected virtual void OnValidate()
        {
            //Activate value cannot be less than deactivate value
            if (OffDistance < OnDistance)
            {
                OffDistance = OnDistance;
            }
        }

        void Awake()
        {
            proximityWatcherCoroutine = proximityWatcher();
        }

        void OnEnable()
        {
            StopCoroutine(proximityWatcherCoroutine);
            StartCoroutine(proximityWatcherCoroutine);
        }

        void OnDisable()
        {
            StopCoroutine(proximityWatcherCoroutine);
            Deactivate();
        }

        IEnumerator proximityWatcher()
        {
            bool proximityState = false;
            float onSquared, offSquared; //Use squared distances to avoid taking square roots
            while (true)
            {
                onSquared = OnDistance * OnDistance;
                offSquared = OffDistance * OffDistance;
                if (_currentLhsObj != null && _currentRhsObj != null)
                {
                    if (distanceSquared(_currentLhsObj, _currentRhsObj) > offSquared)
                    {
                        _currentLhsObj = null;
                        _currentRhsObj = null;
                        proximityState = false;
                    }
                }
                else
                {
                    if(TargetObjects.Length <= 1)
                    {
                        for (int i = 0; i < TargetObjects.Length; i++)
                        {
                            if(TargetObjects[i].Length <= 1)
                            {
                                _currentLhsObj = _currentRhsObj = null;
                                proximityState = false;
                                break;
                            }

                            for (int j = 0; j < TargetObjects[i].Length - 1 && !proximityState; j++)
                            {
                                GameObject lhs = TargetObjects[i][j];
                                for(int k = j + 1; j < TargetObjects[i].Length; k++)
                                {
                                    GameObject rhs = TargetObjects[i][k];
                                    if (distanceSquared(lhs, rhs) < onSquared)
                                    {
                                        _currentLhsObj = lhs;
                                        _currentRhsObj = rhs;
                                        proximityState = true;
                                        OnProximity.Invoke(_currentLhsObj, _currentRhsObj);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for(int i = 0; i < TargetObjects.Length - 1 && !proximityState; i++)
                        {
                            for(int j = i + 1; j < TargetObjects.Length && !proximityState; j++)
                            {
                                for(int k = 0; k < TargetObjects[i].Length && !proximityState; k++)
                                {
                                    GameObject lhs = TargetObjects[i][k];
                                    for(int l = 0; l < TargetObjects[j].Length; l++)
                                    {
                                        GameObject rhs = TargetObjects[j][l];
                                        if (distanceSquared(lhs, rhs) < onSquared)
                                        {
                                            _currentLhsObj = lhs;
                                            _currentRhsObj = rhs;
                                            proximityState = true;
                                            OnProximity.Invoke(_currentLhsObj, _currentRhsObj);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (proximityState)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
                yield return new WaitForSeconds(Period);
            }
        }

        private float distanceSquared(GameObject lhs, GameObject rhs)
        {
            Collider lhsCollider = lhs.GetComponent<Collider>();
            Collider rhsCollider = rhs.GetComponent<Collider>();
            Vector3 lhsClosestPoint, rhsClosestPoint;
            if (lhsCollider != null)
            {
                lhsClosestPoint = lhsCollider.ClosestPointOnBounds(rhs.transform.position);
            }
            else
            {
                lhsClosestPoint = lhs.transform.position;
            }
            if (rhsCollider != null)
            {
                rhsClosestPoint = rhsCollider.ClosestPointOnBounds(rhs.transform.position);
            }
            else
            {
                rhsClosestPoint = rhs.transform.position;
            }
            return (lhsClosestPoint - rhsClosestPoint).sqrMagnitude;
        }

        public void SetTargetObjects(GameObjectArray[] targets)
        {
            TargetObjects = targets;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (ShowGizmos)
            {
                if (IsActive)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                for (int i = 0; i < TargetObjects.Length; i++)
                {
                    for (int j = 0; j < TargetObjects[i].Length; j++)
                    {
                        Gizmos.DrawWireSphere(TargetObjects[i][j].transform.position, OnDistance);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireSphere(TargetObjects[i][j].transform.position, OffDistance);
                    }
                }
            }
        }
#endif
    }

    [System.Serializable]
    public class ObjectProximityEvent : UnityEvent<GameObject, GameObject> { }

    [System.Serializable]
    public class GameObjectArray
    {
        [SerializeField]
        private GameObject[] gameObjects;

        public GameObject this[int index]
        {
            get
            {
                return gameObjects[index];
            }
        }

        public int Length
        {
            get { return gameObjects == null ? 0 : gameObjects.Length; }
        }

        public GameObjectArray()
        {

        }

        public GameObjectArray(int length)
        {
            gameObjects = new GameObject[length];
        }

        public GameObjectArray(GameObject[] obj)
        {
            gameObjects = obj;
        }
    }
}