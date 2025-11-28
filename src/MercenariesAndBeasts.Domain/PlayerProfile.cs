using System;
using System.Collections.Generic;
using MercenariesAndBeasts.Domain.Progress;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players
{
    public class PlayerProfile: BaseGuid
    {

        // napojení na Identity AppUser
        public string UserId { get; set; } = string.Empty;
        
        // základní RPG parametry
        public int Level { get; set; } = 1;
        public long Experience { get; set; } = 0;

        // energie (na výpravy / dungeony)
        public int Energy { get; set; } = 100;
        public int MaxEnergy { get; set; } = 100;

        // zlaťáky
        public long Gold { get; set; } = 0;

        // aktuálně vybavené žoldáky / monstra do týmů
        public Guid? ActiveMercenaryTeamId { get; set; }
        public Guid? ActiveMonsterTeamId { get; set; }

        // navigační seznamy progressů
        public List<PlayerExpeditionProgress> ExpeditionProgress { get; set; } = new();
        public List<PlayerDungeonProgress> DungeonProgress { get; set; } = new();

        // čas vytvoření
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}