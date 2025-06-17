using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 대상
    public Vector3 offset; // offset: 카메라와 대상 간의 위치 차이

    void Update()
    {
        transform.position = target.position + offset; // 대상의 위치에 offset을 더한 위치로 카메라 이동
    }
}