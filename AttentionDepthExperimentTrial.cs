using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using bmlTUX;
using UnityEngine;


public class AttentionDepthExperimentTrial : Trial {

    AttentionDepthExperimentRunner myRunner;

    MeshRenderer targetMesh;
    MeshRenderer coloredTargetMesh;
    MeshRenderer distractorMesh1;
    MeshRenderer distractorMesh2;
    MeshRenderer distractorMesh3;
    MeshRenderer distractorMesh4;
    MeshRenderer distractorMesh5;
    MeshRenderer distractorMesh6;
    MeshRenderer distractorMesh7;

    MeshRenderer coloredDistractorMesh1;
    MeshRenderer coloredDistractorMesh2;
    MeshRenderer coloredDistractorMesh3;
    MeshRenderer coloredDistractorMesh4;
    MeshRenderer coloredDistractorMesh5;
    MeshRenderer coloredDistractorMesh6;
    MeshRenderer coloredDistractorMesh7;

    Material distractorMaterial1;
    Material distractorMaterial2;
    Material distractorMaterial3;
    Material distractorMaterial4;
    Material distractorMaterial5;
    Material distractorMaterial6;
    Material distractorMaterial7;

    Vector3 top;
    Vector3 topRight;
    Vector3 right;
    Vector3 bottomRight;
    Vector3 bottom;
    Vector3 bottomLeft;
    Vector3 left;
    Vector3 topLeft;

    GameObject targetGameObject;
    GameObject coloredTargetGameObject;

    GameObject blueRingsGameObject;
    GameObject coloredRingsGameObject;

    GameObject distractorsGameObject1;
    GameObject distractorsGameObject2;
    GameObject distractorsGameObject3;
    GameObject distractorsGameObject4;
    GameObject distractorsGameObject5;
    GameObject distractorsGameObject6;
    GameObject distractorsGameObject7;

    GameObject colorDistractorsGameObject1;
    GameObject colorDistractorsGameObject2;
    GameObject colorDistractorsGameObject3;
    GameObject colorDistractorsGameObject4;
    GameObject colorDistractorsGameObject5;
    GameObject colorDistractorsGameObject6;
    GameObject colorDistractorsGameObject7;

    GameObject fixation;
    GameObject fixationMeshObject;

    float variableDelay = 0;

    public Vector3[] scenarios;

    public float[] randomDepths;

    GameObject blueRingsTag;
    GameObject coloredRingsTag;

    bool toggleDistractorCondition;
    float depth;

    bool userResponse;
    bool correctAnswer;

    bool minDepth;
    bool middleDepth;
    bool farDepth;

    public float distractorDepth;

    float randomDepth;
    float time = 0.0f;

    bool targetOrientationDisplay1;
    bool targetOrientationDisplay2;
    bool targetBarOrientationSame;

    public AttentionDepthExperimentTrial(ExperimentRunner runner, DataRow data) : base(runner, data) {
        myRunner = (AttentionDepthExperimentRunner)runner;  
        
        blueRingsTag = GameObject.FindGameObjectWithTag("BlueRings");
        coloredRingsTag = GameObject.FindGameObjectWithTag("ColoredRings");

        toggleDistractorCondition = (bool)Data["ToggleDistractorConditions"];
        depth = (float)Data["Depth"];

        fixation = myRunner.FixationPoint;
        fixationMeshObject = myRunner.FixationPointMeshGameObject;

        coloredRingsGameObject = myRunner.ColoredRings;
        blueRingsGameObject = myRunner.BlueRings;

        distractorsGameObject1 = myRunner.Distractor1;
        distractorsGameObject2 = myRunner.Distractor2;
        distractorsGameObject3 = myRunner.Distractor3;
        distractorsGameObject4 = myRunner.Distractor4;
        distractorsGameObject5 = myRunner.Distractor5;
        distractorsGameObject6 = myRunner.Distractor6;
        distractorsGameObject7 = myRunner.Distractor7;
        targetGameObject = myRunner.Target;
        coloredTargetGameObject = myRunner.ColoredTarget;

        colorDistractorsGameObject1 = myRunner.ColoredDistractor1;
        colorDistractorsGameObject2 = myRunner.ColoredDistractor2;
        colorDistractorsGameObject3 = myRunner.ColoredDistractor3;
        colorDistractorsGameObject4 = myRunner.ColoredDistractor4;
        colorDistractorsGameObject5 = myRunner.ColoredDistractor5;
        colorDistractorsGameObject6 = myRunner.ColoredDistractor6;
        colorDistractorsGameObject7 = myRunner.ColoredDistractor7;

        distractorMesh1 = distractorsGameObject1.GetComponent<MeshRenderer>();
        distractorMesh2 = distractorsGameObject2.GetComponent<MeshRenderer>();
        distractorMesh3 = distractorsGameObject3.GetComponent<MeshRenderer>();
        distractorMesh4 = distractorsGameObject4.GetComponent<MeshRenderer>();
        distractorMesh5 = distractorsGameObject5.GetComponent<MeshRenderer>();
        distractorMesh6 = distractorsGameObject6.GetComponent<MeshRenderer>();
        distractorMesh7 = distractorsGameObject7.GetComponent<MeshRenderer>();

        coloredDistractorMesh1 = colorDistractorsGameObject1.GetComponent<MeshRenderer>();
        coloredDistractorMesh2 = colorDistractorsGameObject2.GetComponent<MeshRenderer>();
        coloredDistractorMesh3 = colorDistractorsGameObject3.GetComponent<MeshRenderer>();
        coloredDistractorMesh4 = colorDistractorsGameObject4.GetComponent<MeshRenderer>();
        coloredDistractorMesh5 = colorDistractorsGameObject5.GetComponent<MeshRenderer>();
        coloredDistractorMesh6 = colorDistractorsGameObject6.GetComponent<MeshRenderer>();
        coloredDistractorMesh7 = colorDistractorsGameObject7.GetComponent<MeshRenderer>();
    }

    protected override void PreMethod() {

    }

    protected override IEnumerator PreCoroutine() {
        yield return null;
    }

    protected override IEnumerator RunMainCoroutine() {

        // Hardcoded depths here (adjust these values to change depth differences)
        randomDepths = new float[] { 0.52f, 0.57f, 0.62f };

        // Vectors for rings in Display 1
        top = new Vector3(0f, .2037f, .57f);
        topRight = new Vector3(.144f, .144f, .57f);
        right = new Vector3(.2037f, 0f, .57f);
        bottomRight = new Vector3(.144f, -.144f, .57f);
        bottom = new Vector3(0f, -.2037f, .57f);
        bottomLeft = new Vector3(-.144f, -.144f, .57f);
        left = new Vector3(-.2037f, 0f, .57f);
        topLeft = new Vector3(-.144f, .144f, .57f);

        // An array of the ring vectors in Display 1
        scenarios = new Vector3[] {top, topRight, right, bottomRight, bottom, bottomLeft, left, topLeft };
        
        // Independent Variable that controls whether in high load or low load condition
        bool isOneColorCondition = toggleDistractorCondition;


        // The assigned rotations for distractors bars stored in a vector
        float[] barAngleRotations = new float[] { 22.5f, 67.5f, 112.5f, 157.5f, 202.5f, 247.5f, 292.5f, 337.5f};

        // Display 1 boolean value
        bool t1 = true;

        // Low Load Condition
        if (isOneColorCondition)
        {
            // Shuffle array of Display 1 distractor positions
            Reshuffle(scenarios);

            // Pause between trials
            yield return myRunner.StartCoroutine(CoroutineDelay(1.5f));

            fixationMeshObject.SetActive(true);

            // Shows blue rings in Display 1 and deactivates the colored rings
            blueRingsTag.SetActive(true);
            coloredRingsTag.SetActive(false);

            // Assigns shuffled vectors to blue distractors in Display 1
            ReassignRingPositionsOneColor();

            // Resets the bar rotations for blue distractors
            SetZeroRotationBlueBars();

            // Resets bar rotation for target
            SetZeroRotationTarget();

            // Assigns distractor bar orientation
            AssignRandomBarAngles(barAngleRotations);

            // Assigns target bar orientation
            AssignTargetBarAngle(t1);

            // How long the first display is up for (1 seconds = 1f)
            yield return myRunner.StartCoroutine(CoroutineDelay(1f));

            // Hide blue rings when Display 1 becomes inactive
            blueRingsTag.SetActive(false);

            // This is for the ISI that was taken out. The yield return below the variable delay starts the ISI
            variableDelay = UnityEngine.Random.Range(.5f, 1f);
            //yield return myRunner.StartCoroutine(CoroutineDelay(variableDelay));

            // Shuffle array of Display 2 distractor positions
            Reshuffle(scenarios);

            // Reassigns colored distractors positions
            ReassignRingPositionsMultiColor(!t1);

            // Creates Transform of target and the color rings game object in hierarchy
            Transform targetTransform = targetGameObject.transform;
            Transform coloredRingsObject = coloredRingsGameObject.transform;

            // Transfers target game object to the multi-colored rings game object dynamically
            // It made more sense for T1 and T2 to share a target game object because of the way the blue rings and color rings were activated and deactivated each display
            TransferObjectBetweenParents(targetTransform, coloredRingsObject);

            // Rotates Fixation Point Parent Object (Parents all ring objects) so that the rings fill in the holes in Display 2
            RotateFixationPoint();

            // Reset Distractor bar rotations
            SetZeroRotationBlueBars();
            
            // Reset Target bar rotation
            SetZeroRotationTarget();

            // Assign random angles for bars for distractors
            AssignRandomBarAngles(barAngleRotations);

            // Assign vertical or horizontal bar to target
            AssignTargetBarAngle(!t1);

            // Shuffle vector array for positions of rings
            Reshuffle(scenarios);

            // Set color rings to active for Display 2
            coloredRingsTag.SetActive(true);
            coloredTargetGameObject.SetActive(false);

            // You might want to do a while-loop to wait for participant response: 
            bool waitingForParticipantResponse = true;

            while (waitingForParticipantResponse)
            {              
                //  Left mouse click for Same Target Depth Response
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    userResponse = true;
                       
                    // if the subject indicates the depths for the target in T1 and T2 are the same and they actually are, assign true value to correct answer
                    if(userResponse && targetBarOrientationSame)
                    {
                        correctAnswer = true;
                    }
                    else
                    {
                        correctAnswer = false;
                    }

                    waitingForParticipantResponse = false;
                }

                //  Right mouse click for Different Target Depth Response
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    userResponse = false;

                    // if the subject indicates the depth is different but it is the same, assign wrong answer
                    if (userResponse == false && targetBarOrientationSame == false)
                    {
                        correctAnswer = true;
                    }
                    else
                    {
                        correctAnswer = false;
                    }

                    waitingForParticipantResponse = false;


                }

                yield return null;
            }
            
            // Reassigns target and blue rings game object transforms
            targetTransform = targetGameObject.transform;
            Transform blueRingsObject = blueRingsGameObject.transform;

            // Transfers Target back to Blue ring distractors Game Object to begin display 1 on the next trial
            TransferObjectBetweenParents(targetTransform, blueRingsObject);

            // Makes Display 2 go away
            coloredRingsTag.SetActive(false);

            // Resets fixation rotation back so display 1 positioning of distractors will function properly
            RotateFixationPointBack();

            fixationMeshObject.SetActive(false);

        }

        // High load condition  //   **** Most of the code in this condition is similar to the low load condition so no need for more commenting
        if (!isOneColorCondition)
        {         
            Reshuffle(scenarios);

            yield return myRunner.StartCoroutine(CoroutineDelay(1.5f));

            fixationMeshObject.SetActive(true);

            blueRingsTag.SetActive(false);
            coloredRingsTag.SetActive(true);
            coloredTargetGameObject.SetActive(true);

            // Parameter of t1 indicates that the assignment of the position of the color rings is for Display 1
            ReassignRingPositionsMultiColor(t1);

            SetZeroRotationBlueBars();
            SetZeroRotationColoredTarget();

            AssignRandomBarAngles(barAngleRotations);
            AssignTargetBarAngle(t1);

            yield return myRunner.StartCoroutine(CoroutineDelay(1f));

            coloredRingsTag.SetActive(false);

            variableDelay = UnityEngine.Random.Range(.5f, 1f);
            //yield return myRunner.StartCoroutine(CoroutineDelay(variableDelay));

            Reshuffle(scenarios);

            coloredRingsTag.SetActive(true);

            Transform targetTransform = targetGameObject.transform;
            Transform coloredRingsObject = coloredRingsGameObject.transform;

            // makes t1 variable false and tells it to reposition the color rings for Display 2
            ReassignRingPositionsMultiColor(!t1);
            RotateFixationPoint();
            SetZeroRotationBlueBars();
            SetZeroRotationColoredTarget();

            AssignRandomBarAngles(barAngleRotations);
            AssignTargetBarAngle(!t1);

            // You might want to do a while-loop to wait for participant response: 
            bool waitingForParticipantResponse = true;

            while (waitingForParticipantResponse)
            {
                //  Left mouse click for Same Target Depth Response
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    userResponse = true;
                  
                    if (userResponse && targetBarOrientationSame)
                    {
                        correctAnswer = true;
                    }
                    else
                    {
                        correctAnswer = false;
                    }


                    waitingForParticipantResponse = false;
                }

                //  Right mouse click for Different Target Depth Response
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    userResponse = false;

                    if (userResponse == false && targetBarOrientationSame == false)
                    {
                        correctAnswer = true;
                    }
                    else
                    {
                        correctAnswer = false;
                    }

                    waitingForParticipantResponse = false;
                }

                yield return null;
            }

            targetTransform = targetGameObject.transform;
            Transform blueRingsObject = blueRingsGameObject.transform;

            coloredRingsTag.SetActive(false);

            RotateFixationPointBack();

            fixationMeshObject.SetActive(false);

        }
    }

    protected override IEnumerator PostCoroutine() {

        yield return null;
    }

    protected override void PostMethod() {
        // How to write results to dependent variables: 
        Data["Response"] = userResponse;
        Data["Correct"] = correctAnswer; 
    }

    // Shuffles the angles of the bars in an array
    float[] AngleReshuffle(float[] angleArray)
    {
        for (int t = 0; t < angleArray.Length; t++)
        {
            float tmp = angleArray[t];
            int r = UnityEngine.Random.Range(t, angleArray.Length);
            angleArray[t] = angleArray[r];
            angleArray[r] = tmp;
        }

        return angleArray;
    }

    // Shuffles the contents of the array that stores the vectors of the rings
    Vector3[] Reshuffle(Vector3[] initialVectorArray)
    {
        for (int t = 0; t < initialVectorArray.Length; t++)
        {
            Vector3 tmp = initialVectorArray[t];
            int r = UnityEngine.Random.Range(t, initialVectorArray.Length);
            initialVectorArray[t] = initialVectorArray[r];
            initialVectorArray[r] = tmp;
        }
        return initialVectorArray;
    }

    // Custom Coroutine that takes a float value as an argument to set a delay
    IEnumerator CoroutineDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }


    // Returns a random depth for the distractors from an array containing the 3 possible depths (these values are hardcoded in this script)
    float GetRandomDepth(float[] randomDepthArray)
    {
        int randomIndex = UnityEngine.Random.Range(0, randomDepthArray.Length);

        float randomDepthChosen = randomDepthArray[randomIndex];

        if (randomIndex == 0)
        {
            minDepth = true;
        }
        else
        {
            minDepth = false;
        }
        if (randomIndex == 1)
        {
            middleDepth = true;       
        }
        else
        {
            middleDepth = false;
        }
        if (randomIndex == 2)
        {
            farDepth = true;    
        }
        else
        {
            farDepth = false;
        }

        return randomDepthChosen;
    }

    // Assign rings positions for blue distractors
    void ReassignRingPositionsOneColor()
    {
        float oneColorDepth = depth;
        Reshuffle(scenarios);

        if (distractorsGameObject1 == null)
        {
            GameObject distractorsGameObject1 = myRunner.Distractor1;

            Vector3 currentDistractorPosition1 = scenarios[0];
            distractorMesh1.transform.position = currentDistractorPosition1;

            currentDistractorPosition1.z = 0.57f;  
        
            distractorMesh1.transform.position = currentDistractorPosition1;
        }
        else
        {
            Vector3 currentDistractorPosition1 = scenarios[0];
            distractorMesh1.transform.position = currentDistractorPosition1;
      
            currentDistractorPosition1.z = 0.57f;
       
            distractorMesh1.transform.position = currentDistractorPosition1;
        }

        if (distractorsGameObject2 == null)
        {
            GameObject distractorsGameObject2 = myRunner.Distractor2;

            Vector3 currentDistractorPosition2 = scenarios[1];
            distractorMesh2.transform.position = currentDistractorPosition2;

            currentDistractorPosition2.z = 0.57f;

            distractorMesh2.transform.position = currentDistractorPosition2;
        }
        else
        {

            Vector3 currentDistractorPosition2 = scenarios[1];
            distractorMesh2.transform.position = currentDistractorPosition2;

            currentDistractorPosition2.z = 0.57f;

            distractorMesh2.transform.position = currentDistractorPosition2;
        }

        if (distractorsGameObject3 == null)
        {
            GameObject distractorsGameObject3 = myRunner.Distractor3;

            Vector3 currentDistractorPosition3 = scenarios[2];
            distractorMesh3.transform.position = currentDistractorPosition3;

            currentDistractorPosition3.z = 0.57f;
       
            distractorMesh3.transform.position = currentDistractorPosition3;
        }
        else
        {

            Vector3 currentDistractorPosition3 = scenarios[2];
            distractorMesh3.transform.position = currentDistractorPosition3;

            currentDistractorPosition3.z = 0.57f;
       
            distractorMesh3.transform.position = currentDistractorPosition3;
        }
        if (distractorsGameObject4 == null)
        {
            GameObject distractorsGameObject4 = myRunner.Distractor4;

            Vector3 currentDistractorPosition4 = scenarios[3];
            distractorMesh4.transform.position = currentDistractorPosition4;

            currentDistractorPosition4.z = 0.57f;
       
            distractorMesh4.transform.position = currentDistractorPosition4;
        }
        else
        {

            Vector3 currentDistractorPosition4 = scenarios[3];
            distractorMesh4.transform.position = currentDistractorPosition4;

            currentDistractorPosition4.z = 0.57f;

            distractorMesh4.transform.position = currentDistractorPosition4;
        }
        if (distractorsGameObject5 == null)
        {
            GameObject distractorsGameObject5 = myRunner.Distractor5;

            Vector3 currentDistractorPosition5 = scenarios[4];
            distractorMesh5.transform.position = currentDistractorPosition5;

            currentDistractorPosition5.z = 0.57f;

            distractorMesh5.transform.position = currentDistractorPosition5;
        }
        else
        {
            Vector3 currentDistractorPosition5 = scenarios[4];
            distractorMesh5.transform.position = currentDistractorPosition5;

            currentDistractorPosition5.z = 0.57f;

            distractorMesh5.transform.position = currentDistractorPosition5;
        }
        if (distractorsGameObject6 == null)
        {
            GameObject distractorsGameObject6 = myRunner.Distractor6;

            Vector3 currentDistractorPosition6 = scenarios[5];
            distractorMesh6.transform.position = currentDistractorPosition6;

            currentDistractorPosition6.z = 0.57f;
            
            distractorMesh6.transform.position = currentDistractorPosition6;

        }
        else
        {

            Vector3 currentDistractorPosition6 = scenarios[5];
            distractorMesh6.transform.position = currentDistractorPosition6;

            currentDistractorPosition6.z = 0.57f;

            distractorMesh6.transform.position = currentDistractorPosition6;
        }
        if (distractorsGameObject7 == null)
        {
            GameObject distractorsGameObject7 = myRunner.Distractor7;

            Vector3 currentDistractorPosition7 = scenarios[6];
            distractorMesh7.transform.position = currentDistractorPosition7;

            currentDistractorPosition7.z = 0.57f;
 
            distractorMesh7.transform.position = currentDistractorPosition7;
        }
        else
        {

            Vector3 currentDistractorPosition7 = scenarios[6];
            distractorMesh7.transform.position = currentDistractorPosition7;

            currentDistractorPosition7.z = 0.57f;

            distractorMesh7.transform.position = currentDistractorPosition7;
        }
        if (targetGameObject == null)
        {
            GameObject targetGameObject = myRunner.Target;

            Vector3 targetPosition = scenarios[7];
            if (targetMesh == null)
            {
                targetMesh = targetGameObject.GetComponent<MeshRenderer>();
            }

            targetPosition.z = 0.57f;
            targetMesh.transform.position = targetPosition;
        }
        else
        {
            Vector3 targetPosition = scenarios[7];
            if (targetMesh == null)
            {
                targetMesh = targetGameObject.GetComponent<MeshRenderer>();
            }

            targetPosition.z = 0.57f;
            targetMesh.transform.position = targetPosition;

        }

    }

    void ReassignRingPositionsMultiColor(bool toggleDisplay1)
    {
        Vector3 currentDistractorPosition1 = scenarios[0];
        Vector3 currentDistractorPosition2 = scenarios[1];
        Vector3 currentDistractorPosition3 = scenarios[2];
        Vector3 currentDistractorPosition4 = scenarios[3];
        Vector3 currentDistractorPosition5 = scenarios[4];
        Vector3 currentDistractorPosition6 = scenarios[5];
        Vector3 currentDistractorPosition7 = scenarios[6];

        Vector3 targetPosition = scenarios[7];

        if (toggleDisplay1)
        {
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;
            coloredDistractorMesh2.transform.position = currentDistractorPosition2;
            coloredDistractorMesh3.transform.position = currentDistractorPosition3;
            coloredDistractorMesh4.transform.position = currentDistractorPosition4;
            coloredDistractorMesh5.transform.position = currentDistractorPosition5;
            coloredDistractorMesh6.transform.position = currentDistractorPosition6;
            coloredDistractorMesh7.transform.position = currentDistractorPosition7;

            currentDistractorPosition1.z = 0.57f;
            currentDistractorPosition2.z = 0.57f;
            currentDistractorPosition3.z = 0.57f;
            currentDistractorPosition4.z = 0.57f;
            currentDistractorPosition5.z = 0.57f;
            currentDistractorPosition6.z = 0.57f;
            currentDistractorPosition7.z = 0.57f;

            if (targetMesh == null)
            {
                targetMesh = coloredTargetGameObject.GetComponent<MeshRenderer>();
            }
            targetPosition.z = 0.57f;
            targetMesh.transform.position = targetPosition;
        }
        else
        {
            distractorDepth = GetRandomDepth(randomDepths);

            currentDistractorPosition1.z = depth;
            currentDistractorPosition2.z = depth;
            currentDistractorPosition3.z = depth;
            currentDistractorPosition4.z = depth;
            currentDistractorPosition5.z = depth;
            currentDistractorPosition6.z = depth;
            currentDistractorPosition7.z = depth;

            targetPosition.z = depth;
            targetMesh.transform.position = targetPosition;
        }

        coloredDistractorMesh1.transform.position = currentDistractorPosition1;
        coloredDistractorMesh2.transform.position = currentDistractorPosition2;
        coloredDistractorMesh3.transform.position = currentDistractorPosition3;
        coloredDistractorMesh4.transform.position = currentDistractorPosition4;
        coloredDistractorMesh5.transform.position = currentDistractorPosition5;
        coloredDistractorMesh6.transform.position = currentDistractorPosition6;
        coloredDistractorMesh7.transform.position = currentDistractorPosition7;

    }

    /***  Way of distributing distractors and target along different random depth

    void ReassignRingPositionsMultiColor(bool toggleDisplay1)
    {

        Vector3 currentDistractorPosition1 = scenarios[0];
        Vector3 currentDistractorPosition2 = scenarios[1];
        Vector3 currentDistractorPosition3 = scenarios[2];
        Vector3 currentDistractorPosition4 = scenarios[3];
        Vector3 currentDistractorPosition5 = scenarios[4];
        Vector3 currentDistractorPosition6 = scenarios[5];
        Vector3 currentDistractorPosition7 = scenarios[6];

        Vector3 targetPosition = scenarios[7];

        if (toggleDisplay1)
        {
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;
            coloredDistractorMesh2.transform.position = currentDistractorPosition2;
            coloredDistractorMesh3.transform.position = currentDistractorPosition3;
            coloredDistractorMesh4.transform.position = currentDistractorPosition4;
            coloredDistractorMesh5.transform.position = currentDistractorPosition5;
            coloredDistractorMesh6.transform.position = currentDistractorPosition6;
            coloredDistractorMesh7.transform.position = currentDistractorPosition7;

            currentDistractorPosition1.z = 0.57f;
            currentDistractorPosition2.z = 0.57f;
            currentDistractorPosition3.z = 0.57f;
            currentDistractorPosition4.z = 0.57f;
            currentDistractorPosition5.z = 0.57f;
            currentDistractorPosition6.z = 0.57f;
            currentDistractorPosition7.z = 0.57f;

            if(targetMesh == null)
            {
                targetMesh = coloredTargetGameObject.GetComponent<MeshRenderer>();
            }
            targetPosition.z = 0.57f;
            targetMesh.transform.position = targetPosition;
        }
        else
        {
            distractorDepth = GetRandomDepth(randomDepths);

            currentDistractorPosition1.z = distractorDepth;
            currentDistractorPosition2.z = distractorDepth;
            currentDistractorPosition3.z = distractorDepth;
            currentDistractorPosition4.z = distractorDepth;
            currentDistractorPosition5.z = distractorDepth;
            currentDistractorPosition6.z = distractorDepth;
            currentDistractorPosition7.z = distractorDepth;

            targetPosition.z = depth;
            targetMesh.transform.position = targetPosition;
        }

        coloredDistractorMesh1.transform.position = currentDistractorPosition1;
        coloredDistractorMesh2.transform.position = currentDistractorPosition2;
        coloredDistractorMesh3.transform.position = currentDistractorPosition3;
        coloredDistractorMesh4.transform.position = currentDistractorPosition4;
        coloredDistractorMesh5.transform.position = currentDistractorPosition5;
        coloredDistractorMesh6.transform.position = currentDistractorPosition6;
        coloredDistractorMesh7.transform.position = currentDistractorPosition7;

    }

    /*  OLD WAY OF DOING IT   ******   ASSIGNS EACH DISTRACTOR ITS OWN RANDOM DEPTH  ****
     *  
     *  
    void ReassignRingPositionsMultiColor(bool toggleDisplay1)
    {
        if (colorDistractorsGameObject1 == null)
        {
            GameObject colorDistractorsGameObject1 = myRunner.ColoredDistractor1;

            Vector3 currentDistractorPosition1 = scenarios[0];
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;

            if (toggleDisplay1)
            {
                currentDistractorPosition1.z = 0.57f;
            }
            else
            {
                currentDistractorPosition1.z = GetRandomDepth(randomDepths);
            }
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;  
        }
        else
        {
            Vector3 currentDistractorPosition1 = scenarios[0];
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;

            if (toggleDisplay1)
            {
                currentDistractorPosition1.z = 0.57f;
            }
            else
            {
                currentDistractorPosition1.z = GetRandomDepth(randomDepths);
            }
            coloredDistractorMesh1.transform.position = currentDistractorPosition1;
        }
        if (colorDistractorsGameObject2 == null)
        {
            GameObject colorDistractorsGameObject2 = myRunner.ColoredDistractor2;

            Vector3 currentDistractorPosition2 = scenarios[1];

            coloredDistractorMesh2.transform.position = currentDistractorPosition2;

            if (toggleDisplay1)
            {
                currentDistractorPosition2.z = 0.57f;
            }
            else
            {
                currentDistractorPosition2.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh2.transform.position = currentDistractorPosition2;

        }
        else
        {
            Vector3 currentDistractorPosition2 = scenarios[1];

            coloredDistractorMesh2.transform.position = currentDistractorPosition2;

            if (toggleDisplay1)
            {
                currentDistractorPosition2.z = 0.57f;
            }
            else
            {
                currentDistractorPosition2.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh2.transform.position = currentDistractorPosition2;
        }
        if (colorDistractorsGameObject3 == null)
        {
            GameObject colorDistractorsGameObject3 = myRunner.ColoredDistractor3;

            Vector3 currentDistractorPosition3 = scenarios[2];

            coloredDistractorMesh3.transform.position = currentDistractorPosition3;

            if (toggleDisplay1)
            {
                currentDistractorPosition3.z = 0.57f;
            }
            else
            {
                currentDistractorPosition3.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh3.transform.position = currentDistractorPosition3;
        }
        else
        {
            Vector3 currentDistractorPosition3 = scenarios[2];

            coloredDistractorMesh3.transform.position = currentDistractorPosition3;

            if (toggleDisplay1)
            {
                currentDistractorPosition3.z = 0.57f;
            }
            else
            {
                currentDistractorPosition3.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh3.transform.position = currentDistractorPosition3;
        }
        if (colorDistractorsGameObject4 == null)
        {
            GameObject colorDistractorsGameObject4 = myRunner.ColoredDistractor4;

            Vector3 currentDistractorPosition4 = scenarios[3];

            coloredDistractorMesh4.transform.position = currentDistractorPosition4;

            if (toggleDisplay1)
            {
                currentDistractorPosition4.z = 0.57f;
            }
            else
            {
                currentDistractorPosition4.z = GetRandomDepth(randomDepths);
            }
            coloredDistractorMesh4.transform.position = currentDistractorPosition4;
        }
        else
        {
            Vector3 currentDistractorPosition4 = scenarios[3];

            coloredDistractorMesh4.transform.position = currentDistractorPosition4;

            if (toggleDisplay1)
            {
                currentDistractorPosition4.z = 0.57f;
            }
            else
            {
                currentDistractorPosition4.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh4.transform.position = currentDistractorPosition4;
        }
        if (colorDistractorsGameObject5 == null)
        {
            GameObject colorDistractorsGameObject5 = myRunner.ColoredDistractor5;

            Vector3 currentDistractorPosition5 = scenarios[4];

            coloredDistractorMesh5.transform.position = currentDistractorPosition5;

            if (toggleDisplay1)
            {
                currentDistractorPosition5.z = 0.57f;
            }
            else
            {
                currentDistractorPosition5.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh5.transform.position = currentDistractorPosition5;
        }
        else
        {
            Vector3 currentDistractorPosition5 = scenarios[4];

            coloredDistractorMesh5.transform.position = currentDistractorPosition5;

            if (toggleDisplay1)
            {
                currentDistractorPosition5.z = 0.57f;
            }
            else
            {
                currentDistractorPosition5.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh5.transform.position = currentDistractorPosition5;
        }
        if (colorDistractorsGameObject6 == null)
        {
            GameObject colorDistractorsGameObject6 = myRunner.ColoredDistractor6;

            Vector3 currentDistractorPosition6 = scenarios[5];

            coloredDistractorMesh6.transform.position = currentDistractorPosition6;

            if (toggleDisplay1)
            {
                currentDistractorPosition6.z = 0.57f;
            }
            else
            {
                currentDistractorPosition6.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh6.transform.position = currentDistractorPosition6;

        }
        else
        {
            Vector3 currentDistractorPosition6 = scenarios[5];

            coloredDistractorMesh6.transform.position = currentDistractorPosition6;

            if (toggleDisplay1)
            {
                currentDistractorPosition6.z = 0.57f;
            }
            else
            {
                currentDistractorPosition6.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh6.transform.position = currentDistractorPosition6;
        }
        if (colorDistractorsGameObject7 == null)
        {
            GameObject colorDistractorsGameObject7 = myRunner.ColoredDistractor7;

            Vector3 currentDistractorPosition7 = scenarios[6];

            coloredDistractorMesh7.transform.position = currentDistractorPosition7;

            if (toggleDisplay1)
            {
                currentDistractorPosition7.z = 0.57f;
            }
            else
            {
                currentDistractorPosition7.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh7.transform.position = currentDistractorPosition7;
        }
        else
        {
            Vector3 currentDistractorPosition7 = scenarios[6];

            coloredDistractorMesh7.transform.position = currentDistractorPosition7;

            if (toggleDisplay1)
            {
                currentDistractorPosition7.z = 0.57f;
            }
            else
            {
                currentDistractorPosition7.z = GetRandomDepth(randomDepths);
            }

            coloredDistractorMesh7.transform.position = currentDistractorPosition7;
        }

        if (coloredTargetGameObject == null)
        {
            GameObject coloredTargetGameObject = myRunner.ColoredTarget;

            Vector3 targetPosition = scenarios[7];
            if (targetMesh == null)
            {
                targetMesh = coloredTargetGameObject.GetComponent<MeshRenderer>();
            }

            if (toggleDisplay1)
            {
                targetPosition.z = 0.57f;
                targetMesh.transform.position = targetPosition;
            }
            else
            {

                targetPosition.z = depth;
                targetMesh.transform.position = targetPosition;
            }               
        }
        else
        {
            Vector3 targetPosition = scenarios[7];
            if (targetMesh == null)
            {
                targetMesh = coloredTargetGameObject.GetComponent<MeshRenderer>();
            }

            if (toggleDisplay1)
            {
                targetPosition.z = 0.57f;
                targetMesh.transform.position = targetPosition;
            }
            else
            {
                targetPosition.z = depth;
                targetMesh.transform.position = targetPosition;
                
            }
        }
    }
    **************************************************/





    void RotateFixationPoint()
    {
        fixation.transform.Rotate(0f, 0f, 22.5f);
    }

    void RotateFixationPointBack()
    {
        fixation.GetComponent<MeshRenderer>().transform.rotation = Quaternion.identity;
    } 

    void SetZeroRotationBlueBars()
    {
        distractorsGameObject1.transform.rotation = Quaternion.identity;
        distractorsGameObject2.transform.rotation = Quaternion.identity;
        distractorsGameObject3.transform.rotation = Quaternion.identity;
        distractorsGameObject4.transform.rotation = Quaternion.identity;
        distractorsGameObject5.transform.rotation = Quaternion.identity;
        distractorsGameObject6.transform.rotation = Quaternion.identity;
        distractorsGameObject7.transform.rotation = Quaternion.identity;

        distractorsGameObject1.transform.Rotate(90f, 0f, 0f);
        
        distractorsGameObject2.transform.Rotate(90f, 0f, 0f);
        distractorsGameObject3.transform.Rotate(90f, 0f, 0f);
        distractorsGameObject4.transform.Rotate(90f, 0f, 0f);
        distractorsGameObject5.transform.Rotate(90f, 0f, 0f);
        distractorsGameObject6.transform.Rotate(90f, 0f, 0f);
        distractorsGameObject7.transform.Rotate(90f, 0f, 0f); 
    }

    void SetZeroRotationColoredBars()
    {
        colorDistractorsGameObject1.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject2.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject3.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject4.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject5.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject6.transform.rotation = Quaternion.identity;
        colorDistractorsGameObject7.transform.rotation = Quaternion.identity;

        colorDistractorsGameObject1.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject2.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject3.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject4.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject5.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject6.transform.Rotate(90f, 0f, 0f);
        colorDistractorsGameObject7.transform.Rotate(90f, 0f, 0f);
    }

    void SetZeroRotationColoredTarget()
    {
        coloredTargetGameObject.GetComponent<MeshRenderer>().transform.rotation = Quaternion.identity;
        targetGameObject.GetComponent<MeshRenderer>().transform.rotation = Quaternion.identity;

        coloredTargetGameObject.transform.Rotate(90f, 0f, 0f);
        targetGameObject.transform.Rotate(90f, 0f, 0f);
    }
 
    void SetZeroRotationTarget()
    {
        targetGameObject.GetComponent<MeshRenderer>().transform.rotation = Quaternion.identity;
        targetGameObject.transform.Rotate(90f, 0f, 0f);
    }

    void AssignRandomBarAngles(float[] barAnglesArray)
    {
        float[] newRandomBarRotation = AngleReshuffle(barAnglesArray);

        distractorsGameObject1.transform.RotateAround(distractorsGameObject1.transform.position, Vector3.forward, newRandomBarRotation[0]);
        distractorsGameObject2.transform.RotateAround(distractorsGameObject2.transform.position, Vector3.forward, newRandomBarRotation[1]);
        distractorsGameObject3.transform.RotateAround(distractorsGameObject3.transform.position, Vector3.forward, newRandomBarRotation[2]);
        distractorsGameObject4.transform.RotateAround(distractorsGameObject4.transform.position, Vector3.forward, newRandomBarRotation[3]);
        distractorsGameObject5.transform.RotateAround(distractorsGameObject5.transform.position, Vector3.forward, newRandomBarRotation[4]);
        distractorsGameObject6.transform.RotateAround(distractorsGameObject6.transform.position, Vector3.forward, newRandomBarRotation[5]);
        distractorsGameObject7.transform.RotateAround(distractorsGameObject7.transform.position, Vector3.forward, newRandomBarRotation[6]);


        colorDistractorsGameObject1.transform.RotateAround(colorDistractorsGameObject1.transform.position, Vector3.forward, newRandomBarRotation[0]);
        colorDistractorsGameObject2.transform.RotateAround(colorDistractorsGameObject2.transform.position, Vector3.forward, newRandomBarRotation[1]);
        colorDistractorsGameObject3.transform.RotateAround(colorDistractorsGameObject3.transform.position, Vector3.forward, newRandomBarRotation[2]);
        colorDistractorsGameObject4.transform.RotateAround(colorDistractorsGameObject4.transform.position, Vector3.forward, newRandomBarRotation[3]);
        colorDistractorsGameObject5.transform.RotateAround(colorDistractorsGameObject5.transform.position, Vector3.forward, newRandomBarRotation[4]);
        colorDistractorsGameObject6.transform.RotateAround(colorDistractorsGameObject6.transform.position, Vector3.forward, newRandomBarRotation[5]);
        colorDistractorsGameObject7.transform.RotateAround(colorDistractorsGameObject7.transform.position, Vector3.forward, newRandomBarRotation[6]);
    }

    void AssignTargetBarAngle(bool display1)
    {
        bool isVertical = (UnityEngine.Random.value > 0.5f);

        if (isVertical)
        {
            targetGameObject.transform.RotateAround(targetGameObject.transform.position, Vector3.forward, 0);
            coloredTargetGameObject.transform.RotateAround(coloredTargetGameObject.transform.position, Vector3.forward, 0);
        }
        else
        {
            targetGameObject.transform.RotateAround(targetGameObject.transform.position, Vector3.forward, 90);
            coloredTargetGameObject.transform.RotateAround(coloredTargetGameObject.transform.position, Vector3.forward, 90);
        }


        if (display1 && isVertical)
        {
            targetOrientationDisplay1 = true;
        }
        else if (display1 && !isVertical)
        {
            targetOrientationDisplay1 = false;
        }

        if (!display1 && isVertical)
        {
            targetOrientationDisplay2 = true;
        }
        else if (!display1 && !isVertical)
        {
            targetOrientationDisplay2 = false;
        }

        if (targetOrientationDisplay1 == targetOrientationDisplay2)
        {
            targetBarOrientationSame = true;

        }
        else
        {
            targetBarOrientationSame = false;
        }

    }

    // Transfers a child object to a different parent in the Unity hierarchy
    void TransferObjectBetweenParents(Transform targetPos1, Transform targetPos2)
    {
        targetPos1.transform.parent = targetPos2.transform;
    }

}




