import { AnagramOccurrence } from '../../core/models/anagram.models';

export interface TextSegment {
  text: string;
  match: boolean;
}

/**
 * Splits the searched text into plain / matched segments so the template can
 * highlight where each anagram was found, e.g. "the[god]sand[dog]s...".
 * Overlapping matches simply merge into one highlighted run.
 */
export function toSegments(text: string, occurrences: AnagramOccurrence[], wordLength: number): TextSegment[] {
  const matched = new Array<boolean>(text.length).fill(false);
  for (const { index } of occurrences) {
    matched.fill(true, index, index + wordLength);
  }

  const segments: TextSegment[] = [];
  for (let i = 0; i < text.length; i++) {
    const last = segments.at(-1); /** give me last item in an array */
    if (last && last.match === matched[i]) {
      last.text += text[i];
    } else {
      segments.push({ text: text[i], match: matched[i] });
    }
  }

  return segments;
}
