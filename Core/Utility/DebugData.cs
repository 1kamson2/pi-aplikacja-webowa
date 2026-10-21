using System.Diagnostics;
using Core.Algorithm;
using Core.Histogram;
using Core.History;

namespace Core.Utility;

public static class DebugData
{
    private static void AlgorithmSingletonTestData()
    {
        List<AlgorithmEntry> sampleEntries = new()
        {
            new AlgorithmEntry(
                Hash: "AES",
                Name: "AES-CBC",
                Category: "Symetryczny",
                Description: "Szyfr blokowy AES w trybie CBC z losowym wektorem inicjującym.",
                KeySizeBits: 256,
                IsImplemented: false
            ),
            new AlgorithmEntry(
                Hash: "XOR",
                Name: "XOR",
                Category: "Strumieniowy",
                Description: "Prosty szyfr XOR z kluczem. Tylko do celów demonstracyjnych.",
                KeySizeBits: 128,
                IsImplemented: false
            ),
            new AlgorithmEntry(
                Hash: "ARNOLD",
                Name: "Arnold Cat Map",
                Category: "Chaotyczny",
                Description: "Permutacja pikseli przez odwzorowanie chaotyczne.",
                KeySizeBits: 0,
                IsImplemented: false
            ),
            new AlgorithmEntry(
                Hash: "LOGISTIC",
                Name: "Logistic Map",
                Category: "Chaotyczny",
                Description: "Generator ciągu pseudolosowego oparty na odwzorowaniu logistycznym.",
                KeySizeBits: 64,
                IsImplemented: false
            ),
        };

        foreach (var entry in sampleEntries)
        {
            AlgorithmSingleton.Instance.Add(entry);
        }
    }

    [Conditional("DEBUG")]
    public static void Apply()
    {
        AlgorithmSingletonTestData();
    }
}
