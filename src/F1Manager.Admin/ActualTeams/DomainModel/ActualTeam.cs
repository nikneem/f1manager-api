using System;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Admin.ActualTeams.DomainModel;

public class ActualTeam : DomainModel<Guid>

{
    public string Name { get; private set; }
    public string Base { get; private set; }
    public string Principal { get; private set; }
    public string TechnicalChief { get; private set; }
    public Guid FirstDriverId { get; private set; }
    public Guid SecondDriverId { get; private set; }
    public Guid ChassisId { get; private set; }
    public Guid EngineId { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsDeleted { get; private set; }

    public void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ActualTeamNameInvalid,
                $"The name {value} is invalid");
        }

        if (!Equals(Name, value))
        {
            Name = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetBase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ActualTeamBaseInvalid,
                $"The base name {value} is invalid");
        }

        if (!Equals(Base, value))
        {
            Base = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetPrincipal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ActualTeamPrincipalInvalid,
                $"The principal {value} is invalid");
        }

        if (!Equals(Principal, value))
        {
            Principal = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetTechnicalChief(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ActualTeamTechChiefInvalid,
                $"The technical chief {value} is invalid");
        }

        if (!Equals(TechnicalChief, value))
        {
            TechnicalChief = value;
            SetState(TrackingState.Modified);
        }
    }

    public void SetFirstDriver(Guid value)
    {
        if (!Equals(FirstDriverId, value))
        {
            FirstDriverId = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetSecondDriver(Guid value)
    {
        if (!Equals(SecondDriverId, value))
        {
            SecondDriverId = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetEngine(Guid value)
    {
        if (!Equals(EngineId, value))
        {
            EngineId = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetChassis(Guid value)
    {
        if (!Equals(ChassisId, value))
        {
            ChassisId = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetAvailable(bool value)
    {
        if (!Equals(IsAvailable, value))
        {
            IsAvailable = value;
            SetState(TrackingState.Modified);
        }
    }
    public void SetDeleted(bool value)
    {
        if (!Equals(IsDeleted, value))
        {
            IsDeleted = value;
            SetState(TrackingState.Modified);
        }
    }

    public void Delete()
    {
        SetDeleted(true);
    }
    public void Undelete()
    {
        SetDeleted(false);
    }

    private ActualTeam() : base(Guid.NewGuid(), TrackingState.New)
    {
    }
    public ActualTeam(Guid id,
        string name,
        string baseD,
        string principal,
        string techChief,
        Guid first,
        Guid second,
        Guid engine,
        Guid chassis,
        bool available,
        bool deleted) : base(id)
    {
        Name = name;
        Base = baseD;
        Principal = principal;
        TechnicalChief = techChief;
        FirstDriverId = first;
        SecondDriverId = second;
        EngineId = engine;
        ChassisId = chassis;
        IsAvailable = available;
        IsDeleted = deleted;
    }

    public static ActualTeam Create(string name, string teamBase, string principal, string techChief, Guid first,
        Guid second, Guid engine, Guid chassis)
    {
        var team = new ActualTeam();
        team.SetName(name);
        team.SetBase(teamBase);
        team.SetPrincipal(principal);
        team.SetTechnicalChief(techChief);
        team.SetFirstDriver(first);
        team.SetSecondDriver(second);
        team.SetEngine(engine);
        team.SetChassis(chassis);
        team.SetAvailable(true);
        return team;
    }
    public static ActualTeam Create(string name, Guid first, Guid second, Guid engine, Guid chassis)
    {
        return Create(name, string.Empty, string.Empty, string.Empty, first, second, engine, chassis);
    }


}