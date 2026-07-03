export interface Department {
  id: number;
  name: string;
  description: string;
  employeeCount?: number;
}

export interface CreateDepartment {
  name: string;
  description: string;
}

export interface UpdateDepartment {
  id: number;
  name: string;
  description: string;
}
