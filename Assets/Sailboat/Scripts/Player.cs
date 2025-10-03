using UnityEngine;
using Pixelplacement;
public class Player : Singleton<Player>
{
    [SerializeField]
    protected Entity playersEntity;
    [SerializeField]
    StateMachine stateMachine;
    //TODO optimize?
    public bool entityIsPlayer(Entity entity)
    {
        return entity == playersEntity;
    }

    [ContextMenu("Switch to next vehicle")]
    public void switchNext()
    {
        stateMachine.Next();
    }

    public void switchVehicle(VehicleType vehicleType)
    {
        Vector2 pos = bodyTransform.position;
        stateMachine.ChangeState((int)vehicleType);
        //MoveBody(pos);
    }

    

    //TODO optimize!!!! make events and stuff, fix that body is the just a first child
    public Transform bodyTransform { get => stateMachine.currentState.transform.GetChild(0); }

    public void MoveBody(Vector2 newPosition)
    {
        foreach (var rb in bodyTransform.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.position = newPosition;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}

[System.Serializable]
public enum VehicleType { SquareRigRaft, SquareRigBoat, ForeAndAft, None }
