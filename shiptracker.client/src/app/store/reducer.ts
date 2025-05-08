import { createReducer, on } from "@ngrx/store";  
import { Country } from "./model";  
import { loadCountries, loadCountriesSuccess } from "./action";  

export interface CountryState {  
 countries: Country[];  
 loading: boolean;  
}  

export const initialState: CountryState = {  
 countries: [],  
 loading: false,  
};  

export const countryReducer = createReducer(  
 initialState,  
 on(loadCountries, (state) => ({  
   ...state,  
   loading: true,  
 })),  
 on(loadCountriesSuccess, (state, { countries }) => ({  
   ...state,  
   countries,  
   loading: false,  
 }))  
);
