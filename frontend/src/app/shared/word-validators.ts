import { Validators } from '@angular/forms';

// Kept in step with Anagram.Api.Contracts.ValidationRules on the server.
export const MAX_WORD_LENGTH = 50;
export const MAX_TEXT_LENGTH = 1000;
export const WORD_PATTERN = /^\s*[A-Za-zÀ-ÿ']+\s*$/;

export const wordValidators = [
  Validators.required,
  Validators.maxLength(MAX_WORD_LENGTH),
  Validators.pattern(WORD_PATTERN),
];

export const textValidators = [Validators.required, Validators.maxLength(MAX_TEXT_LENGTH)];
