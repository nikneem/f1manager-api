﻿using System;

namespace F1Manager.Teams.DataTransferObjects;

public class TeamDriverDetailsDto
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public DateTimeOffset BoughtOn { get; set; }
    public decimal BoughtFor { get; set; }
    public decimal CurrentPrice { get; set; }
    public int PointsGained { get; set; }
    public bool IsFirstDriver { get; set; }
}