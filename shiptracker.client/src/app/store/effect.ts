import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CountryService } from "../service/country.service";
import { catchError, map, mergeMap, of } from "rxjs";
import { loadCountries, loadCountriesSuccess } from "./action";

@Injectable()
export class CountryEffect {
  action$ = inject(Actions);
  constructor(
    private countryService: CountryService
  ) { };

  loadCountries$ = createEffect(() => 
    this.action$.pipe(
      ofType(loadCountries),
      mergeMap(() =>
        this.countryService.getCountries().pipe(
          map((countries: any) => loadCountriesSuccess({ countries }))
        )
      )
    )
  );


};
