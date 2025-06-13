1. Aplicacions/tecnologies que fan servir programació distribuïda
La programació distribuïda implica que diversos processos s’executen en diferents màquines que cooperen per aconseguir un objectiu comú. Aquí tens 5 exemples:

  1. Google Search
  Què fa: Processa milions de cerques per segon.
  
  Per què distribuïda: Utilitza milers de servidors per indexar pàgines web i respondre consultes eficientment.
  
  2. Netflix
  Què fa: Reprodueix vídeos en streaming a milions d’usuaris.
  
  Per què distribuïda: Distribueix els seus serveis i continguts per servidors arreu del món (CDN) per garantir baixa latència i alta disponibilitat.
  
  3. Bitcoin i Blockchain
  Què fa: Registra transaccions financeres de forma descentralitzada.
  
  Per què distribuïda: Cada node de la xarxa manté una còpia del llibre de comptes, garantint seguretat i transparència sense un servidor central.
  
  4. Amazon Web Services (AWS)
  Què fa: Proporciona serveis com emmagatzematge, computació, IA, etc.
  
  Per què distribuïda: Ofereix alta disponibilitat i escalabilitat a través de servidors repartits globalment.
  
  5. Dropbox
  Què fa: Sincronitza i emmagatzema fitxers al núvol.
  
  Per què distribuïda: Els fitxers es repliquen entre servidors per garantir disponibilitat i tolerància a fallades.

2. Concurrència en CPU’s multicore
Actualment, les CPU amb múltiples nuclis apliquen la concurrència de diferents formes:

  A. Multithreading (fil multitasca)
  S’executen múltiples fils en paral·lel.
  
  Avantatge: Millor aprofitament de recursos, sobretot per aplicacions I/O-bound i UI responsiva.
  
  B. Multiprocessament
  S’executen múltiples processos en paral·lel.
  
  Avantatge: Major aïllament, útil per a tasques pesades i computacionalment intensives.
  
  C. Vectorització / SIMD (Single Instruction Multiple Data)
  Operacions sobre múltiples dades en paral·lel (molt usada en gràfics).
  
  Avantatge: Augmenta molt el rendiment en càlculs matemàtics repetitius.
  
  D. Hiperhilo (Hyper-threading)
  Simula múltiples fils per nucli físic.
  
  Avantatge: Millor rendiment en aplicacions multitarea amb poc ús de CPU.

3. Programació paral·lela vs asíncrona
Característica	        Programació Paral·lela	                      Programació Asíncrona
Objectiu	              Executar diverses tasques simultàniament	    Evitar bloqueigs mentre s’espera I/O
Tipus de tasques	      CPU-bound (requereixen càlcul)	              I/O-bound (espera xarxa, fitxers, etc.)
Exemples	              Processar imatges, simulacions	              Descàrrega web, lectura fitxers
Recursos	              Multiprocés o multithread	                    Task, async/await

És el cicle de vida d’un mètode asíncron en C#. El mètode és GetUrlContentLengthAsync.

Crida inicial (fletxa vermella): El mètode GetUrlContentLengthAsync() és cridat.

S’inicia la crida asíncrona: GetStringAsync() demana una URL, retorna una Task pendent.

Continuació: Es continua executant el mètode, però el control es cedeix al codi que va fer la crida (sense bloquejar).

Tasques independents: DoIndependentWork() es crida mentre s’espera la resposta.

Sortida provisional: La funció retorna una Task pendent al mètode que la va cridar.

Resum del procés suspès (fletxa blava): Quan getStringTask s’ha completat, el mètode es reprèn on es va suspendre.

Continuació post-await: Ara getStringTask.Result està disponible.

Fi del mètode: Es calcula la longitud i es retorna.

Quin tipus de programació usar
Aplicació	                                      Tipus de programació	    Raó
Processament de lots d’imatges	                Paral·lela	              Són tasques independents i CPU-bound. Cadascuna es pot paralel·litzar.
Aplicació d’escriptori amb UI fluida	          Asíncrona	                Cal evitar bloqueigs de la interfície quan es fa I/O o espera xarxa.
Aplicació de missatgeria en temps real	        Asíncrona	                Interacció constant amb xarxa, no convé bloquejar l’execució.
Renderització de gràfics 3D (blocs petits)	    Paral·lela	              Els blocs es poden calcular simultàniament, procés intensiu de CPU.

7. Analitza el següent codi, explica que pretén fer i determina si té errors o punts de millora per un ús correcte dels Threads. 
Simula una cursa de sensors que generen valors entre -20 i 50. Cada sensor genera 100.000 valors en un fil (thread). Durant el procés, el programa actualitza:

Un màxim global (GlobalMax)

Un mínim global (GlobalMin)

Una lectura final per cada sensor a Readings[i]

També mesura el temps total d’execució amb un cronòmetre (StopWatch).

Errors i punts de millora
1. Stopwacth està mal escrit (hauria de ser Stopwatch)
També cal afegir using System.Diagnostics;

2. sW.StarNew() no existeix
L’ús correcte és sW.Start() (i es crida només una vegada, abans de començar la feina, no dins el bucle)

3. Condicions de cursa (race conditions) amb GlobalMax i GlobalMin
Diversos fils poden llegir i escriure simultàniament a aquestes variables → comportament indeterminat.

4. Accés compartit a Random rng
L’objecte Random no és thread-safe. S’ha de crear un per fil o usar ThreadLocal<Random>.

5. No s’espera a que tots els fils acabin
Falta threads[i].Join() després del bucle per esperar que tots els fils acabin abans d’imprimir els resultats.

6. sW.Restart() no retorna el temps, només reinicia el cronòmetre
Per mostrar el temps total, cal usar sW.Elapsed.

Versió corregida del codi
csharp
Copiar
Editar
using System;
using System.Diagnostics;
using System.Threading;

namespace SensorRace
{
    class Program
    {
        public static int[] Readings;
        public static int GlobalMax = int.MinValue;
        public static int GlobalMin = int.MaxValue;
        private static readonly object lockObj = new object();

        static void Main(string[] args)
        {
            Console.Write("Introdueix el nombre de sensors: ");
            int sensors = int.Parse(Console.ReadLine());

            Readings = new int[sensors];
            Thread[] threads = new Thread[sensors];

            Stopwatch sW = new Stopwatch();
            sW.Start();

            for (int i = 0; i < sensors; i++)
            {
                int id = i;

                threads[i] = new Thread(() =>
                {
                    Random localRng = new Random(Guid.NewGuid().GetHashCode());

                    for (int j = 0; j < 100000; j++)
                    {
                        int value = localRng.Next(-20, 51);
                        Readings[id] = value;

                        lock (lockObj)
                        {
                            if (value > GlobalMax)
                                GlobalMax = value;

                            if (value < GlobalMin)
                                GlobalMin = value;
                        }
                    }
                });

                threads[i].Start();
            }

            // Esperar que tots els fils acabin
            for (int i = 0; i < sensors; i++)
            {
                threads[i].Join();
            }

            sW.Stop();

            Console.WriteLine($"Final – Max: {GlobalMax}, Min: {GlobalMin}");
            Console.WriteLine($"Total Process time: {sW.Elapsed.TotalSeconds:F2} segons");
        }
    }
}
