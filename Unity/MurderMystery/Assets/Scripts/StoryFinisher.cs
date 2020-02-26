using UnityEngine;

public class StoryFinisher : MonoBehaviour
{

    public void SendInteraction(string interaction)
    {
        StoryStructure.Instance.Interact(interaction);
    }
    
    public void FinishCurrentStory()
    {
        StoryStructure.Instance.FinishCurrentStory();
    }
}
