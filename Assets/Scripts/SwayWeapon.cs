using UnityEngine;

public class SwayWeapon : MonoBehaviour
{
    [SerializeField] float intensity = 10.0f; 
    [SerializeField] float amplitudeHorizontal = 5.0f; 
    [SerializeField] float amplitudeVertical = 2.0f; 

    Vector3 nextSwayVector;
    Vector3 nextSwayPosition;
    Vector3 startLocalPosition;

    void Start()
    {
        nextSwayVector = new Vector3(amplitudeHorizontal, amplitudeVertical, 0.0f);
        nextSwayPosition = transform.localPosition + nextSwayVector;
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (Movement.isMove) 
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextSwayPosition, intensity * Time.deltaTime);

            if (Vector3.SqrMagnitude(transform.localPosition - nextSwayPosition) < 0.01f)
            {
                nextSwayVector = -nextSwayVector;
                nextSwayPosition = startLocalPosition + nextSwayVector;
            }
        }
        else
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startLocalPosition, intensity * Time.deltaTime);
    }
}

