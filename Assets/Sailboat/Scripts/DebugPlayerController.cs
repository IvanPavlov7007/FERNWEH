using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class DebugPlayerController : MonoBehaviour
    {
        [SerializeField]
        VehicleType currentVehicleType = VehicleType.SquareRigBoat;

        void Start()
        {
            G.Instance.debugInputController.onChangeVehicle += ChangeVehicle;
        }

        void ChangeVehicle()
        {
            switch (currentVehicleType)
            {
                case VehicleType.SquareRigRaft:
                    currentVehicleType = VehicleType.SquareRigBoat;
                    break;
                case VehicleType.SquareRigBoat:
                    currentVehicleType = VehicleType.ForeAndAft;
                    break;
                case VehicleType.ForeAndAft:
                    currentVehicleType = VehicleType.SquareRigRaft;
                    break;
                default:
                    break;
            }
            G.Instance.player.switchVehicle(currentVehicleType);
        }
    }
}