import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { tap } from 'rxjs';
import { AppConstants } from '../constants/app.constants';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly API = environment.apiUrl;

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post<any>(
      `${this.API}/${AppConstants.API_ENDPOINTS.AUTH}/login`,
      { username, password }
    ).pipe(
      tap(res => {
       const token = btoa(`${username}:${password}`);
        localStorage.setItem(
          AppConstants.STORAGE_KEYS.AUTH_KEY,
          token
        );
        localStorage.setItem(
          AppConstants.STORAGE_KEYS.USER_ID,
          res.userId.toString()
        );
        localStorage.setItem(
          AppConstants.STORAGE_KEYS.USER_NAME,
          res.userName.toString()
        );
      })
    );
  }  
}
