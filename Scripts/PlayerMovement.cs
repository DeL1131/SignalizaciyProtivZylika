using UnityEngine;

public class Movement : MonoBehaviour
{
    public readonly string Horizontal = "Horizontal";
    public readonly string Vertical = "Vertical";

    [SerializeField] private float _speed;

    private void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis(Horizontal), 0f, Input.GetAxis(Vertical));

        transform.Translate(_speed * Time.deltaTime * -direction);
    }
}