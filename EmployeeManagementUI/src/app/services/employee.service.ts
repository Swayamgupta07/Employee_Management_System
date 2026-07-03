import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Employee, CreateEmployee, UpdateEmployee, SearchEmployee } from '../models/employee.model';
import { ApiResponse, PagedResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private apiUrl = `${environment.apiUrl}/employees`;

  constructor(private http: HttpClient) {}

  getAll(pageNumber = 1, pageSize = 10): Observable<PagedResponse<Employee>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<PagedResponse<Employee>>(this.apiUrl, { params });
  }

  getById(id: number): Observable<ApiResponse<Employee>> {
    return this.http.get<ApiResponse<Employee>>(`${this.apiUrl}/${id}`);
  }

  search(criteria: SearchEmployee): Observable<ApiResponse<Employee[]>> {
    let params = new HttpParams();
    if (criteria.name) params = params.set('name', criteria.name);
    if (criteria.email) params = params.set('email', criteria.email);
    if (criteria.departmentId) params = params.set('departmentId', criteria.departmentId);
    if (criteria.joinedAfter) params = params.set('joinedAfter', criteria.joinedAfter);
    return this.http.get<ApiResponse<Employee[]>>(`${this.apiUrl}/search`, { params });
  }

  create(dto: CreateEmployee): Observable<ApiResponse<Employee>> {
    return this.http.post<ApiResponse<Employee>>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateEmployee): Observable<ApiResponse<Employee>> {
    return this.http.put<ApiResponse<Employee>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/${id}`);
  }

  deactivate(id: number): Observable<ApiResponse<string>> {
    return this.http.patch<ApiResponse<string>>(`${this.apiUrl}/deactivate/${id}`, {});
  }
}
