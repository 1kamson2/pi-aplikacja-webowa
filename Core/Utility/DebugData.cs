using Core.History;
using System.Diagnostics;

namespace Core.Utility;

public static class DebugData
{
    [Conditional("DEBUG")]
    private static void HistorySingletonTestData()
    {
        byte[] Blue = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAFklEQVR4nGPImn2DJMQwqmFUw/DVAAAn1t0QngnCpgAAAABJRU5ErkJggg=="
        );
        byte[] Teal = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAFklEQVR4nGOI37yQJMQwqmFUw/DVAAAAFrMQdcGX1AAAAABJRU5ErkJggg=="
        );
        byte[] Amber = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAFklEQVR4nGO4uTieJMQwqmFUw/DVAAD/qdsQ/ZXq4AAAAABJRU5ErkJggg=="
        );
        byte[] Grey = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAFklEQVR4nGPIK6giCTGMahjVMHw1AADV4lgQ1C1FagAAAABJRU5ErkJggg=="
        );
        List<EncryptionEntry> sampleEntries = new()
        {
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddMinutes(-5),
                FileName: "AAAA.png",
                Algorithm: "AES",
                OriginalData: Blue,
                EncryptedData: Grey
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddMinutes(-47),
                FileName: "BBBB.png",
                Algorithm: "XOR",
                OriginalData: Teal,
                EncryptedData: Grey
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddHours(-3),
                FileName: "CCCC.png",
                Algorithm: "AES",
                OriginalData: Amber,
                EncryptedData: Grey
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddDays(-1),
                FileName: "DDDD.png",
                Algorithm: "XOR",
                OriginalData: Grey,
                EncryptedData: Blue
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddDays(-1),
                FileName: "EEEE.png",
                Algorithm: "XOR",
                OriginalData: Grey,
                EncryptedData: Blue
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddDays(-1),
                FileName: "FFFF.png",
                Algorithm: "XOR",
                OriginalData: Grey,
                EncryptedData: Blue
            ),
            new EncryptionEntry(
                Id: Guid.NewGuid(),
                CreatedAt: DateTime.Now.AddDays(-1),
                FileName: "GGGG.png",
                Algorithm: "XOR",
                OriginalData: Grey,
                EncryptedData: Blue
            ),
        };

        foreach (var entry in sampleEntries)
        {
            HistorySingleton.Add(entry);
        }
    }

    [Conditional("DEBUG")]
    public static void Apply()
    {
        HistorySingletonTestData();
    }
}
