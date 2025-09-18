import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Topic {
  id: number;
  name: string;
}
export interface MessageCreate {
  name: string;
  email: string;
  phone: string;
  topic_id: number;
  text: string;
  captcha?: string | null;
}

export interface MessageResponse {
  id: number;
  contact_id: number;
  topic_id: number;
  text: string;
  created_at: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7180/api';

  getTopics(): Observable<Topic[]> {
    return this.http.get<Topic[]>(`${environment.apiUrl}/topics`);
  }

  createMessage(payload: MessageCreate): Observable<MessageResponse> {
    return this.http.post<MessageResponse>(`${environment.apiUrl}/messages`, payload, {
      withCredentials: true,
    });
  }

  validateCaptcha(code: string) {
    return this.http.post<{ ok: boolean }>(
      `${environment.apiUrl}/captcha/validate`,
      { code },
      { withCredentials: true }
    );
  }

  getCaptchaUrl(): string {
    return `${environment.apiUrl}/captcha?ts=${Date.now()}`;
  }
}
