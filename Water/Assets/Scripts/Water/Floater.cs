using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public GameObject water;
    private WaterGrid waterGrid;
    private Grid grid;
    private GridVertex[,] gridArray;
    
    private float depthBeforeSubmerged = 2.5f;
    private float displacementAmount = 1f;
    public int floaterCount;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    public float dt;

    public void Start()
    {
        waterGrid = water.GetComponent<WaterGrid>();
        grid = waterGrid.water_grid;
        gridArray = waterGrid.gridArray;
        Time.fixedDeltaTime = dt;
    }
    
    private void FixedUpdate()
    {
        //NOT REALLY GOOD GRAVITY - dividing by mass to make gravity effect smaller - even though in reality it should be higher
        //Done this just cause it makes it more realistic
        rigidBody.AddForceAtPosition(new Vector3(0f, Physics.gravity.y / (rigidBody.mass * floaterCount), 0f), transform.position, ForceMode.Acceleration);
        Vector3Int cellPos = grid.LocalToCell(transform.position);
        float waveHeight = gridArray[cellPos.x, cellPos.z].GetVertexPosition().y;
        if (transform.position.y < waveHeight)
        {
            print("Wave height: " + waveHeight);
            print("Y Position: " + transform.position.y);
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
