import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Subject, map, switchMap } from 'rxjs';

import { AnagramOccurrencesResult } from '../../core/models/anagram.models';
import { AnagramApiService } from '../../core/services/anagram-api.service';
import { toViewState } from '../../core/state/view-state';
import { textValidators, wordValidators } from '../../shared/word-validators';
import { TextSegment, toSegments } from './text-segments';

interface OccurrencesViewModel extends AnagramOccurrencesResult {
  segments: TextSegment[];
}

@Component({
  selector: 'app-anagram-occurrences',
  standalone: true,
  imports: [ReactiveFormsModule, AsyncPipe],
  templateUrl: './anagram-occurrences.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnagramOccurrencesComponent {
  private readonly api = inject(AnagramApiService);

  readonly form = inject(FormBuilder).nonNullable.group({
    text: ['', textValidators],
    word: ['', wordValidators],
  });

  private readonly searches$ = new Subject<{ text: string; word: string }>();

  readonly result$ = this.searches$.pipe(
    switchMap(({ text, word }) =>
      this.api.findOccurrences(text, word).pipe(
        map((result): OccurrencesViewModel => ({
          ...result,
          segments: toSegments(result.text, result.occurrences, result.word.length),
        })),
        toViewState<OccurrencesViewModel>(),
      ),
    ),
  );

  get text() {
    return this.form.controls.text;
  }

  get word() {
    return this.form.controls.word;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { text, word } = this.form.getRawValue();
    this.searches$.next({ text: text.trim(), word: word.trim() });
  }
}
