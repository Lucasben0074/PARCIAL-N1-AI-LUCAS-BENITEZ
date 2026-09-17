using UnityEngine;

public class WrapAround : MonoBehaviour
{
    [SerializeField] private float limitX = 15f;
    [SerializeField] private float limitZ = 15f;

    private void Update()
    {
        Vector3 position = transform.position;

        if (position.x > limitX)
        {
            position.x = -limitX;
        }
        else if (position.x < -limitX)
        {
            position.x = limitX;
        }

        if (position.z > limitZ)
        {
            position.z = -limitZ;
        }
        else if (position.z < -limitZ)
        {
            position.z = limitZ;
        }

        transform.position = position;
    }
}