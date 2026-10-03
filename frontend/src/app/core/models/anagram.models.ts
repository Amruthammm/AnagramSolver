/** Mirrors Anagram.Application.Models.MatchingAnagramsResult */
export interface MatchingAnagramsResult {
  input: string;
  anagrams: string[];
  count: number;
}

/** Mirrors Anagram.Application.Models.AnagramOccurrence */
export interface AnagramOccurrence {
  index: number;
  value: string;
}

/** Mirrors Anagram.Application.Models.AnagramOccurrencesResult */
export interface AnagramOccurrencesResult {
  text: string;
  word: string;
  occurrences: AnagramOccurrence[];
  count: number;
}
