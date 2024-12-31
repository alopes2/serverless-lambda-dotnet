using System.Collections.ObjectModel;

namespace Lambda;

public record Data(string ID, string Title, decimal Rating, Collection<string> Genres);