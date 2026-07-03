import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { HiringTrend, DepartmentGrowth, AttendancePattern, PerformanceMetrics } from '../models/bonus.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class BonusFeatureService {
  private apiUrl = `${environment.apiUrl}/bonusfeatures`;

  constructor(private http: HttpClient) { }

  private buildParams(start: string, end: string): HttpParams {
    return new HttpParams().set('start', start).set('end', end);
  }

  private handleError<T>() {
    return (error: any): Observable<ApiResponse<T>> => {
      // If 404 or other error, return a graceful empty response so forkJoin doesn't break
      return of({
        success: false,
        message: error.error?.message || 'Data not found',
        data: [] as unknown as T
      } as ApiResponse<T>);
    };
  }

  getHiringTrends(start: string, end: string): Observable<ApiResponse<HiringTrend[]>> {
    return this.http.get<ApiResponse<HiringTrend[]>>(`${this.apiUrl}/hiring-trends`, {
      params: this.buildParams(start, end)
    }).pipe(catchError(this.handleError<HiringTrend[]>()));
  }

  getDepartmentGrowth(start: string, end: string): Observable<ApiResponse<DepartmentGrowth[]>> {
    return this.http.get<ApiResponse<DepartmentGrowth[]>>(`${this.apiUrl}/department-growth`, {
      params: this.buildParams(start, end)
    }).pipe(catchError(this.handleError<DepartmentGrowth[]>()));
  }

  getAttendancePatterns(start: string, end: string): Observable<ApiResponse<AttendancePattern[]>> {
    return this.http.get<ApiResponse<AttendancePattern[]>>(`${this.apiUrl}/attendance-patterns`, {
      params: this.buildParams(start, end)
    }).pipe(catchError(this.handleError<AttendancePattern[]>()));
  }

  getPerformanceMetrics(start: string, end: string): Observable<ApiResponse<PerformanceMetrics[]>> {
    return this.http.get<ApiResponse<PerformanceMetrics[]>>(`${this.apiUrl}/performance-metrics`, {
      params: this.buildParams(start, end)
    }).pipe(catchError(this.handleError<PerformanceMetrics[]>()));
  }
}
