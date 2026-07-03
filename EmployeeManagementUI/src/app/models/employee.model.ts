export interface Employee {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position: string;
  departmentId: number;
  departmentName: string;
  salary: number;
  hireDate: string;
  isActive: boolean;
  profilePictureUrl?: string;
}

export interface CreateEmployee {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position: string;
  departmentId: number;
  salary: number;
  hireDate: string;
  profilePictureUrl?: string;
}

export interface UpdateEmployee {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position: string;
  departmentId: number;
  salary: number;
  hireDate: string;
  profilePictureUrl?: string;
}

export interface SearchEmployee {
  name?: string;
  email?: string;
  departmentId?: number;
  joinedAfter?: string;
}
