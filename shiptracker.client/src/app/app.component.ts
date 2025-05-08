import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { loadCountries } from './store/action';
import { selectCountries, selectLoading } from './store/selector';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: false
})
export class AppComponent implements OnInit {
  title = 'shiptracker.client';

  countries$!: Observable<any>;
  loading$!: Observable<boolean>;

  constructor(private http: HttpClient, private store: Store) { }

  ngOnInit() {
    this.store.dispatch(loadCountries());
    this.countries$ = this.store.select(selectCountries);
    this.loading$ = this.store.select(selectLoading);
  }
}
