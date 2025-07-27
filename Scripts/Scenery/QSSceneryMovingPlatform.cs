using Unity.VisualScripting;
using UnityEngine;


namespace QuickStart
{

    public class QSSceneryMovingPlatform : MonoBehaviour
    {
        public Transform visuals;
        public Transform collisions;

        public bool waitForPlayer = false;

        public Interpolation movementInterpolation;

        public Vector2 originPosition;
        public Vector2 destinationPosition;

        public bool beginAtRandomPosition = false;

        [Range(0, 1)]
        public float beginningPosition = 0.5f;

        public bool moveTowardsEndFirst = false;

        [Tooltip("Units/second")]
        public float movementSpeed = 1;

        public float waitTimeAtArrival = 1;


        private bool isComingBack = false;
        private bool isWaiting = false;
        private float timer = 0;

        private float interpolationSpeed;

        private void Start()
        {
            if (!collisions.GetComponent<QSSceneryMovingPlaformSetPlayerAsParent>())
                collisions.AddComponent<QSSceneryMovingPlaformSetPlayerAsParent>();

            interpolationSpeed = 1.0f / Vector2.Distance(originPosition, destinationPosition) * movementSpeed;

            if (beginAtRandomPosition)
            {
                timer = Random.value;
                isComingBack = Random.value > 0.5f;
                return;
            }

            timer = beginningPosition;
            if (moveTowardsEndFirst) isComingBack = true;
        }

        private void Update()
        {
            if (isWaiting)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    isWaiting = false;
                }

                return;
            }

            timer += Time.deltaTime * interpolationSpeed;


            Vector2 A = isComingBack ? destinationPosition : originPosition;
            Vector2 B = isComingBack ? originPosition : destinationPosition;

            Vector2 currentPosition = movementInterpolation.LerpWithInterpolation(timer, A, B);

            visuals.position = currentPosition;

            if (timer >= 1)
            {
                timer = 0;

                //Recalculate speed in case one of the positions has changed
                interpolationSpeed = 1.0f / Vector2.Distance(originPosition, destinationPosition) * movementSpeed;


                if (waitTimeAtArrival > 0)
                {
                    timer = waitTimeAtArrival;
                    isWaiting = true;
                }

                isComingBack = !isComingBack;
            }
        }

        private void FixedUpdate()
        {
            if (isWaiting) return;

            collisions.position = visuals.position;
        }

    }


}