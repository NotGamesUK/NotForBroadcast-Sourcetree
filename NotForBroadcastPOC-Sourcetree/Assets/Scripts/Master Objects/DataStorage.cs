using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DataStorage : MonoBehaviour {

    [System.Serializable]
    public class SequenceData
    {
        public string SequenceName;
        public float runIn = 30;
        public float runOut = 15;
        public VideoClip[] screenVideo;
        public VideoClip[] playbackVideo;
        public AudioClip[] screenAudio;
        public LevelController interferenceLevel;
        public AudioClip AudioInterference;
        public VideoClip resistanceVideo;
        public AudioClip resistanceAudio;
        public ScoringController.ScoringMode scoringMode;



        public string GetName()
        {
            return SequenceName;
        }

        public VideoClip GetVideo(int thisScreen)
        {
            return screenVideo[thisScreen];
        }

        public AudioClip GetAudio(int thisScreen)
        {
            return screenAudio[thisScreen];
        }


    }

    public SequenceData[] sequenceData;
}
