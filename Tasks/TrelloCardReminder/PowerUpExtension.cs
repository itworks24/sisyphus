using Manatee.Json.Serialization;
using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manatee.Json;

namespace Sisyphus
{ 
    public class PowerUpSetting 
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public static class PowerUpExtensions
    {
        /// <summary>
        /// Gets meta-data about the custom fields.
        /// </summary>
        /// <param name="board">The board that defines the fields.</param>
        /// <returns>The custom field settings.</returns>
        public static IEnumerable<PowerUpSetting> GetPowerUpSettings(this Board board, string PowerUpId)
        {
            var data = board.PowerUpData.FirstOrDefault(d => d.PluginId == PowerUpId);
            if (data == null) return null;

            var json = JsonValue.Parse(data.Value);
            var settings = json.Object.Select(d => new PowerUpSetting
                                                {
                                                    Id = d.Key,
                                                    Value = d.Value.Type == JsonValueType.String
                                                                                                                              ? d.Value.String
                                                                                                                              : d.Value.ToString()
                                                });
            return settings.ToList();
        }      
    }
}
