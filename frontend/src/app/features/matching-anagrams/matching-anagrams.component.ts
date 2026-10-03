import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Subject, switchMap } from 'rxjs';

import { MatchingAnagramsResult } from '../../core/models/anagram.models';
import { AnagramApiService } from '../../core/services/anagram-api.service';
import { toViewState } from '../../core/state/view-state';
import { wordValidators } from '../../shared/word-validators';

@Component({
  selector: 'app-matching-anagrams',
  standalone: true,
  imports: [ReactiveFormsModule, AsyncPipe],
  templateUrl: './matching-anagrams.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchingAnagramsComponent {
  private readonly api = inject(AnagramApiService);

  /**
   * reactive form builder 
   * */
  readonly form = inject(FormBuilder).nonNullable.group({
    word: ['', wordValidators],
  });

  /** searches is an observable we can push values into it */
  private readonly searches$ = new Subject<string>();

  /**
   * Each submitted word becomes an API call. switchMap(RxJS) cancels an in-flight
   * request if the user searches again, so results(observable) never arrive out of order.
   */
  readonly result$ = this.searches$.pipe(
    switchMap((word) => this.api.findMatchingAnagrams(word).pipe(toViewState<MatchingAnagramsResult>())),
  );

  get word() {
    return this.form.controls.word;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.searches$.next(this.word.value.trim());
  }
}
