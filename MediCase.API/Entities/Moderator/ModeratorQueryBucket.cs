using System;
using System.Collections.Generic;

namespace MediCase.API.Entities.Moderator;

public partial class ModeratorQueryBucket
{
    public ulong OperationId { get; set; }

    public string OperationType { get; set; } = null!;

    public string DestinationTable { get; set; } = null!;

    public string QueryLog { get; set; } = null!;
}
