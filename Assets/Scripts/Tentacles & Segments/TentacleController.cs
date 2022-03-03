using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//this class will require a line render and tentacle state manager
[RequireComponent(typeof(LineRenderer), typeof(TentacleStateManager))]

//this class handles the controlling of tentacles, with the segment updating/ai positioning being here so we dont have to copy/paste code in many different states.
public class TentacleController : MonoBehaviour
{
    //quick reference to the wall layer mask
    [SerializeField] private LayerMask wallLayer;

    //line render
    private LineRenderer line = null;
    //agent
    private NavMeshAgent agent = null;
    //tentacle spawner
    private TentacleSpawner spawner = null;
    //list of all segments currently being used
    private List<Segment> segments = new List<Segment>();
    //list of all segments not being used right now
    private List<Segment> queuedSegments = new List<Segment>();
    //list of all agent tracked positions based on the length of segments
    private List<Vector3> agentTrackedPositions = new List<Vector3>();
    //array for the segment velocity
    private Vector2[] segmentVelocity = new Vector2[0];
    //segment count
    private int segmentCount = 0;

    //property to check if the tentacle is setup or not
    public bool setup { get; private set; }
    //property to check if the tentacle is occupied
    public bool occupied { get; set; }
    //reference to the tentacle property
    public TentacleProperties properties { get; set; }
    //refernce to the state manager
    public TentacleStateManager stateManager { get; private set; }

    private int index = 1;
    [HideInInspector]
    public float targetRotation;

    private void Awake()
    {
        //assign all the components 
        line = GetComponent<LineRenderer>();
        agent = GetComponentInChildren<NavMeshAgent>();
        stateManager = GetComponent<TentacleStateManager>();
    }


    //method for initializing tentacles for use.
    public void InitializeTentacles()
    {
        //check if we have no properties, if so dont proceed
        if (properties == null)
            return;

        //if we have segments, clear it
        if (segments.Count > 0)
            segments.Clear();

        //if we have queued segments, clear it
        if (queuedSegments.Count > 0)
            queuedSegments.Clear();
        
        //if we have tracked positions, clear it
        if (agentTrackedPositions.Count > 0)
            agentTrackedPositions.Clear();

        //loop through the amount of segments this tentacle will have
        for (int i = 0; i < properties.tentacleSegments; i++)
        {
            //create new instances of the segment class and add them to the list.
            segments.Add(new Segment(i, properties));
        }

        //assign the length of the velocity array
        segmentVelocity = new Vector2[segments.Count];
        //assign the position count of the line render
        line.positionCount = properties.tentacleSegments;
        //we are not setup for use.
        setup = true;
    }
    
    //method used for updating the amount of segments currently needed based on the distance of the player and the anchor point.
    public void UpdateSegmentCount()
    {
        //calculate dist between player and anchor
        float dist = (Game.instance.player.GetPosition() - spawner.spawnPoint).magnitude;
        //calculate segment count based on the distance, casted it to an int
        segmentCount = (int)(dist / properties.lengthBetweenSegments);
        //clamp the segment count between 0 and the max segments this tentacle will have
        segmentCount = Mathf.Clamp(segmentCount, 0, properties.tentacleSegments);
    }

    //method used for updating segment positions
    public void UpdateSegmentPositions()
    {
        //set the first segment to the anchor point
        segments[0].position = spawner.spawnPoint;
        //loop through all the segments expect for the first one, as we set it just before.
        for (int i = 1; i < segments.Count; i++)
        {
            /*/
            //if the amount of segments is greater then the amount of segments needed
            if (i >= segmentCount)
            {
                //set their position 
                segments[i].position = agent.nextPosition;

                //add it to the queued list
                queuedSegments.Add(segments[i]);
                //remove it from the active list
                segments.Remove(segments[i]);
                continue;
            }
            /*/

            //if we can move this segment

            //set the current segment and previous segment
            Segment currentSeg = segments[i];
            Segment previousSeg = segments[i - 1];

            //update the current segment position with the given arguments
            Vector2 segPos = currentSeg.UpdatePosition(previousSeg, agent.nextPosition, wallLayer, i * targetRotation);

            //move the position to the target position using smooth damp
            segments[i].position = Vector2.SmoothDamp(segments[i].position, segPos, ref segmentVelocity[i],  properties.tentacleMoveSpeed);    
        }

        //set the position count and positions of the line render
        line.positionCount = segmentCount;
        line.SetPositions(GetSegmentPositions());
    }

    //method used for updating the agent tracked positions
    public void UpdateAgentTrackedPositions()
    {
        //check if we can add
        bool canAdd = agentTrackedPositions.Count < segmentCount;

        if(canAdd && !agentTrackedPositions.Contains(agent.transform.position))
        {
            //if everything is clear, add the current position of the agent to tracked list.
            agentTrackedPositions.Add(agent.transform.position);
        }
    }

    //method used for updating queued segments
    public void UpdateQueuedSegments()
    {
        //if there is a difference in our active segments and the needed segments
        if (segments.Count != segmentCount)
        {
            //loop through all the queued segments
            for (int i = 0; i < queuedSegments.Count; i++)
            {
                //add them back to the active list and remove them from the queued list.
                segments.Add(queuedSegments[i]);
                queuedSegments.Remove(queuedSegments[i]);
            }
        }
    }

    //method used for setting the destination of the agent
    public void UpdateAgentPosition(Vector3 position)
    {
        if (agent == null)
            return;

        //set the destination to the given pos
        agent.SetDestination(position);
    }

    //method used for hard setting the agent position, instead of pathfinding to the destination
    public void SetAgentPosition(Vector3 position)
    {
        if (agent == null)
            return;

        //set the transform position to the given position
        agent.transform.position = position;
    }

    //set a new anchor or spawner (hole) for this tentacle
    public void SetNewAnchor(TentacleSpawner spawner)
    {
        Vector3 anchor = spawner.spawnPoint;

        for (int i = 0; i < segments.Count; i++)
        {
            //set all the segment positions to the anchor
            segments[i].position = anchor;
        }

        //bad code, but only way to make it work and im to lazy to figure out another way lol
        agent.enabled = false;
        agent.transform.position = anchor;
        agent.enabled = true;

        //set the spawner
        this.spawner = spawner;
    }

    //method used for resetting a tentacle to square one
    public void ResetTentacle()
    {
        //reset all the variables
        spawner.occupied = false;
        occupied = false;
        spawner = null;
        line.positionCount = 0;
    }

    //method used for converting all the segments to a vector array
    private Vector3[] GetSegmentPositions()
    {
        List<Vector3> seg = new List<Vector3>();

        for (int i = 0; i < segments.Count; i++)
        {
            seg.Add(segments[i].position);
        }

        return seg.ToArray();
    }


    //method used for finding the next agent position the segment i will use.
    public Vector3 GetTrackedPosition(int i)
    {
        //check to make sure i is within the bounds of the tracked position
        if (i >= agentTrackedPositions.Count - 1)
        {
            if (agentTrackedPositions.Count <= 0)
                return agent.nextPosition;

            Vector3 pos = agentTrackedPositions[agentTrackedPositions.Count - 1];
            return pos;
        }

        //if everything is clear, set the agentPos to the corresponding agent tracked pos, and remove it from the list
        Vector3 agentPos = agentTrackedPositions[i];
        agentTrackedPositions.Remove(agentPos);

        //return the next position
        return agentPos;
    }

    //helper methods
    public Segment GetLastTentacle() 
    {
        return segments[segments.Count - 1];
    }
    public Vector3 GetTentacleEndPoint()
    {
      return  segments[segments.Count - 1].position;
    }
    public float GetDistanceBetweenAgentAndEndPoint()
    {
        return (agent.transform.position - GetTentacleEndPoint()).magnitude;
    }
    public float GetDistanceBetweenPlayerAndEndPoint()
    {
        return (GetTentacleEndPoint() - Game.instance.player.GetPosition()).magnitude;
    }
    public float GetDistanceBetweenEndPointAndAnchor()
    {
       return (spawner.spawnPoint - GetTentacleEndPoint()).magnitude;
    }
    public TentacleProperties GetTentacleProperties()
    {
        return properties;
    }
    public Vector3 GetAnchorPosition()
    {
       return spawner.spawnPoint;
    }
    public Vector3 GetAgentPosition()
    {
       return agent.nextPosition;
    }
    public float GetDistanceBetweenPlayerAndAnchor()
    {
        return (spawner.spawnPoint - Game.instance.player.GetPosition()).magnitude;
    }
}

