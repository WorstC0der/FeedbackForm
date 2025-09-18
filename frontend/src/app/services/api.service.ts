import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  private baseUrl = 'https://feedbackformbackend-431l.onrender.com/api';

  getTopics(): Observable<Topic[]> {
    return this.http.get<Topic[]>(`${this.baseUrl}/topics`);
  }

  createMessage(payload: MessageCreate): Observable<MessageResponse> {
    return this.http.post<MessageResponse>(`${this.baseUrl}/messages`, payload, {
      withCredentials: true,
    });
  }

  validateCaptcha(code: string) {
    return this.http.post<{ ok: boolean }>(
      `${this.baseUrl}/captcha/validate`,
      { code },
      { withCredentials: true }
    );
  }

  getCaptchaUrl(): string {
    return `${this.baseUrl}/captcha?ts=${Date.now()}`;
  }

  getCaptchaBlob(): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/captcha?ts=${Date.now()}`, {
      responseType: 'blob',
      withCredentials: true,
    });
  }
}
