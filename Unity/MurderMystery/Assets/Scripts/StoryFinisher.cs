using UnityEngine;

public class StoryFinisher : MonoBehaviour
{
    
    public void FinishCurrentStory()
    {
        StoryStructure.Instance.FinishCurrentStory();
    }
}
