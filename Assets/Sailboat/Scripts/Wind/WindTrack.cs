using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Those structures are currently out of use
namespace Sailboat
{

    public interface CartesianTrack
    {
        public Vector2 getWindSpeed(float time);
    }

    public interface PolarTrack
    {
        public float angle(float time);
        public float magnitude(float time);
    }

    public abstract class WindTrack : CartesianTrack
    {
        public abstract Vector2 getWindSpeed(float time);

        public static ConstantWindTrack NorthWind = new ConstantWindTrack(Vector2.up);
        public static ConstantWindTrack SouthWind = new ConstantWindTrack(Vector2.down);
        public static ConstantWindTrack EastWind = new ConstantWindTrack(Vector2.right);
        public static ConstantWindTrack WestWind = new ConstantWindTrack(Vector2.left);

    }

    public class ConstantWindTrack : WindTrack
    {
        public readonly Vector2 direction;

        public ConstantWindTrack(Vector2 direction)
        {
            this.direction = direction;
        }

        public override Vector2 getWindSpeed(float time)
        {
            return direction;
        }
    }
}