# BibTeXLibrary
A utility library for BibTeX written in C#.

## Example

- string -> BibEntry
```csharp
var parser = new BibParser(
                new StringReader(
                  "@article{keyword, title = {\"0\"{123}456{789}}, year = 2012, address=\"PingLeYuan\"}"));
var entry = parser.GetAllResult()[0];
```

- Using BibEntry
```csharp
// Get Property
entry.Type;       // string: Article
entry.Title;      // string: 0{123}456{789}
entry["Title"];   // string: 0{123}456{789}
```

- BibEntry -> string
```csharp
entry.ToString();
// @Article{keyword,
//   title = {0{123}456{789}},
//   year = {2012},
//   address = {PingLeYuan},
// }
```

- Local File -> BibEntries
```csharp
var parser = new BibParser(new StreamReader("text.bib", Encoding.Default));
var entries = parser.GetAllResult();

foreach(var entry in entries) { ... }
```
