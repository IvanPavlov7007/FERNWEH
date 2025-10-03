using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{

    /// <summary>
    /// Animator of multiple WindTracks-Clips. Another approach for managing wind I started but thrown away
    /// </summary>
    public class DontUseWind : MonoBehaviour
    {
        public float animatorTime { get; set; }

        //could also be WindTrack -_-
        public Vector2 currentWind { get; private set; }

        //useless stuff remove that sheaaat
        class TrackContext
        {
            public readonly WindTrack track;
            public bool active = true;
            public float time_elapsed = 0f;
            public float alpha = 1f;

            public TrackContext(WindTrack track)
            {
                this.track = track;
            }
        }

        List<TrackContext> windTracks = new List<TrackContext>();


        public void addWind(WindTrack windTrack)
        {
            windTracks.Add(new TrackContext(windTrack));
        }


        // use events( callbacks) to link each fixedUpdate to respective changes in gameObjects?
        private void FixedUpdate()
        {
            UpdateDeltaTime(Time.fixedDeltaTime);
        }

        private void UpdateDeltaTime(float deltaTime)
        {
            Vector2 windValue = Vector2.zero;
            foreach (var context in windTracks)
            {
                if (!context.active)
                    continue;
                windValue += context.alpha * context.track.getWindSpeed(context.time_elapsed + deltaTime);

                //updating elapsed Time
                context.time_elapsed += deltaTime;
            }
            currentWind = windValue;
        }
    }
}