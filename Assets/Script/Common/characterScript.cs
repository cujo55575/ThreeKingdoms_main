using UnityEngine;

public class characterScript : MonoBehaviour
{
    public void increaseSize()
    {
        gameObject.transform.localScale = new Vector3(0.8f, 15f, 0.8f);
    }
    public void decreaseSize()
    {
        gameObject.transform.localScale = new Vector3(0.4f, 7.5f, 0.4f);
    }
}
