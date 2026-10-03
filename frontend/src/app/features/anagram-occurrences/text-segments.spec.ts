import { toSegments } from './text-segments';

describe('toSegments', () => {
  it('marks each matched window in the text', () => {
    const segments = toSegments(
      'thegodsanddogs',
      [
        { index: 3, value: 'god' },
        { index: 10, value: 'dog' },
      ],
      3,
    );

    expect(segments).toEqual([
      { text: 'the', match: false },
      { text: 'god', match: true },
      { text: 'sand', match: false },
      { text: 'dog', match: true },
      { text: 's', match: false },
    ]);
  });
});
