using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    public List<NarrativeNode> nodeList;

    int currentNodeIndex;
    NarrativeNode currentNode;

    void Start()
    {
        currentNode = nodeList[0];
        currentNodeIndex = 0;

        currentNode.nodeLogic.OnEnter();
    }

    public void TransitionNode(List<FlagObjects> flags)
    {
        for(int i = 0; i > flags.Count; i++)
        {
            if(flags[i].name == currentNode.flag.name)
            {
                currentNode.nodeLogic.OnExit();

                currentNode = nodeList[currentNodeIndex + 1];
                currentNodeIndex++;

                currentNode.nodeLogic.OnEnter();
                break;
            }
        }
    }

    void Update()
    {
        currentNode.nodeLogic.OnUpdate();
    }
}

[System.Serializable]
public struct NarrativeNode
{
    public DefaultNode nodeLogic;
    public FlagObjects flag;

    public NarrativeNode(DefaultNode nodeLogic, FlagObjects flag)
    {
        this.nodeLogic = nodeLogic;
        this.flag = flag;
    }
}
