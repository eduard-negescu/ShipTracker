import { createAction, props } from "@ngrx/store";  
import { Country } from "./model";  

export const loadCountries = createAction("[Countries] Load Countries");  

export const loadCountriesSuccess = createAction(  
 "[Countries] Load Countries Success",  
 props<{ countries: Country[] }>()  
);
