using System;
using System.Linq;

namespace asp.net
{
    public class ParticleProgram
    {
        public static double[] Run(Func<double[], double> errorFunction, int dimensions = 2, int particleCount = 5, int maxEpochs = 1000, double minX = -10.0, double maxX = 10.0, double minAcceptedError = 0.0)
        {
            System.Diagnostics.Debug.WriteLine(@"Begin Particle Swarm Optimization");
            System.Diagnostics.Debug.WriteLine(@"Setting problem dimension to " + dimensions);
            System.Diagnostics.Debug.WriteLine(@"Setting particleCount = " + particleCount);
            System.Diagnostics.Debug.WriteLine(@"Setting maxEpochs = " + maxEpochs);
            System.Diagnostics.Debug.WriteLine(@"Setting early exit error = " + minAcceptedError.ToString("F10"));
            System.Diagnostics.Debug.WriteLine(@"Setting minX, maxX = " + minX.ToString("F2") + @" " + maxX.ToString("F2"));
            System.Diagnostics.Debug.WriteLine(@"Starting PSO");

            double[] bestPosition = Solve(dimensions, particleCount, minX, maxX, maxEpochs, minAcceptedError, errorFunction, 
                out Particle[] finalSwarm, out double finalEpoch, out double minError, out double[] errors);

            // Show final swarm results
            System.Diagnostics.Debug.WriteLine(@"Processing complete");
            System.Diagnostics.Debug.WriteLine(@"Final swarm:");
            //foreach (Particle t in finalSwarm)
                //System.Diagnostics.Debug.WriteLine(t.ToString());
            System.Diagnostics.Debug.WriteLine($@"Final epoch: {finalEpoch}");
            System.Diagnostics.Debug.WriteLine(@"Best position/solution found:");
            //for (int i = 0; i < bestPosition.Length; ++i)
            //{
            //    System.Diagnostics.Debug.Write(@"x" + i + @" = ");
            //    System.Diagnostics.Debug.WriteLine(bestPosition[i].ToString("F6") + @" ");
            //}
            for (int i = 0; i < errors.Length; ++i)
            {
                System.Diagnostics.Debug.Write(@"error[" + i + @"] = ");
                System.Diagnostics.Debug.WriteLine(errors[i].ToString("F14") + @" ");
            }
            System.Diagnostics.Debug.Write(@"Final best error = ");
            System.Diagnostics.Debug.WriteLine(minError.ToString("F10"));
            System.Diagnostics.Debug.WriteLine(@"End PSO demo");
            return errors;
            //System.Diagnostics.Debug.ReadLine();
        }

 
        static double[] Solve(int dimensions, int particleCount, double minX, double maxX, int maxEpochs, double minAcceptedError, 
            Func<double[], double> errorFunction, out Particle[] swarm, out double epoch, out double minError, out double[] errors)
        {
            Random random = new Random(0);
            double magicMultiplier = 0.1; // TODO: why 0.1?

            swarm = new Particle[particleCount];
            double[] bestGlobalPosition = new double[dimensions];
            double minGlobalError = double.MaxValue;
            
            for (int i = 0; i < swarm.Length; ++i) // Swarm initialization
            {
                double[] randomPosition = new double[dimensions];
                for (int j = 0; j < randomPosition.Length; ++j)
                    randomPosition[j] = (maxX - minX) * random.NextDouble() + minX;

                double error = errorFunction(randomPosition);
                double[] randomVelocity = new double[dimensions];

                for (int j = 0; j < randomVelocity.Length; ++j)
                {
                    double lo = minX * magicMultiplier;
                    double hi = maxX * magicMultiplier;
                    randomVelocity[j] = (hi - lo) * random.NextDouble() + lo;
                }
                swarm[i] = new Particle(randomPosition, error, randomVelocity, randomPosition, error);
                
                if (swarm[i].Error < minGlobalError) // Check if has global best position/solution
                {
                    minGlobalError = swarm[i].Error;
                    swarm[i].Position.CopyTo(bestGlobalPosition, 0);
                }
            }

            // Prepare
            double w = 0.729; // Inertia weight. see http://ieeexplore.ieee.org/stamp/stamp.jsp?arnumber=00870279
            double c1 = 1.49445; // Cognitive/local weight
            double c2 = 1.49445; // Social/global weight
            double deathProbability = 0.01;
            epoch = 0;
            double[] newVelocity = new double[dimensions];
            double[] newPosition = new double[dimensions];
            double[] currErrors = new double[maxEpochs];
            
            while (epoch < maxEpochs && minGlobalError > minAcceptedError) // Main execution
            {
                foreach (Particle currentParticle in swarm)
                {
                    for (int j = 0; j < currentParticle.Velocity.Length; ++j) // New velocity
                        newVelocity[j] = (w * currentParticle.Velocity[j]) +
                                         (c1 * random.NextDouble() * (currentParticle.BestPosition[j] - currentParticle.Position[j])) + // Cognitive/Local weight randomization
                                         (c2 * random.NextDouble() * (bestGlobalPosition[j] - currentParticle.Position[j])); // Social/Global weight randomization

                    newVelocity.CopyTo(currentParticle.Velocity, 0);
                    
                    for (int j = 0; j < currentParticle.Position.Length; ++j) // Calculate new Position
                    {
                        newPosition[j] = currentParticle.Position[j] + newVelocity[j];
                        if (newPosition[j] < minX)
                            newPosition[j] = minX;
                        else if (newPosition[j] > maxX)
                            newPosition[j] = maxX;
                    }
                    newPosition.CopyTo(currentParticle.Position, 0);

                    double newError = errorFunction(newPosition);
                    //System.Diagnostics.Debug.WriteLine(@"error " + newError.ToString("F6") + @" ");
                    currentParticle.Error = newError;

                    if (newError < currentParticle.BestError)
                    {
                        newPosition.CopyTo(currentParticle.BestPosition, 0);
                        currentParticle.BestError = newError;
                    }
                    if (newError < minGlobalError)
                    {
                        newPosition.CopyTo(bestGlobalPosition, 0);
                        minGlobalError = newError;
                    }
                    
                    if (random.NextDouble() < deathProbability) // If particle dies: new position, leave velocity, update error
                    {
                        for (int j = 0; j < currentParticle.Position.Length; ++j)
                            currentParticle.Position[j] = (maxX - minX) * random.NextDouble() + minX;
                        currentParticle.Error = errorFunction(currentParticle.Position);
                        currentParticle.Position.CopyTo(currentParticle.BestPosition, 0);
                        currentParticle.BestError = currentParticle.Error;

                        if (currentParticle.Error < minGlobalError) // global best by chance?
                        {
                            minGlobalError = currentParticle.Error;
                            currentParticle.Position.CopyTo(bestGlobalPosition, 0);
                        }
                    }
                }
                currErrors[Convert.ToInt32(epoch)] = minGlobalError;
                ++epoch;
            }
            
            double[] result = new double[dimensions];
            bestGlobalPosition.CopyTo(result, 0);
            minError = minGlobalError;
            errors = new double[Convert.ToInt32(epoch)];
            Array.Copy(currErrors, errors, Convert.ToInt32(epoch));

            return result;
        }
    }
}
