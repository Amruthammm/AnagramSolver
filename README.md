# Anagram Finder — .NET / Angular coding exercise

A .NET 8 Web API and an Angular 19 client that:

1. **List matching anagrams**: every dictionary word that uses all of the letters of the input (`dogs` → `gods`).
2. **Find the anagrams**: counts how many times a word, or any anagram of it, appears in a text (`thegodsanddogsweredogged` + `dog` → **3**: *god*, *dog*, *dog*).

The dictionary is `brit-a-z.txt`, loaded into a SQL Server table on first run.

The repository holds **two separate applications**:

| Folder | Application | Run with |
|---|---|---|
| `backend/` | .NET 8 Web API (solution `AnagramSolver.sln`, projects in `src/`, tests in `tests/`) | `dotnet run` |
| `frontend/` | Angular 19 single-page app | `npm start` |

They only talk to each other over HTTP (`/api/...`).

---

## Running it

### Prerequisites
- .NET 8 SDK
- Node 18.19+ / 20+ and npm
- SQL Server LocalDB (installed with Visual Studio). There is an alternative without SQL Server [below](#running-without-sql-server).

### 1. API

```bash
cd backend/src/Anagram.Api
dotnet run
```

On first start-up the API creates the `AnagramDictionary` database and loads the ~80,000 words (this takes a few seconds; later starts skip it).
The API listens on **http://localhost:5080**. Swagger is at http://localhost:5080/swagger.

### 2. Angular client

```bash
cd frontend
npm install
npm start
```

Open **http://localhost:4200** (it opens on `/matching-anagrams`; `/find-anagrams` is the second page). The dev server proxies `/api` to the API (`proxy.conf.json`), so no URLs are hard-coded.

### 3. Tests

```bash
cd backend && dotnet test       # xUnit tests
cd ../frontend && npm test      # Jasmine / Karma tests
```

### Running without SQL Server

The data layer also supports SQLite. Override the settings with environment variables (run from `backend/src/Anagram.Api`):

```bash
# macOS / Linux
Database__Provider=Sqlite Database__ConnectionString="Data Source=anagrams.db" dotnet run

# Windows PowerShell
$env:Database__Provider="Sqlite"; $env:Database__ConnectionString="Data Source=anagrams.db"; dotnet run
```

To use a full SQL Server instance (or SQL Server in Docker), change `Database:ConnectionString` in `appsettings.json`.

---

## API

| Method | Route | Example | Response |
|---|---|---|---|
| GET | `/api/anagrams?word={word}` | `?word=dogs` | `{ "input": "dogs", "anagrams": ["gods"], "count": 1 }` |
| GET | `/api/anagrams/occurrences?text={text}&word={word}` | `?text=thegodsanddogsweredogged&word=dog` | `{ "text": "...", "word": "dog", "occurrences": [{ "index": 3, "value": "god" }, { "index": 10, "value": "dog" }, { "index": 18, "value": "dog" }], "count": 3 }` |

Invalid input (missing, too long, or a "word" containing digits or symbols) returns `400` with standard `ProblemDetails` validation errors.

---

## Solution structure

```
backend/
├── AnagramSolver.sln
├── src
│   ├── Anagram.Domain           Word entity, AnagramSignature, WordText normalisation (no dependencies)
│   ├── Anagram.Application      Use cases: MatchingAnagramsService, AnagramOccurrenceService,
│   │                            interfaces (IWordRepository, ...), result models (no dependencies)
│   ├── Anagram.Infrastructure   EF Core DbContext, table configuration, WordRepository,
│   │                            dictionary seeding (FileWordSource + DictionarySeeder)
│   └── Anagram.Api              Controller, request contracts + validation, composition root
├── tests
│   └── Anagram.Application.Tests  xUnit tests for the domain and both use cases

frontend/                        Angular 19 standalone app
└── src/app
    ├── app.routes.ts         The two pages + redirects (lazy-loaded)
    ├── core/models           API response types
    ├── core/services         AnagramApiService (HttpClient wrapper), API_BASE_URL token
    ├── core/state            ViewState<T> + toViewState() RxJS operator
    ├── features/matching-anagrams
    ├── features/anagram-occurrences
    └── shared                Form validators (kept in step with the API's rules)
```

Dependencies point inwards: `Api → Application ← Infrastructure`, and everything → `Domain`. The Application layer only knows about `IWordRepository`, so it can be tested without a database and the storage could be replaced without touching business logic.

---

## Design decisions

### The anagram signature (the key idea)
Two words are anagrams exactly when their **sorted letters** are equal: `dogs` → `dgos`, `gods` → `dgos`.

- When the dictionary is loaded, every word is stored with its signature in a `Signature` column.
- `Signature` has an index that also **includes** `Text`, so finding anagrams is one index seek: `SELECT Text FROM Words WHERE Signature = 'dgos'`. There is no need to compare against all 80k words.
- Words of a different length always have a different signature, so the rule *"must use all of the letters"* holds automatically: `dogs` never matches `dog` or `god`.
- The input word itself is excluded from its own results.

### Table design
`Words(Id int PK, Text nvarchar(64) UNIQUE, Signature nvarchar(64) INDEXED INCLUDE(Text))`.
The signature is computed in `Word.Create(...)`, so it can never get out of step with the text.

### Counting occurrences (feature 2)
A sliding window the length of the word moves left to right across the text; a window counts when its signature equals the word's signature. Windows overlap (`abab` + `ab` → 3). This feature needs no database, so it is a pure, stateless service.
Complexity is O(n · k log k), which is fine for form input. For very long texts, a rolling letter-frequency count would make it O(n). The service sits behind an interface, so the algorithm could be swapped without changing anything else.

### Dictionary loading
- `brit-a-z.txt` is **Latin-1** encoded with CRLF line endings, so it is read with `Encoding.Latin1`. That keeps words like *cliché* and *déjà* intact instead of turning them into `?`.
- Seeding runs once (only when the table is empty), in batches of 5,000 inside one transaction, so the table is never half-loaded.
- Words with apostrophes (e.g. *dog's*) are kept as they appear in the file, apostrophe included, so `dogs` does not return `god's`.

### Case and whitespace
`WordText.Normalise` (trim + lower-case) is the single place that defines normalisation. Both the dictionary loader and user input go through it.

### SOLID in practice
- **Single responsibility:** reading the file (`FileWordSource`), seeding (`DictionarySeeder`), querying (`WordRepository`) and each use case are separate classes.
- **Open/closed and dependency inversion:** the use cases depend on interfaces; the database provider is chosen in configuration.
- **Interface segregation:** each feature has its own small interface rather than one "AnagramService".

### Angular
- Standalone components, reactive forms, `OnPush` change detection.
- **Routing:** each feature has its own page: `/matching-anagrams` and `/find-anagrams`, with a tab bar to switch. Pages are lazy-loaded (`loadComponent`), so each page's code downloads only when it is first visited. `/` and unknown URLs redirect to the first page, and each page sets its own browser tab title.
- **RxJS:** each form submit is pushed into a `Subject`, and `switchMap` turns it into an API call (cancelling any in-flight request). The custom `toViewState()` operator maps the response to `loading | success | error`. The template binds with the `async` pipe, so there are no manual subscriptions or unsubscribes.
- Client-side validation mirrors the server rules; the server still validates independently.
- The layout is a responsive CSS grid: two columns on desktop, one on mobile.

### What I'd add with more time
- EF Core migrations instead of `EnsureCreated()`.
- An integration test using `WebApplicationFactory<Program>` with SQLite in-memory.
- Response caching for repeated anagram lookups, since the dictionary is read-only.
