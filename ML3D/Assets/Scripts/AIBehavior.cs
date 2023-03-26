using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AIBehavior : Agent
{
    [SerializeField] private Transform goalTransform;
    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goalTransform);

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        float xPos = actions.ContinuousActions[0];
        float zPos = actions.ContinuousActions[1];

        transform.position += new Vector3(xPos, 0, zPos) * Time.deltaTime * 5f;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Goal")
        {
            SetReward(+10f);
            EndEpisode();
        }
        else if (coll.gameObject.tag == "Wall")
        {
            AddReward(-10f);
        }
    }
}
