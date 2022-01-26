using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    public List<PlayableDirector> playableDirectors;
    public List<TimelineAsset> timelines;


    public void Play()
    {
        foreach (PlayableDirector playableDirector in playableDirectors)
        {
            playableDirector.Play();
        }
    }

    public void PlayFromTimelines(int index)
    {
        TimelineAsset selectedAsset;

        if (timelines.Count <= index)
        {
            selectedAsset = timelines[timelines.Count - 1];
        }
        else
        {
            selectedAsset = timelines[index];
        }

        playableDirectors[0].Play(selectedAsset);
    }


    void Update()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entites = entityManager.GetAllEntities();

        for (int i = 0; i < entites.Length; i++)
        {
            var hasDialog = entityManager.HasComponent<NpcShowDialogComponent>(entites[i]);

            if (hasDialog)
            {
                //  var npcData = entityManager.GetComponentData<NpcComponent>(entites[i]);
                //todo liwen 
                Play();
                break;
      
             
            }
        }

        entites.Dispose();
    }


}
