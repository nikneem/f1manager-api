using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Leagues.Entities
{
    public class LeagueMemberEntity : TableEntity
    {
        public DateTimeOffset JoinedOn { get; set; }
    }
}