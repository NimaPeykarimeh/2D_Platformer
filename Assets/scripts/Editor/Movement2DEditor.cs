using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Movement2D))]
public class Movement2DEditor : Editor
{


    float headerTopSpace = 50f;
    float headerBottomSpace;

    bool showAdvancedJumpSetting = false;
    bool wallSlideDebug = false;
    bool _speedDebug = false;
    public override void OnInspectorGUI()
    {
        Movement2D movement2D = (Movement2D)target;
        

        serializedObject.Update();

        GUIStyle mainHeader = movement2D.mainHeaderStyle;
        GUIStyle subHeader = movement2D.subHeaderStyle;
        mainHeader.normal.textColor = new Color(0.85f, 0.25f, 0.19f);


        headerTopSpace = EditorGUILayout.FloatField("HeaderTopSpace",headerTopSpace);
        headerBottomSpace = EditorGUILayout.FloatField("HeaderBottomSpace", headerBottomSpace);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("mainHeaderStyle"));


        Movement2D.MovingType type = (Movement2D.MovingType)serializedObject.FindProperty("movingType").enumValueIndex;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("movingType"));

        if (type == Movement2D.MovingType.Platformer)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spriteTransform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rb2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleCollider"));

            //SPEED HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("Speed Values",mainHeader);
            EditorGUILayout.Space(headerBottomSpace);


            EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"), new GUIContent("MOVEMENT SPEED","Max Speed Of The Player"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedUpAccelaration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedDownAccelaration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stopAccelaration"));

            _speedDebug = EditorGUILayout.Foldout(_speedDebug, "Debug");
            if (_speedDebug)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentHorizontalSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentVerticalSpeed"));
            }


            //DASH HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("Dash", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            SerializedProperty _dash = serializedObject.FindProperty("Dash");
            EditorGUILayout.PropertyField(_dash);
            if (_dash.boolValue)
            {

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashButton"));
            
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelDashOnWallHit"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashStopEffect"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetDashOnGround"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetDashOnWall"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("airDash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashCancelsGravity"));
           
                EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalDash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalDash"));
            
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderOffset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashParticle"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeMagni"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeFreq"));
            }
        
            EditorGUILayout.Space();


            //WALL JUMP HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("WALL JUMP/SLIDE", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            SerializedProperty _wallJump = serializedObject.FindProperty("WallJump");
            EditorGUILayout.PropertyField(_wallJump);

            if (_wallJump.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wallJumpVelocity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wallSlideSpeed"));

                wallSlideDebug = EditorGUILayout.Foldout(wallSlideDebug, "Debug");
                if (wallSlideDebug)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isSlidingOnWall"));
                }

            }

            //JUMP HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("JUMPING", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpHight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpVelocity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpUpAcceleration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpDownAcceleration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fallSpeedClamp"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fallClamp"));

            EditorGUILayout.Space();


            //JUMP ADJUSTMENTS HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("JUMP ADJUSTMENTS", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            showAdvancedJumpSetting = EditorGUILayout.Foldout(showAdvancedJumpSetting, "Advanced Jump Settings");

            if (showAdvancedJumpSetting)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpButton"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("coyoteTime"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpBuffer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onAirControl"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("variableJumpHeightDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpReleaseEffect"));
            }



            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("LEDGE GRAB", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            SerializedProperty _canGrabLedge = serializedObject.FindProperty("LedgeGrab");
            EditorGUILayout.PropertyField(_canGrabLedge);
            if (_canGrabLedge.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoClimbLedge"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheckDistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheckLayer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isLedge"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbPosList"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimpPosIndex"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isClimbingLedge"));
            }
            
            






    EditorGUILayout.Space();

            // New fields for ground check

            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("GROUND CHECK", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheckRayDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundLayer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheckCircleRadius"));

            EditorGUILayout.Space();

            // New fields for ceiling check
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("CEIL CHECK", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilCheckRayDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilLayer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilCheckCircleRadius"));

            EditorGUILayout.Space();

            // New fields for wall check
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("WALL CHECK", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheckRayDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheckLayer"));

            EditorGUILayout.Space();

            // New fields for jump debugging
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpToleranceTimer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fallToleranceTimer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isGrounded"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onCeil"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("canJump"));

            EditorGUILayout.Space();

            // New fields for general debugging
            EditorGUILayout.PropertyField(serializedObject.FindProperty("leftWallHit"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rightWallHit"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hitWall"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dist"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isJumped"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isPressedJumpButton"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalSpeedDebugger"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalSpeedDebugger"));
        }
        else if (type == Movement2D.MovingType.TopDown)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spriteTransform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rb2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleCollider"));

            //SPEED HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("Speed Values", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);


            EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"), new GUIContent("MOVEMENT SPEED", "Max Speed Of The Player"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedUpAccelaration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedDownAccelaration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stopAccelaration"));

            //DASH HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.LabelField("Dash", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            SerializedProperty _dash = serializedObject.FindProperty("Dash");
            EditorGUILayout.PropertyField(_dash);
            if (_dash.boolValue)
            {

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashButton"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelDashOnWallHit"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashStopEffect"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("airDash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashCancelsGravity"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalDash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalDash"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderOffset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashParticle"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeMagni"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashShakeFreq"));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

}