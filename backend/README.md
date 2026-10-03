# Anagram Finder — .NET 8 Web API

```bash
cd src/Anagram.Api
dotnet run          # http://localhost:5080, Swagger at /swagger
```

```bash
dotnet test         # from this folder
```

On first run the API creates the `AnagramDictionary` database (SQL Server LocalDB by default; see `appsettings.json`) and loads `Data/brit-a-z.txt`.

| Endpoint | Example |
|---|---|
| `GET /api/anagrams?word=dogs` | `{ "input": "dogs", "anagrams": ["gods"], "count": 1 }` |
| `GET /api/anagrams/occurrences?text=thegodsanddogsweredogged&word=dog` | `{ ..., "count": 3 }` |

See the README in the repository root for the full write-up and design decisions.
