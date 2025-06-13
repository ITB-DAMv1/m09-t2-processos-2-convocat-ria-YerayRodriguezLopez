using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class CakeProductionSimulator
{
    public void RunSimulations()
    {
        Console.WriteLine("Simulació SEQÜENCIAL:");
        var sequentialTime = RunSequentialSimulation();
        Console.WriteLine($"\nTemps total seqüencial: {sequentialTime.TotalSeconds} segons\n");

        Console.WriteLine("Simulació PARAL·LELA:");
        var parallelTime = RunParallelSimulation();
        Console.WriteLine($"\nTemps total paral·lel: {parallelTime.TotalSeconds} segons");
    }

    /// <summary>
    /// Simulació pas a pas de forma seqüencial.
    /// </summary>
    private TimeSpan RunSequentialSimulation()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        BeatMixture();
        PreheatOven();
        Bake();
        PrepareTopping();
        CoolDownBase();
        Glaze();
        Decorate();

        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    /// <summary>
    /// Simulació amb tasques en paral·lel utilitzant Task.
    /// </summary>
    private TimeSpan RunParallelSimulation()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        Task beatMixtureTask = Task.Run(() => BeatMixture());
        Task preheatOvenTask = Task.Run(() => PreheatOven());

        Task.WhenAll(beatMixtureTask, preheatOvenTask).Wait();
        Task bakeTask = Task.Run(() => Bake());

        Task toppingTask = Task.Run(() => PrepareTopping());

        bakeTask.Wait();
        Task coolDownTask = Task.Run(() => CoolDownBase());

        Task.WhenAll(toppingTask, coolDownTask).Wait();
        Task glazeTask = Task.Run(() => Glaze());

        glazeTask.Wait();
        Decorate();

        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    // —————————————— Tasques individuals amb temps simulats ——————————————

    private void BeatMixture()
    {
        SimulateTask("1. Batre la massa", 8);
    }

    private void PreheatOven()
    {
        SimulateTask("2. Preescalfar forn", 10);
    }

    private void Bake()
    {
        SimulateTask("3. Enfornar", 15);
    }

    private void PrepareTopping()
    {
        SimulateTask("4. Preparar cobertura", 5);
    }

    private void CoolDownBase()
    {
        SimulateTask("5. Refredar base", 4);
    }

    private void Glaze()
    {
        SimulateTask("6. Glassejar", 3);
    }

    private void Decorate()
    {
        SimulateTask("7. Decorar", 2);
    }

    /// <summary>
    /// Simula una tasca amb un retard de n segons.
    /// </summary>
    private void SimulateTask(string taskName, int seconds)
    {
        Console.WriteLine($"{taskName}... ({seconds}s)");
        Task.Delay(seconds * 1000).Wait();
    }
}
