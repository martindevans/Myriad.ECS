using Myriad.ECS.Systems;

namespace NBodyIntegrator.Integrator.NBodies;

public class RailIntegrator
    : ISystem
{
    /// <summary>
    /// Number of iterations to integrate in a single step.
    /// </summary>
    public const int OrbitIters = 64;


    public void Update(GameTime time)
    {
        throw new NotImplementedException();
    }
}