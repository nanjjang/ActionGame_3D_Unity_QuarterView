using UnityEngine;
public class Rewards : MonoBehaviour
{
    public GameObject[] chests;
    public GameObject Key;
    Rigidbody rb;
    public void Visible(int index)
    {
        chests[index].gameObject.SetActive(true);
    }
    public void KeyDrop( GameObject keyPos)
    {
        GameObject key = Instantiate(Key,keyPos.transform.position,keyPos.transform.rotation);
        rb = key.GetComponent<Rigidbody>();
        rb.AddForce(-keyPos.transform.forward * 20f, ForceMode.Impulse);
    }
}