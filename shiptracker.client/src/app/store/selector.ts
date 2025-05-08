import { createFeatureSelector, createSelector } from "@ngrx/store";
import { CountryState } from "./reducer";

export const selectCountryState = createFeatureSelector<CountryState>('countries');

export const selectCountries = createSelector(
  selectCountryState,
  (state: CountryState) => state.countries
);

export const selectLoading = createSelector(
  selectCountryState,
  (state: CountryState) => state.loading
);
