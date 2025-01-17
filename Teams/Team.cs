﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PlenBotLogUploader.Teams
{
    public class Team
    {
        /// <summary>
        /// ID of the team, for internal use
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// Name of the webhook team
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// What condition to use to resolve the team
        /// </summary>
        [JsonProperty("condition")]
        public TeamCondition MainCondition { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Name;

        public bool IsSatisfied(DPSReport.DPSReportJSONExtraJSON extraJSON) => MainCondition?.IsSatisfied(extraJSON) ?? false;

        public static IDictionary<int, Team> FromJsonString(string jsonData)
        {
            var parsedData = JsonConvert.DeserializeObject<IEnumerable<Team>>(jsonData)
                             ?? throw new JsonException("Could not parse json to WebhookData");
            
            var result = parsedData.Select(x => (Key: x.ID, TeamData: x))
                .ToDictionary(x => x.Key, x => x.TeamData);

            result.Values.ToList().ForEach(x => x.MainCondition?.SetUp(null));

            return result;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Team) obj);
        }

        protected bool Equals(Team other) => (ID == other.ID) && (Name == other.Name);
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (ID * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}
