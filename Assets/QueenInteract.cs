using UnityEngine;

public class QueenInteract : Dialog
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void TakeFood() {
        Pickup food = PlayerInteract.Instance.Drop();
        food.transform.parent = transform;
        food.GetComponent<Collider2D>().enabled = false;
        food.GetComponent<Rigidbody2D>().isKinematic = true;
        food.transform.localPosition = Vector3.up;
    }
    
    public override void OnInteract(PlayerInteract player) {
        Debug.Log(PlayerInteract.Instance.PickupType);
        switch (PlayerInteract.Instance.PickupType) {
            case PickupType.Food:
                TakeFood();
                ReadOut("yummi!", "whatever");
                break;
            case PickupType.Larva:
                break;
            case PickupType.Leaf:
                break;
            case PickupType.None:
                break;
        }
        
    }
}
