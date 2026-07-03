import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ReportService {
  private apiUrl = `${environment.apiUrl}/reports`;

  constructor(private http: HttpClient) {}

  downloadEmployeeExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/employees/excel`, { responseType: 'blob' });
  }

  downloadEmployeePdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/employees/pdf`, { responseType: 'blob' });
  }

  triggerDownload(blob: Blob, filename: string): void {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  }
}
