using UnityEngine;
using UnityEditor;

public class GenerateHeartBubbleParticle
{
    [MenuItem("Tools/Generate Heart Bubble Particle")]
    static void CreateHeartBubble()
    {
        GameObject root =
        new GameObject("HeartBubble");


        CreateBubbleShell(root.transform);

        CreateInnerHearts(root.transform);


        Selection.activeGameObject = root;

        Debug.Log(
            "Heart bubble particle created successfully 💗"
        );
    }


    static void CreateBubbleShell(Transform parent)
    {
        GameObject shell =
        new GameObject("BubbleShellParticles");

        shell.transform.SetParent(parent);

        ParticleSystem ps =
        shell.AddComponent<ParticleSystem>();


        var main = ps.main;

        main.duration = 2.5f;

        main.loop = false;

        main.startLifetime = 2.5f;

        main.startSpeed = 0f;

        main.startSize = 1.6f;

        main.simulationSpace =
        ParticleSystemSimulationSpace.World;


        main.startColor =
        new Color(1f, 0.7f, 0.9f, 0.4f);


        var shape = ps.shape;

        shape.enabled = true;

        shape.shapeType =
        ParticleSystemShapeType.Sphere;

        shape.radius = 0.4f;


        var velocity =
        ps.velocityOverLifetime;

        velocity.enabled = true;

        velocity.y = 0.4f;


        var colorLifetime =
        ps.colorOverLifetime;

        colorLifetime.enabled = true;

        Gradient grad =
        new Gradient();

        grad.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(
                    Color.white,
                    0
                )
            },

            new GradientAlphaKey[]
            {
                new GradientAlphaKey(
                    0.4f,
                    0
                ),

                new GradientAlphaKey(
                    0f,
                    1
                )
            }
        );

        colorLifetime.color =
        new ParticleSystem.MinMaxGradient(
            grad
        );


        var sizeLifetime =
        ps.sizeOverLifetime;

        sizeLifetime.enabled = true;

        AnimationCurve curve =
        new AnimationCurve();

        curve.AddKey(0, 0.7f);

        curve.AddKey(1, 1.1f);

        sizeLifetime.size =
        new ParticleSystem.MinMaxCurve(
            1,
            curve
        );


        var renderer =
        ps.GetComponent<ParticleSystemRenderer>();

        renderer.renderMode =
        ParticleSystemRenderMode.Billboard;
    }


    static void CreateInnerHearts(Transform parent)
    {
        GameObject inner =
        new GameObject("InnerHeartParticles");

        inner.transform.SetParent(parent);


        ParticleSystem ps =
        inner.AddComponent<ParticleSystem>();


        var main = ps.main;

        main.duration = 2.5f;

        main.loop = false;

        main.startLifetime = 1f;

        main.startSpeed = 0.2f;

        main.startSize = 0.15f;

        main.startColor =
        new Color(1f, 0.5f, 0.7f, 0.8f);


        var emission =
        ps.emission;

        emission.rateOverTime = 8;


        var shape =
        ps.shape;

        shape.enabled = true;

        shape.shapeType =
        ParticleSystemShapeType.Sphere;

        shape.radius = 0.2f;


        var velocity =
        ps.velocityOverLifetime;

        velocity.enabled = true;

        velocity.y = 0.3f;


        var renderer =
        ps.GetComponent<ParticleSystemRenderer>();

        renderer.renderMode =
        ParticleSystemRenderMode.Billboard;
    }
}