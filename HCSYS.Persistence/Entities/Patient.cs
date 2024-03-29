﻿using HCSYS.Persistence.Enums;

namespace HCSYS.Persistence.Entities;

public class Patient : BaseEntity
{
    required public PatientName Name { get; set; }

    public Gender Gender { get; set; }

    public DateTimeOffset BirthDate { get; set; }

    public bool IsActive { get; set; }
}
