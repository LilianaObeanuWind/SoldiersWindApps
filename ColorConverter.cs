using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldiersWindApps
{
    public class ColorConverter : JsonConverter<Color>
    {
        private readonly Dictionary<string, Color> colorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
    {
        { "Red", Color.Red },
        { "Lime", Color.Lime },
        { "Blue", Color.Blue },
        { "Yellow", Color.Yellow },
        { "Magenta", Color.Magenta }
    };

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            string colorName = colorMap.FirstOrDefault(x => x.Value == value).Key;
            writer.WriteValue(colorName ?? value.Name); // Fallback to color name if not found
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string colorName = reader.Value?.ToString();
            if (colorMap.TryGetValue(colorName, out Color color))
            {
                return color;
            }
            else
            {
                try
                {
                    return ColorTranslator.FromHtml(colorName); // Try converting from hex if not found in colorMap
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException($"Invalid color name or hex code: {colorName}", ex);
                }
            }
        }
    }
}
