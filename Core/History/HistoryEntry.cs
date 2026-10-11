using System;
using System.Collections.Generic;
using System.Text;

namespace Core.History;

public record EncryptionEntry(
    Guid Id,
    DateTime CreatedAt,
    string FileName,
    string Algorithm,
    byte[] OriginalData,
    byte[] EncryptedData);
