using UnityEngine;

public class Lubang : MonoBehaviour
{
    public bool isLubang = false;
    public int collided;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLubang)
        {
            isLubang = true;
            collided++;
        }
    }
}
