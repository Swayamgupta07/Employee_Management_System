import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Attendance, CreateAttendance, UpdateAttendance } from '../models/attendance.model';

@Injectable({ providedIn: 'root' })
export class AttendanceService {
  private apiUrl = `${environment.apiUrl}/attendance`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Attendance[]> {
    return this.http.get<Attendance[]>(this.apiUrl);
  }

  getById(id: number): Observable<Attendance> {
    return this.http.get<Attendance>(`${this.apiUrl}/${id}`);
  }

  getByEmployee(employeeId: number): Observable<Attendance[]> {
    return this.http.get<Attendance[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  getByDateRange(start: string, end: string): Observable<Attendance[]> {
    const params = new HttpParams().set('start', start).set('end', end);
    return this.http.get<Attendance[]>(`${this.apiUrl}/daterange`, { params });
  }

  getMonthlyReport(month: number, year: number): Observable<Attendance[]> {
    const params = new HttpParams().set('month', month).set('year', year);
    return this.http.get<Attendance[]>(`${this.apiUrl}/monthlyreport`, { params });
  }

  create(dto: CreateAttendance): Observable<any> {
    return this.http.post<any>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateAttendance): Observable<Attendance> {
    return this.http.put<Attendance>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
