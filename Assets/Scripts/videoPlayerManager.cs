using UnityEngine;
using UnityEngine.Video;
using System.Linq;
using System.Collections.Generic;

public class VideoPlayerManager : MonoBehaviour
{
    public Transform player;
}
    //public List<VideoPlayer> videoPlayers = new List<VideoPlayer>();

//     public int activeCount = 5;

//     void Update()
//     {
//         if(player == null) return;
// if (videoPlayers==null)
// {
//     return;
// }
//         // Sort videos by distance from player
//         var sorted =
//             videoPlayers
//             .OrderBy(v =>
//                 Vector3.Distance(
//                     player.position,
//                     v.transform.position
//                 ))
//             .ToList();

//         for(int i = 0; i < sorted.Count; i++)
//         {
//             if(i < activeCount)
//             {
//                 if(!sorted[i].isPlaying)
//                     sorted[i].Play();
//             }
//             else
//             {
//                 if(sorted[i].isPlaying)
//                     sorted[i].Pause();
//             }
//         }
//     }
// }