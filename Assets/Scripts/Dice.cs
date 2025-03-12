using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;

    private bool isRolling = true;
    private float restTimer = 0f;
    public float velocityThreshold = 0.05f;
    public float restDuration = 1.0f;

    private Mesh mesh;
    public Vector3[] faceNormals;
    public int[] faceValues;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshFilter>().mesh;

        InitializeFaceNormals();
    }

    void InitializeFaceNormals()
    {
        faceNormals = new Vector3[20];
        faceValues = new int[20];

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        int[] correctNumbers = new int[]
            {
                5,
                3,
                16,
                18,
                11,
                8,
                17,
                13,
                10,
                4,
                9,
                20,
                12,
                19,
                7,
                6,
                15,
                2,
                14,
                1
            };
        for (int i = 0; i < 20; i++)
        {
            // Get the vertices of this face
            Vector3 v1 = vertices[triangles[(i * 3) % triangles.Length]];
            Vector3 v2 = vertices[triangles[(i * 3 + 1) % triangles.Length]];
            Vector3 v3 = vertices[triangles[(i * 3 + 2) % triangles.Length]];

            // Calculate face normal
            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
            faceNormals[i] = normal;
            faceValues[i] = correctNumbers[i];
        }

        // Make sure normals are normalized
        for (int i = 0; i < faceNormals.Length; i++)
        {
            faceNormals[i] = faceNormals[i].normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < velocityThreshold && rb.angularVelocity.magnitude < velocityThreshold)
        {
            restTimer += Time.deltaTime;
            if (restTimer >= restDuration && isRolling)
            {
                isRolling = false;
                int result = GetFaceUp();
                Debug.Log("Dice Result: " + result);
            }
        }
        else
        {
            isRolling = true;
            restTimer = 0f;
        }
    }

    int GetFaceUp()
    {
        Vector3 worldUp = Vector3.up;

        float highestDot = -1f;
        int topFace = 0;


        for (int i = 0; i < faceNormals.Length; i++)
        {
            // Convert local face normal to world space
            Vector3 worldNormal = transform.TransformDirection(faceNormals[i]);

            // Calculate alignment with world up
            float dot = Vector3.Dot(worldNormal, worldUp);

            if (dot > highestDot)
            {
                highestDot = dot;
                topFace = faceValues[i];
            }
        }

        return topFace;
    }

    void OnDrawGizmos()
    {
        if (faceNormals != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < faceNormals.Length; i++)
            {
                // Draw normal ray
                Vector3 worldNormal = transform.TransformDirection(faceNormals[i]);
                Vector3 endpoint = transform.position + worldNormal * 0.1f;
                Gizmos.DrawRay(transform.position, worldNormal * 0.1f);

                // Only visible in Scene view when object is selected
                #if UNITY_EDITOR
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(endpoint, faceValues[i].ToString());
                #endif
            }
        }
    }
}
