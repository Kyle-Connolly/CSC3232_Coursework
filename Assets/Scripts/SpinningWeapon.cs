using UnityEngine;

public class SpinningWeapon : MonoBehaviour
{
    private Vector3 _spinSpeed = new Vector3(0f, 100f, 0f);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_spinSpeed * Time.deltaTime);
    }
}
