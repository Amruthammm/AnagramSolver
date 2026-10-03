import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { AnagramApiService } from '../../core/services/anagram-api.service';
import { MatchingAnagramsComponent } from './matching-anagrams.component';

describe('MatchingAnagramsComponent', () => {
  let fixture: ComponentFixture<MatchingAnagramsComponent>;
  let component: MatchingAnagramsComponent;
  let api: jasmine.SpyObj<AnagramApiService>;

  beforeEach(async () => {
    api = jasmine.createSpyObj<AnagramApiService>('AnagramApiService', ['findMatchingAnagrams']);

    await TestBed.configureTestingModule({
      imports: [MatchingAnagramsComponent],
      providers: [{ provide: AnagramApiService, useValue: api }],
    }).compileComponents();

    fixture = TestBed.createComponent(MatchingAnagramsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('does not call the API and shows a message when the word is invalid', () => {
    component.form.setValue({ word: 'd0gs' });
    component.submit();
    fixture.detectChanges();

    expect(api.findMatchingAnagrams).not.toHaveBeenCalled();
    expect(fixture.nativeElement.textContent).toContain('Use letters only');
  });

  it('renders the anagrams returned by the API', () => {
    api.findMatchingAnagrams.and.returnValue(of({ input: 'dogs', anagrams: ['gods'], count: 1 }));

    component.form.setValue({ word: '  dogs ' });
    component.submit();
    fixture.detectChanges();

    expect(api.findMatchingAnagrams).toHaveBeenCalledWith('dogs');
    const items = fixture.nativeElement.querySelectorAll('[data-testid="anagram-list"] li');
    expect(Array.from(items, (li: Element) => li.textContent?.trim())).toEqual(['gods']);
  });
});
