using UnityEngine;

public class StoryDebugger : MonoBehaviour
{
    public StoryStructure structure;

    public void ReadStory()
    {
        Debug.Log("Next story: " + structure.GetNextStory().story);
    }
}
