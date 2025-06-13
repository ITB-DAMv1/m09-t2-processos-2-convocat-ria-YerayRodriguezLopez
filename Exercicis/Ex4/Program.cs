using System;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Aquesta classe conté el mètode principal que recull informació sobre els processos i fils actius.
/// </summary>
class Program
{
    /// <summary>
    /// Punt d'entrada del programa.
    /// </summary>
    static void Main()
    {
        string filePath = "../../../processos_i_fils.txt";

        int totalProcessos = 0;
        int totalFils = 0;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            Process[] processos = Process.GetProcesses();
            totalProcessos = processos.Length;

            foreach (Process p in processos)
            {
                try
                {
                    writer.WriteLine($"Procés: {p.ProcessName} (ID: {p.Id})");

                    ProcessThreadCollection fils = p.Threads;
                    foreach (ProcessThread fil in fils)
                    {
                        writer.WriteLine($"\tFil ID: {fil.Id}");
                        totalFils++;
                    }

                    writer.WriteLine(); // línia en blanc entre processos
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"\tError accedint al procés: {ex.Message}");
                }
            }

            // Escriure el resum final
            writer.WriteLine("----- RESUM FINAL -----");
            writer.WriteLine($"Total de processos: {totalProcessos}");
            writer.WriteLine($"Total de fils: {totalFils}");
        }

        Console.WriteLine($"Informació guardada a: {Path.GetFullPath(filePath)}");
    }
}
