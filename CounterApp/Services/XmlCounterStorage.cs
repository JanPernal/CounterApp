using System.Diagnostics;
using System.Xml.Linq;
using CounterApp.Models;

namespace CounterApp.Services;

public class XmlCounterStorage : ICounterStorage
{
    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.xml");

    public async Task<List<Counter>> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Counter>();
        }
        await using FileStream stream = File.OpenRead(_filePath);
        XDocument doc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        return doc.Root!.Elements("Counter").Select(e => new Counter
        {
            Id = (Guid?)e.Attribute("Id") ?? Guid.NewGuid(),
            Name = (string?)e.Attribute("Name") ?? "",
            Value = (int?)e.Attribute("Value") ?? 0,
            InitialValue = (int?)e.Attribute("InitialValue") ?? 0,
            ColorHex = (string?)e.Attribute("ColorHex") ?? "#512BD4"

        }).ToList();

    }

    public async Task SaveAsync(IEnumerable<Counter> counters)
    {
        XDocument counterStorage = new XDocument(
            new XElement("Counters",
                counters.Select(c => new XElement("Counter", new XAttribute("Id", c.Id)
                    , new XAttribute("Name", c.Name)
                    , new XAttribute("Value", c.Value)
                    , new XAttribute("InitialValue", c.InitialValue)
                    , new XAttribute("ColorHex", c.ColorHex)))
                )
        );
        await using FileStream stream = File.Create(_filePath);
        await counterStorage.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
    }
}
