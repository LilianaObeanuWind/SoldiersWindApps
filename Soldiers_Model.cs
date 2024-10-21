using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Soldiers_Model
{
    // Interface for soldier data
    public interface ISoldier
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Rank { get; }
        string Country { get; }
        string TrainingInfo { get; }
        Color Color1 { get; }
    }

    // Interface for position data
    public interface IPosition
    {
        int SoldierId { get; }
        double Latitude { get; }
        double Longitude { get; }
    }

    // Interface for position updates
    public interface IPositionUpdate
    {
        DateTime Timestamp { get; }
        List<Position> Positions { get; }
    }

    // Concrete classes implementing the interfaces
    public class Soldier : ISoldier
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("Rank")]
        public string Rank { get; set; }
        [JsonProperty("Country")]
        public string Country { get; set; }
        [JsonProperty("TrainingInfo")]
        public string TrainingInfo { get; set; }
        [JsonProperty("Color")]
        public Color Color1 { get; set; }
    }

    public class Position : IPosition
    {
        [JsonProperty("SoldierId")]
        public int SoldierId { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }

    public class PositionUpdate : IPositionUpdate
    {
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("Positions")]
        public List<Position> Positions { get; set; } = new List<Position>();
    }

    public class RootObject
    {
        [JsonProperty("Soldiers")]
        public List<Soldier> Soldiers { get; set; }
        [JsonProperty("PositionUpdates")]
        public List<PositionUpdate> PositionUpdates { get; set; } = new List<PositionUpdate>();
    }
}