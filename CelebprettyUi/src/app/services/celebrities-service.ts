import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';
import { catchError} from 'rxjs/operators';
import { Celebrity } from '../models/celebrity';


@Injectable({ providedIn: 'root' })
export class CelebritiesService {

  private celebritiesUrl = 'http://localhost:5055/api/v1/celebrities';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json', "Access-Control-Allow-Origin" : "*"  })
  };

  constructor(private http: HttpClient) {}

  getCelebrities(): Observable<Celebrity[]> {
    return this.http.get<Celebrity[]>(this.celebritiesUrl)
      .pipe(
        catchError((err, caught) => {
            console.log(`Error occured during receiving celebrities ${err}`);
            return caught;
        })
      );
  }

  deleteCelebrity(celebrity: Celebrity): Observable<Object> {
    return this.http.delete(`${this.celebritiesUrl}/${celebrity.id}`).pipe(
      catchError((err, caught) => {
          console.log(`Error occured during deleting celebrities ${err}`);
          return caught;
      })
    )
  }

  reset(): Observable<Object> {
    return this.http.get(`${this.celebritiesUrl}/reset`)
      .pipe(
        catchError((err, caught) => {
            console.log(`Error occured during reset ${err}`);
            return caught;
        })
      );
  }
}